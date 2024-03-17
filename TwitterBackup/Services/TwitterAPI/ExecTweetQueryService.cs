using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TinyOAuth1;
using TwitterBackup.Services.DataModels;

namespace TwitterBackup.Services.TwitterAPI
{
    public class ExecTweetQueryService : IExecTweetQueryService
    {
        private readonly IConfiguration _config;
        private readonly string _baseUrl;

        public ExecTweetQueryService(IConfiguration config)
        {
            _config = config;
            _baseUrl = _config.GetValue<string>("AppSettings:TwitterBaseUrl");
        }

        public HttpClient ConnectToTwitterAPI()
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(_config.GetValue<string>("AppSettings:TwitterBaseUrl"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _config.GetValue<string>("AppSettings:TwitterBearerToken"));
            
            return client;
        }

        public async Task<TwitterUserResults> SearchTwitterUsers(string userName)
        {
            TwitterUserResults searchResult = new TwitterUserResults();
            HttpClient httpclient = ConnectToTwitterAPI();
            HttpResponseMessage response = await httpclient.GetAsync($"{_baseUrl}users/by/username/{userName}");

            if (response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    var json = await reader.ReadToEndAsync();
                    searchResult = JsonSerializer.Deserialize<TwitterUserResults>(json);
                }
            }
            
            return searchResult;
        }

        public async Task<UserTweetsResults> SearchUserTweets(string userName)
        {
            UserTweetsResults searchResult = new UserTweetsResults();
            HttpClient httpclient = ConnectToTwitterAPI();

            var url = $"{_baseUrl}tweets/search/recent?query=from:{userName}&" +
                      $"max_results={_config.GetValue<string>("AppSettings:TwitterMaxSearchResults")}&" +
                      $"tweet.fields=created_at&expansions=author_id&user.fields=created_at";

            HttpResponseMessage response = await httpclient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    var json = await reader.ReadToEndAsync();
                    searchResult = JsonSerializer.Deserialize<UserTweetsResults>(json);
                    searchResult.UserName = userName;
                }
            }

            return searchResult;
        }

        public async Task<string> RequestToken()
        {
            string requestToken = null;
            var consumerKey = _config.GetValue<string>("AppSettings:TwitterAPIKey");
            var url = $"https://api.twitter.com/oauth/request_token?oauth_callback=oob&oauth_callback_confirmed=true&oauth_consumer_key={consumerKey}&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1650965456&oauth_nonce=mbOI71LhNOD&oauth_version=1.0&oauth_signature=egPFUp7rvIjF2I3fDmtAMeSCcsM%3D";

            HttpClient httpclient = ConnectToTwitterAPI();

            StringContent httpContent = new StringContent("", System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpclient.PostAsync(url, httpContent);
            
            if (response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    requestToken = await reader.ReadToEndAsync();
                }
            }

            return requestToken;
        }

        public async Task<string> Authorize(string requestToken)
        {
            string html = null;
            var url = $"https://api.twitter.com/oauth/authorize?{requestToken}";

            HttpClient httpclient = ConnectToTwitterAPI();

            HttpResponseMessage response = await httpclient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    html = await reader.ReadToEndAsync();
                }
            }

            return html;
        }

        public async Task<string> GetAccessToken(string oauth_token, string oauth_verifier)
        {
            string result = null;
            var tokens = oauth_token.Split('&')[0];
            var url = $"https://api.twitter.com/oauth/access_token?oauth_verifier={oauth_verifier}&{tokens}";

            HttpClient httpclient = ConnectToTwitterAPI();

            HttpResponseMessage response = await httpclient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                using (var reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    result = await reader.ReadToEndAsync();
                }
            }

            return result;
        }

        public async Task<string> RetweetTweet(string tokens, string tweetId)
        {          
            string res = "";
            string userId = _config.GetValue<string>("AppSettings:TwitterAPIUselId");
            string accessToken= _config.GetValue<string>("AppSettings:AccessToken");
            string accessTokenSecret = _config.GetValue<string>("AppSettings:AccessTokenSecret");

            if (!string.IsNullOrEmpty(tokens))
            {
                var tarray = tokens.Split("&");
                accessToken = tarray[0].Split("=")[1];
                accessTokenSecret = tarray[1].Split("=")[1];
                userId = tarray[2].Split("=")[1];
            }

            var config = new TinyOAuthConfig
            {
                AccessTokenUrl = "https://api.provider.com/oauth/accessToken",
                AuthorizeTokenUrl = "https://api.provider.com/oauth/authorize",
                RequestTokenUrl = "https://api.provider.com/oauth/requestToken",
                ConsumerKey = _config.GetValue<string>("AppSettings:TwitterAPIKey"),
                ConsumerSecret = _config.GetValue<string>("AppSettings:TwitterAPIKeySecret")
            };

            var tinyOAuth = new TinyOAuth(config);
            var oauthHeader = tinyOAuth.GetAuthorizationHeader(accessToken, accessTokenSecret, $"https://api.twitter.com/2/users/{userId}/retweets", HttpMethod.Post);

            var clientHandler = new HttpClientHandler { UseCookies = false };

            var client = new HttpClient(clientHandler);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://api.twitter.com/2/users/{userId}/retweets"),
                Headers =
                {
                    {"Authorization", $"{oauthHeader.Scheme} {oauthHeader.Parameter}"}
                },
                Content = new StringContent("{\n \"tweet_id\": \"" + tweetId + "\"\n}")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                if (!response.IsSuccessStatusCode) res = "Unauthorize!";
                if (response.IsSuccessStatusCode) res = "Retweet complete successfully!";                
            }

            return res;
        }
    }
}

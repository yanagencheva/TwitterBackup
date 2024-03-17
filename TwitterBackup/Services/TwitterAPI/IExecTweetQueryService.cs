using System.Net.Http;
using System.Threading.Tasks;
using TwitterBackup.Services.DataModels;

namespace TwitterBackup.Services.TwitterAPI
{
    public interface IExecTweetQueryService
    {
        HttpClient ConnectToTwitterAPI();
        Task<TwitterUserResults> SearchTwitterUsers(string userName);     
        Task<UserTweetsResults> SearchUserTweets(string userName);
        Task<string> RequestToken();
        Task<string> Authorize(string requestToken);
        Task<string> GetAccessToken(string oauth_token, string oauth_verifier);
        Task<string> RetweetTweet(string tokens, string tweetId);
    }
}

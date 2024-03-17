using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TwitterBackup.Models;
using TwitterBackup.Services.DataModels;
using TwitterBackup.Services.LocalDBTweets;
using TwitterBackup.Services.LocalDBUsers;
using TwitterBackup.Services.TwitterAPI;

namespace TwitterBackup.Controllers
{
    public class LocalDBTweetsController : BaseController
    {
        private readonly ILocalDBTweetsService _localDBTweetsService;
        private readonly IExecTweetQueryService _execSearchQuery;

        public LocalDBTweetsController(UserManager<IdentityUser> userManager, ILocalDBTweetsService localDBTweetsService,
                                       ILocalDBUsersService localDBUserService, IExecTweetQueryService execSearchQuery) : base(userManager, localDBTweetsService, localDBUserService, execSearchQuery)
        {
            _localDBTweetsService = localDBTweetsService;
            _execSearchQuery = execSearchQuery;
        }

        public IActionResult Index()
        {
            var model = new LocalDBTweetsViewModel();

            model.Tweets = _localDBTweetsService.GetAll(currentUser).ToList().Select(x => new LocalDBTweetDetailsViewModel
            {
                Id = x.TweetId,
                Text = x.Text,
                Date = x.Date,
                BrowsingTweetId = x.BrowsingTweetId,
                UserName = x.TwitterUserName,
                Favourite = x.Favourite == true ? true : false
            });

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery] int id)
        {
            var model = new LocalDBTweetDetailsViewModel();

            var loctweet = _localDBTweetsService.Get(id);

            model.Id = loctweet.TweetId;
            model.Text = loctweet.Text;
            model.Favourite = loctweet.Favourite;
            model.BrowsingTweetId = loctweet.BrowsingTweetId;
            model.UserName = loctweet.TwitterUserName;
            model.Date = loctweet.Date;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LocalDBTweetDetailsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            LocalDBTweetsDetails td = new LocalDBTweetsDetails();

            td.Text = model.Text;
            td.Favourite = model.Favourite;
            td.TweetId = model.Id;
            td.Date = model.Date;
            td.BrowsingTweetId = model.BrowsingTweetId;
            td.TwitterUserName = model.UserName;

            _localDBTweetsService.Edit(td);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete([FromQuery] int id)
        {
            _localDBTweetsService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Filter(LocalDBTweetsViewModel model)
        {
            var tweetsList = _localDBTweetsService.FilterTweets(new TweetFilterModel { UserName = model.UserName, Favourite = model.Favourite }, currentUser).ToList();

            model.Tweets = tweetsList.Select(x => new LocalDBTweetDetailsViewModel
            {
                Id = x.TweetId,
                Text = x.Text,
                Date = x.Date,
                BrowsingTweetId = x.BrowsingTweetId,
                UserName = x.TwitterUserName,
                Favourite = x.Favourite == true ? true : false
            });

            return View("Index", model);
        }

        #region OAuth1Tokens
        [HttpGet]
        public async Task<IActionResult> RequestToken([FromQuery] string id)
        {
            var rtoken = await _execSearchQuery.RequestToken();
            var result = new { tweetId = id, tokenData = rtoken };

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Retweet([FromBody] RetweetsViewModel model)
        {
            var rtoken = await _execSearchQuery.GetAccessToken(model.token, model.pin);
            var retresult = await _execSearchQuery.RetweetTweet(rtoken, model.tweetId);

            return Json(retresult);
        }
        #endregion
    }
}

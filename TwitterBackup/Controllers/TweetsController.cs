using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitterBackup.Models;
using TwitterBackup.Services.DataModels;
using TwitterBackup.Services.LocalDBTweets;
using TwitterBackup.Services.LocalDBUsers;
using TwitterBackup.Services.TwitterAPI;

namespace TwitterBackup.Controllers
{
    public class TweetsController : BaseController
    {
        private readonly ILocalDBTweetsService _localtweetsService;
        private readonly ILocalDBUsersService _localusersService;
        private readonly IExecTweetQueryService _execSeach;

        public TweetsController(UserManager<IdentityUser> userManager, ILocalDBTweetsService localDBTweetsService,
                                ILocalDBUsersService localDBUserService, IExecTweetQueryService execSearchQuery) : base(userManager, localDBTweetsService, localDBUserService, execSearchQuery)
        {
            _localtweetsService = localDBTweetsService;
            _localusersService = localDBUserService;
            _execSeach = execSearchQuery;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SearchTweets([FromQuery] string username)
        {
            var model = new TweetsViewModel();

            model.IsUserInclusedToLocalDB = _localusersService.HasUserInLocalDB(username, currentUser);

            var result = await _execSeach.SearchUserTweets(username);

            if (result != null)
            {
                model.Results = result.data.Select(x => new TweetsDetailsViewModel
                {
                    BrowsingTweetId = x.id,
                    Text = x.text,
                    BrowsingTwitterUserName = result.UserName,
                    BrowsingTwitterUserId = x.author_id,
                    Date = $"{DateTime.Parse(x.created_at).Date.ToShortDateString()}"
                }).ToList();
            }

            return PartialView("_UserTweets", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TweetsDetailsViewModel model)
        {
            if (!ModelState.IsValid) return Json("Incorrect data!");

            _localtweetsService.Add(new TweetDetails
            {
                Text = model.Text,
                TwitterUserId = model.TwitterUserId,
                BrowsingTwitterUserName = model.BrowsingTwitterUserName,
                BrowsingTwitterName = model.BrowsingTwitterName,
                BrowsingTweetId = model.BrowsingTweetId,
                Favourite = model.Favourite,
                Date = model.Date
            }, currentUser);

            return Json("Tweet saved!");
        }
    }
}

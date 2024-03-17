using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using TwitterBackup.Models;
using TwitterBackup.Services.DataModels;
using TwitterBackup.Services.LocalDBTweets;
using TwitterBackup.Services.LocalDBUsers;
using TwitterBackup.Services.TwitterAPI;

namespace TwitterBackup.Controllers
{
    public class TwitterUsersController : BaseController
    {
        private readonly ILocalDBUsersService _usersService;
        private readonly IExecTweetQueryService _execSeach;

        public TwitterUsersController(UserManager<IdentityUser> userManager, ILocalDBTweetsService localDBTweetsService,
                                      ILocalDBUsersService localDBUserService, IExecTweetQueryService execSearchQuery) : base(userManager, localDBTweetsService, localDBUserService, execSearchQuery)
        {
            _usersService = localDBUserService;
            _execSeach = execSearchQuery;
        }

        public IActionResult Index()
        {
            var model = new BrowsingTwitterUsersViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BrowseTwitterUsers(BrowsingTwitterUsersViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _execSeach.SearchTwitterUsers(model.TwitterUserName).Result.data;

                var listResults = new List<TwitterUserViewModel>();

                if (result != null) listResults.Add(new TwitterUserViewModel
                {
                    Id = result.id,
                    Name = result.name,
                    UserName = result.username
                });

                model.Results = listResults;
            }

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery] string userName)
        {
            var model = new TwitterUserViewModel();

            if (!string.IsNullOrEmpty(userName))
            {
                var result = _execSeach.SearchTwitterUsers(userName).Result.data;

                model.Id = result.id;
                model.Name = result.name;
                model.UserName = result.username;
            }

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TwitterUserViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            _usersService.Add(currentUser, new TwitterUserDetails { Id = model.Id, Name = model.Name, UserName = model.UserName });

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

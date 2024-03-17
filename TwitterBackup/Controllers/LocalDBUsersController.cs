using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using TwitterBackup.Models;
using TwitterBackup.Services.DataModels;
using TwitterBackup.Services.LocalDBTweets;
using TwitterBackup.Services.LocalDBUsers;
using TwitterBackup.Services.TwitterAPI;

namespace TwitterBackup.Controllers
{
    public class LocalDBUsersController : BaseController
    {
        private readonly ILocalDBUsersService _localDBUserService;

        public LocalDBUsersController(UserManager<IdentityUser> userManager, ILocalDBTweetsService localDBTweetsService,
                                      ILocalDBUsersService localDBUserService, IExecTweetQueryService execSearchQuery) : base(userManager, localDBTweetsService, localDBUserService, execSearchQuery)
        {
            _localDBUserService = localDBUserService;
        }

        public IActionResult Index()
        {
            var model = new LocalDBUsersViewModel();

            model.Users = _localDBUserService.GetAll(currentUser).ToList().Select(x => new LocalDBUserDetailsViewModel
            {
                Id = x.UserId,
                Name = x.Name,
                UserName = x.UserName
            });

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery] int id)
        {
            var model = new LocalDBUserDetailsViewModel();

            var locuser = _localDBUserService.Get(id);

            if (locuser == null) return NotFound();

            model.Name = locuser.Name;
            model.UserName = locuser.UserName;
            model.Id = locuser.UserId;
            model.Avatar = locuser.Avatar;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(LocalDBUserDetailsViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            LocalDBUserDetails ud = new LocalDBUserDetails();

            ud.Name = model.Name;
            ud.UserName = model.UserName;
            ud.UserId = model.Id;

            if (model.ImageFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    model.ImageFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    ud.Avatar = fileBytes;
                    model.Avatar = fileBytes;
                }
            }

            _localDBUserService.Edit(ud);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete([FromQuery] int id)
        {
            if (id == 0) return NotFound();

            _localDBUserService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}

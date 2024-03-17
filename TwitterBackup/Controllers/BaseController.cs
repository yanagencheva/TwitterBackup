using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TwitterBackup.Helpers.UserManagement;
using TwitterBackup.Services.DataModels;
using TwitterBackup.Services.LocalDBTweets;
using TwitterBackup.Services.LocalDBUsers;
using TwitterBackup.Services.TwitterAPI;

namespace TwitterBackup.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IExecTweetQueryService _execSearchQuery;
        private readonly ILocalDBTweetsService _localDBTweetsService;
        private readonly ILocalDBUsersService _localDBUserService;
        protected CurrentUserDetails currentUser;

        public BaseController(UserManager<IdentityUser> userManager, ILocalDBTweetsService localDBTweetsService, 
                              ILocalDBUsersService localDBUserService, IExecTweetQueryService execSearchQuery)
        {
            _userManager = userManager;
            _localDBTweetsService = localDBTweetsService;
            _localDBUserService = localDBUserService;
            _execSearchQuery = execSearchQuery;
        }
       
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            currentUser = UserHelper.CurrentUser(_userManager, User);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TwitterBackup.Services.DataModels;
using static TwitterBackup.Constants.AuthorizationConstatnts;

namespace TwitterBackup.Helpers.UserManagement
{
    public static class UserHelper
    {
        public static CurrentUserDetails CurrentUser(UserManager<IdentityUser> userManager, ClaimsPrincipal user)
        {
            var currentUserId = userManager.GetUserId(user);
            var isAdmin = false;

            if (user.IsInRole(Roles.ADMINISTRATORS)) isAdmin = true;

            return new CurrentUserDetails
            {
                UserId = currentUserId,
                IsAdmin = isAdmin
            };
        }
    }
}

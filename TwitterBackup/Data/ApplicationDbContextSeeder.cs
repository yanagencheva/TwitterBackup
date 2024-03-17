using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TwitterBackup.Constants;

namespace TwitterBackup.Data
{
    public static class ApplicationDbContextSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstatnts.Roles.ADMINISTRATORS));
            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstatnts.Roles.USERS));

            var adminUsername = "admin";
            var adminUser = new IdentityUser
            {
                UserName = adminUsername,
                Email = $"{adminUsername}@domain.com",
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            await userManager.CreateAsync(adminUser, AuthorizationConstatnts.DEFAULT_PASSWORD);

            adminUser = await userManager.FindByNameAsync(adminUsername);
            await userManager.AddToRoleAsync(adminUser, AuthorizationConstatnts.Roles.ADMINISTRATORS);

            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstatnts.Roles.USERS));
            var basicUsername = "basicuser";
            var basicUser = new IdentityUser
            {
                UserName = basicUsername,
                Email = $"{basicUsername}@domain.com",
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            await userManager.CreateAsync(basicUser, AuthorizationConstatnts.DEFAULT_PASSWORD);

            basicUser = await userManager.FindByNameAsync(basicUsername);
            await userManager.AddToRoleAsync(basicUser, AuthorizationConstatnts.Roles.USERS);
        }
    }
}

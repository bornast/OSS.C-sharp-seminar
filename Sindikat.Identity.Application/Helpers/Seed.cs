using Microsoft.AspNetCore.Identity;
using Sindikat.Identity.Common.Enums;
using Sindikat.Identity.Domain.Entities;
using System;
using System.Linq;

namespace Sindikat.Identity.Application.Helpers
{
    public static class Seed
    {
        public static void SeedCoreData(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (roleManager.Roles.Count() > 0)
                return;

            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<Role> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(Roles)))
            {
                roleManager.CreateAsync(new Role { Name = roleName }).Wait();
            }
        }

        private static void SeedUsers(UserManager<User> userManager)
        {
            var user = new User
            {
                UserName = "Admin",
                Email = "Admin@gmail.com"
            };

            userManager.CreateAsync(user, password: user.UserName).Wait();

            var admin = userManager.FindByNameAsync("Admin").Result;

            userManager.AddToRoleAsync(admin, Enum.GetName(typeof(Roles), Roles.Admin)).Wait();
        }

    }
}

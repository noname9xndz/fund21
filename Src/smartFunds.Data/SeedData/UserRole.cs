using smartFunds.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;
using static smartFunds.Common.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading;

namespace smartFunds.Data.SeedData
{
    public class UserRole : BaseSeedData
    {
        public UserRole(smartFundsDbContext context) : base(context)
        {

        }
        public void Seed()
        {
            var adminUser = new User
            {
                UserName = "admin",
                NormalizedUserName = "admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                FullName = "Administrator"
            };

            var roleStore = new RoleStore<IdentityRole>(Context);

            if (!Context.Roles.Any(x => x.Name == RoleName.Admin))
            {
                roleStore.CreateAsync(new IdentityRole { Name = RoleName.Admin, NormalizedName = RoleName.Admin });
            }
            if (!Context.Roles.Any(x => x.Name == RoleName.Manager))
            {
                roleStore.CreateAsync(new IdentityRole { Name = RoleName.Manager, NormalizedName = "Manager" });
            }
            if (!Context.Roles.Any(x => x.Name == RoleName.Customer))
            {
                roleStore.CreateAsync(new IdentityRole { Name = RoleName.Customer, NormalizedName = "Customer" });
            }

            if (!Context.Users.Any(x => x.UserName == adminUser.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(adminUser, "admin");
                adminUser.PasswordHash = hashed;
                var userStore = new UserStore<User>(Context);
                userStore.CreateAsync(adminUser);

                userStore.AddToRoleAsync(adminUser, RoleName.Admin, CancellationToken.None).Wait();
            }

            Context.SaveChanges();
        }
    }
}

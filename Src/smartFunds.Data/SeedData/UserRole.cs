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
                UserName = "admin@savenow.vn",
                NormalizedUserName = "admin@savenow.vn",
                Email = "admin@savenow.vn",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                FullName = "Administrator"
            };

            if (!Context.Roles.Any(x => x.Name == RoleName.Admin))
            {
                Context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "78d2fb25-5142-45b9-92f0-34d165045d7a", Name = RoleName.Admin, NormalizedName = RoleName.Admin });
                Context.SaveChanges();
            }
            if (!Context.Roles.Any(x => x.Name == RoleName.CustomerManager))
            {
                Context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "1779145a-8576-45b7-929c-f5fbead341a6", Name = RoleName.CustomerManager, NormalizedName = "Customer Manager" });
                Context.SaveChanges();
            }
            if (!Context.Roles.Any(x => x.Name == RoleName.InvestmentManager))
            {
                Context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "2349145a-8576-45b7-929c-f5fbead34caa", Name = RoleName.InvestmentManager, NormalizedName = "Investment Manager" });
                Context.SaveChanges();
            }
            if (!Context.Roles.Any(x => x.Name == RoleName.Accountant))
            {
                Context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "32c9145a-8576-45b7-929c-f5fbead34a4c", Name = RoleName.Accountant, NormalizedName = "Accountant" });
                Context.SaveChanges();
            }
            if (!Context.Roles.Any(x => x.Name == RoleName.Customer))
            {
                Context.Roles.Add(new Microsoft.AspNetCore.Identity.IdentityRole { Id = "871090f7-67b5-48db-9908-1c984db06490", Name = RoleName.Customer, NormalizedName = "Customer" });
                Context.SaveChanges();
            }

            if (!Context.Users.Any(x => x.UserName == adminUser.UserName))
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(adminUser, "admin@1234");
                adminUser.PasswordHash = hashed;
                var userStore = new UserStore<User>(Context);
                userStore.CreateAsync(adminUser);

                userStore.AddToRoleAsync(adminUser, RoleName.Admin, CancellationToken.None).Wait();
            }

            if (!Context.GlobalConfiguration.Any(x=>x.Name == Configuration.IsAdminApproving))
            {
                var isAdminApproving = new GlobalConfiguration();
                isAdminApproving.Name = smartFunds.Common.Constants.Configuration.IsAdminApproving;
                isAdminApproving.Value = "false";
                Context.GlobalConfiguration.Add(isAdminApproving);
                Context.SaveChanges();
            }

            if (!Context.GlobalConfiguration.Any(x => x.Name == Configuration.ProgramLocked))
            {
                var programLocked = new GlobalConfiguration();
                programLocked.Name = smartFunds.Common.Constants.Configuration.ProgramLocked;
                programLocked.Value = "false";
                Context.GlobalConfiguration.Add(programLocked);
                Context.SaveChanges();
            }

            Context.SaveChanges();
        }
    }
}

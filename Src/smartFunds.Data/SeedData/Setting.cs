using smartFunds.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smartFunds.Data.SeedData
{
    public class Setting : BaseSeedData
    {
        public Setting(smartFundsDbContext context) : base(context)
        {

        }
        public void Seed()
        {
            // Non environment specific settings
            if (!Context.Settings.Any(x => x.Key == "TestKey"))
            {
                Context.Settings.Add(new Models.Setting{ Key = "TestKey", Value = "TestValue"});
                Context.SaveChanges();
            }
            
        }
    }
}

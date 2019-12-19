using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Data.SeedData
{
    public static class SeedDataHelper
    {
        public static void Seed(smartFundsDbContext context)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var types =  assembly.GetTypes().Where(t => t.GetTypeInfo().IsSubclassOf(typeof(BaseSeedData))).ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type, new object[] { context });
                var method = type.GetMethod("Seed");
                method.Invoke(instance, new object[0]);
            }
        }
    }
}

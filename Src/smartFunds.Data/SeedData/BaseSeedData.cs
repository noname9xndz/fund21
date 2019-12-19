using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Data.SeedData
{
    public abstract class BaseSeedData
    {
        protected BaseSeedData(smartFundsDbContext context)
        {
            Context = context;
        }
        protected smartFundsDbContext Context { get; set; }

    }
}

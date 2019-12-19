using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Common.Data
{
    public interface IDbContextFactory<Context>
    {
        Context GetContext();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Caching.AutoComplete
{
    public interface IAutoCompleteModel
    {
        AutoCompleteItem AutoCompleteItem { get; }
    }
}

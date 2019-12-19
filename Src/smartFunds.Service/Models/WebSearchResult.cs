using System.Collections.Generic;

namespace smartFunds.Service
{
    public class WebSearchResult<T> where T : class
    {
        public IEnumerable<T> Result { get; set; }
        public int TotalCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class PropertyFluctuations
    {
        public List<PropertyFluctuationItem> ListProperty { get; set; }

        public PropertyFluctuations()
        {
            ListProperty = new List<PropertyFluctuationItem>();
        }
    }

    public class PropertyFluctuationItem
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}

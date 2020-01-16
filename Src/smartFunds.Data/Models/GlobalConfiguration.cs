using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class GlobalConfiguration
    {
        [Key]
        public string Name { set; get; }
        public string Value { set; get; }
    }
}

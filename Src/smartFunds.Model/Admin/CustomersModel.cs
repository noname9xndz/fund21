using Microsoft.AspNetCore.Identity;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Admin
{
    public class CustomersModel : PagingModel
    {
        public CustomersModel()
        {
            Customers = new List<UserModel>();
            TotalCount = 0;
        }
        public List<UserModel> Customers { get; set; }
        public int TotalCount { get; set; }
    }
}

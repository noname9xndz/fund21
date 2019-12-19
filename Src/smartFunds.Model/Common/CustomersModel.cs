using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class CustomersModel : PagingModel
    {
        public CustomersModel()
        {
            Customers = new List<UserModel>();
        }
        public List<UserModel> Customers { get; set; }
    }
}

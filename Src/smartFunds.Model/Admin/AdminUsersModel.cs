using Microsoft.AspNetCore.Identity;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Admin
{
    public class AdminUsersModel : PagingModel
    {
        public List<UserModel> ListUser { get; set; }

        public int TotalCount { get; set; }

        public AdminUsersModel()
        {
            ListUser = new List<UserModel>();
            TotalCount = 0;
        }
        
    }
}

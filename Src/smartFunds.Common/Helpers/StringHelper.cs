using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace smartFunds.Common.Helpers
{
    public static class StringHelper
    {
        public static bool IsEmail(this string str)
        {
            return new EmailAddressAttribute().IsValid(str);
        }

        public static bool IsPhoneNumber(this string str)
        {
            return Regex.Match(str, @"^(0[0-9]{9})$").Success;
        }
    }
}

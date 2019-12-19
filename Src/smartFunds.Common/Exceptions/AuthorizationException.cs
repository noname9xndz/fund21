using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Common.Exceptions
{
    public class AuthorizationException : smartFundsException
    {
        public AuthorizationException() : base("User not authorized")
        {

        }
        public AuthorizationException(string message) : base(message)
        {

        }
    }
}

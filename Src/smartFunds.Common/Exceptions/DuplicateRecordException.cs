using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Common.Exceptions
{
    public class DuplicateRecordException : smartFundsException
    {
        public DuplicateRecordException() : base("Duplicate record found")
        {

        }
        public DuplicateRecordException(string message) : base(message)
        {

        }
    }
}

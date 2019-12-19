using System;

namespace smartFunds.Common.Exceptions
{
    public abstract class smartFundsException : ApplicationException
    {
        public smartFundsException() { }
        public smartFundsException(string message) : base(message)
        {
        }
        public smartFundsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

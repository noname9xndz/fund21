namespace smartFunds.Common.Exceptions
{
    public class InvalidParameterException : smartFundsException
    {
        public InvalidParameterException() : base("Invalid Parameter")
        {

        }
        public InvalidParameterException(string message) : base(message)
        {

        }
    }
}

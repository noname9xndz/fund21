namespace smartFunds.Common.Exceptions
{
    public class MissingParameterException : smartFundsException
    {
        public MissingParameterException() : base("Missing Parameter")
        {

        }
        public MissingParameterException(string message) : base(message)
        {

        }
    }
}

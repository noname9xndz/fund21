namespace smartFunds.Common.Exceptions
{
    public class NotFoundException : smartFundsException
    {
        public NotFoundException(): base("Record not found")
        {

        }
        public NotFoundException(string message) : base(message)
        {

        }
    }
}

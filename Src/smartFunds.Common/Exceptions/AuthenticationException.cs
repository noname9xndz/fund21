namespace smartFunds.Common.Exceptions
{
    public class AuthenticationException : smartFundsException
    {
        public AuthenticationException() : base("User not autenticated")
        {

        }
        public AuthenticationException(string message) : base(message)
        {

        }
    }
}

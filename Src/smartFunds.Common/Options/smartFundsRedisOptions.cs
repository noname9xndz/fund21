namespace smartFunds.Common.Options
{
    public class smartFundsRedisOptions
    {
        public string Host { get; set; }
        public string Endpoint { get; set; }
        public int Port { get; set; }
        public int Database { get; set; }
        public int Expiry { get; set; }
        public bool EnableAutoComplete { get; set; }
    }
}

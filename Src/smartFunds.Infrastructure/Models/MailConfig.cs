using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure.Models
{
    public class MailConfig
    {
        public string EmailFrom { get; set; }
        public string EmailSenderName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string MailTo { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
    }
}

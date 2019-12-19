using smartFunds.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Infrastructure.Services
{
    public interface IEmailSender
    {
        bool SendEmail(MailConfig mailConfig);
    }
}

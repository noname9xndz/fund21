using smartFunds.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        public bool SendEmail(MailConfig mailConfig)
        {
            if (mailConfig == null)
            {
                return false;
            }
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = mailConfig.Host;
                    smtpClient.Port = mailConfig.Port;
                    smtpClient.EnableSsl = mailConfig.EnableSsl;

                    if (!string.IsNullOrWhiteSpace(mailConfig.Username) && !string.IsNullOrWhiteSpace(mailConfig.Password))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(mailConfig.Username,
                                                                       mailConfig.Password);
                    }

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(mailConfig.EmailFrom, mailConfig.EmailSenderName);

                        foreach (var email in mailConfig.MailTo.Split(new char[] { ',', ';', '|' }))
                        {
                            mailMessage.To.Add(email.Trim());
                        }

                        if (!string.IsNullOrEmpty(mailConfig.Cc))
                        {
                            mailMessage.CC.Add(mailConfig.Cc);
                        }

                        if (!string.IsNullOrEmpty(mailConfig.Bcc))
                        {
                            mailMessage.Bcc.Add(mailConfig.Bcc);
                        }

                        mailMessage.Subject = mailConfig.EmailSubject;
                        mailMessage.Body = mailConfig.EmailBody;
                        mailMessage.IsBodyHtml = true;

                        smtpClient.Send(mailMessage);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

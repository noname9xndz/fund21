using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure
{
    public interface ISMSGateway
    {
        void Send(string message);
    }
    public class SMSGateway : ISMSGateway
    {
        public void Send(string message)
        {
            //do something
        }
    }
}

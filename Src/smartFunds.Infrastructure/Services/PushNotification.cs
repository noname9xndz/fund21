using Newtonsoft.Json;
using smartFunds.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace smartFunds.Infrastructure.Services
{
    public interface IPushNotification
    {
        bool Send(string Title, string Content, string AppId, string RestApiKey);
    }
    public class PushNotification : IPushNotification
    {
        public bool Send(string Title, string Content, string AppId, string RestApiKey)
        {
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json; charset=utf-8";

            request.Headers.Add("authorization", "Basic "+ RestApiKey);

            var obj = new
            {
                app_id = AppId,
                contents = new { en = Content },
                headings = new { en = Title },
                included_segments = new string[] { "All" }
            };
            var param = JsonConvert.SerializeObject(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                return true;
            }
            catch (WebException ex)
            {
                throw new ApplicationException("Send Notification Error: " + ex.Message);
            }
        }
    }
}

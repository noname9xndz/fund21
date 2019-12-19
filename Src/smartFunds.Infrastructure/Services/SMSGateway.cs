using smartFunds.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;


namespace smartFunds.Infrastructure.Services
{
    public interface ISMSGateway
    {
        bool Send(SMSConfig smsConfig);
    }
    public class SMSGateway : ISMSGateway
    {
        public bool Send(SMSConfig smsConfig)
        {
            try
            {
                string url = "http://api.esms.vn/MainService.svc/xml/SendMultipleMessage_V4/";
                // declare ascii encoding
                UTF8Encoding encoding = new UTF8Encoding();

                string strResult = string.Empty;

                string customers = string.Empty;

                string[] listPhone = smsConfig.Phone.Split(',');

                foreach (var phone in listPhone)
                {
                    customers = customers + @"<CUSTOMER>"
                                    + "<PHONE>" + phone + "</PHONE>"
                                    + "</CUSTOMER>";
                }

                string SampleXml = @"<RQST>"
                                   + "<APIKEY>" + smsConfig.APIKey + "</APIKEY>"
                                   + "<SECRETKEY>" + smsConfig.SecretKey + "</SECRETKEY>"
                                   + "<ISFLASH>" + (smsConfig.IsFlash ? "1" : "0") + "</ISFLASH>"
                                   + "<BRANDNAME>" + smsConfig.BrandName + "</BRANDNAME>"  //De dang ky brandname rieng vui long lien he hotline 0902435340 hoac nhan vien kinh Doanh cua ban
                                   + "<SMSTYPE>" + smsConfig.SMSType + "</SMSTYPE>"
                                   + "<CONTENT>" + smsConfig.Message + "</CONTENT>"
                                   + "<CONTACTS>" + customers + "</CONTACTS>"
                                   + "</RQST>";

                string postData = SampleXml.Trim().ToString();
                // convert xmlstring to byte using ascii encoding
                byte[] data = encoding.GetBytes(postData);
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "POST";
                webrequest.Timeout = 500000;
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream =
                    new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                // below steps remove unwanted data from response string
                strResult = strResult.Replace("</string>", "");

                var xDoc = XDocument.Parse(strResult);
                var code = xDoc.Descendants("CodeResult").Single();
                var errorMessage = xDoc.Descendants("ErrorMessage").Single();

                if (code.Value == "100")
                {
                    return true;
                }
                else
                {
                    var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                    logger.Error("Send SMS Error: Code: " + code.Value + ", Message: " + errorMessage.Value);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Send SMS Error: " + ex.Message);
            }
            
        }
    }
}

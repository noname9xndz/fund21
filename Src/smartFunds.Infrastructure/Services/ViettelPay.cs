using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace smartFunds.Infrastructure.Services
{
    public interface IViettelPay
    {
        string CheckAccount(string apiURL, string cmd, string rsaPublicKey, string rsaPrivateKey, string rsaPublicKeyVTP, DataCheckAccount dataCheckAccount, SoapDataCheckAccount soapDataCheckAccount);
        string Request(string apiURL, string cmd, string rsaPublicKey, string rsaPrivateKey, string rsaPublicKeyVTP, DataRequestPayment dataRequestPayment, SoapDataRequestPayment soapDataRequestPayment);
    }
    public class ViettelPay : IViettelPay
    {
        public string CheckAccount(string apiURL, string cmd, string rsaPublicKey, string rsaPrivateKey, string rsaPublicKeyVTP, DataCheckAccount dataCheckAccount, SoapDataCheckAccount soapDataCheckAccount)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"" + apiURL + "");
                webRequest.ContentType = "text/xml;charset=UTF-8;action=\"SOAP:Action\"";
                webRequest.Method = "POST";
                webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                var sign = string.Empty;
                var data = GetSoapDataCheckAccount(dataCheckAccount, soapDataCheckAccount, rsaPrivateKey, rsaPublicKey, out sign);
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(cmd, data, sign);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();

                        var xDoc = XDocument.Parse(soapResult);
                        var resultReturn = xDoc.Descendants("return").Single().Value;
                        dynamic resultReturnJson = JObject.Parse(resultReturn);
                        var soapData = resultReturnJson.data.ToString();
                        var soapSign = resultReturnJson.signature.ToString();

                        var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", rsaPublicKeyVTP);
                        var soapDataReplace = soapData.Replace("  \"", "\"").Replace(" \"", "\"").Replace("\r", "").Replace("\n", "");
                        var verifySign = rsa.Verify(soapDataReplace, soapSign);

                        if (!verifySign)
                        {
                            return string.Empty;
                        }

                        dynamic soapDataJson = JObject.Parse(soapData);
                        var dataZip = soapDataJson.data.ToString();
                        var dataUnzip = StringHelper.UnzipBase64(dataZip);

                        if (!verifySign)
                        {
                            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                            logger.Error("ViettelPay Check Account Error: Code: " + soapDataJson.errorCode?.ToString() + ", Message: " + soapDataJson.errorMsg?.ToString() + ", Verify Sign: False");

                            return string.Empty;
                        }

                        if (soapDataJson.errorCode.ToString() != "00")
                        {
                            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                            logger.Error("ViettelPay Check Account Error: Code: " + soapDataJson.errorCode?.ToString() + ", Message: " + soapDataJson.errorMsg?.ToString());
                        }

                        return soapDataJson.errorCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("ViettelPay Check Account Error: " + ex.Message);
            }
        }

        public string Request(string apiURL, string cmd, string rsaPublicKey, string rsaPrivateKey, string rsaPublicKeyVTP, DataRequestPayment dataRequestPayment, SoapDataRequestPayment soapDataRequestPayment)
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@""+ apiURL + "");
                webRequest.ContentType = "text/xml;charset=UTF-8;action=\"SOAP:Action\"";
                webRequest.Method = "POST";
                webRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                var sign = string.Empty;
                var data = GetSoapDataRequestPayment(dataRequestPayment, soapDataRequestPayment, rsaPrivateKey, rsaPublicKey, out sign);
                XmlDocument soapEnvelopeXml = CreateSoapEnvelope(cmd, data, sign);
                var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                logger.Info("ViettelPay Request Payment: Data: " + data);

                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                    {
                        string soapResult = rd.ReadToEnd();

                        var xDoc = XDocument.Parse(soapResult);
                        var resultReturn = xDoc.Descendants("return").Single().Value;
                        dynamic resultReturnJson = JObject.Parse(resultReturn);
                        var soapData = resultReturnJson.data.ToString();
                        var soapSign = resultReturnJson.signature.ToString();

                        var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", rsaPublicKeyVTP);
                        var soapDataReplace = soapData.Replace("  \"", "\"").Replace(" \"", "\"").Replace("\r", "").Replace("\n", "");
                        var verifySign = rsa.Verify(soapDataReplace, soapSign);

                        dynamic soapDataJson = JObject.Parse(soapData);
                        var dataZip = soapDataJson.data.ToString();
                        var dataUnzip = StringHelper.UnzipBase64(dataZip);

                        if (!verifySign)
                        {
                            
                            logger.Error("ViettelPay Request Payment Error: Code: " + soapDataJson.errorCode?.ToString() + ", Message: " + soapDataJson.errorMsg?.ToString() + ", Verify Sign: False");

                            return string.Empty;
                        }

                        if (soapDataJson.errorCode.ToString() != "00")
                        {
                            logger.Error("ViettelPay Request Payment Error: Code: " + soapDataJson.errorCode?.ToString() + ", Message: " + soapDataJson.errorMsg?.ToString());
                        }
                        else
                        {
                            logger.Info("ViettelPay Request Payment: Status: Success");
                        }

                        return soapDataJson.errorCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("ViettelPay Request Error: " + ex.Message);
            }

        }

        private static XmlDocument CreateSoapEnvelope(string cmd, string data, string sign)
        {
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.LoadXml(string.Format(
                @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:par=""http://partnerapi.bankplus.viettel.com/"">
                    <soapenv:Header/>
                    <soapenv:Body>
                        <par:process>
                            <cmd>{0}</cmd>
                            <data>{1}</data>
                            <signature>{2}</signature>
                        </par:process>
                    </soapenv:Body>
                  </soapenv:Envelope>", cmd, data, sign));
            //soapEnvelopeDocument.LoadXml(
            //    @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:par=""http://partnerapi.bankplus.viettel.com/"">
            //            <soapenv:Header/>
            //            <soapenv:Body>
            //                <par:process>
            //                    <cmd>VTP306</cmd>
            //                    <data>{""totalAmount"":""2010"",""totalTrans"":""3"",""transContent"":""Chi luong Thang 8"",""data"":""H4sIAAAAAAAAAIuuViopSswr9kxRslJQMlfSUcotzixOyQPxLEwsLYzNjUxNjUzNgBLJpcUl+bmpRX6JualKVkrB/n4Kfu6hka5+QLnE3PzSvBKgqKEBkFecW+ycn1eSChZxzshUyCnNz0tXSAYROZVABXn5JakoUiUZiUDSVKlWh5cLxUUWSjoK2J1EvIsMqOwmy0HgplgAK8GDR7kBAAA="",""username"":""os_congth1"",""password"":""Udw7GvAiQRrvI8hzHlgm48r3+3uUQBvu/G4yMjY8RUSsW68EpU/yLiuw23ZgrvwBG307AXdHU/X6zJC4rLNgGL4wkeahAryrjkrcVwbSPiOihX4/g5yscBrH2pl55KYAEppFdP5gxzqvfgAuLouxyWXygSbKMhwiTjmThYWOLgU="",""serviceCode"":""os_congth1"",""orderId"":""20190816160812659""}</data>
            //                    <signature>y09xQN7m3odbdorQJ56XwD28c20Z0KrKyAlMZABi9Jl+/DiSa8QGfPCsv/jTGy7r6IQSOUnRWrv0mgeneQkoN0xar6KqN4eI98wlDN29LbUKNpsIIzkYmgFLE8CjHDyBjAfaooFCHImDbPR6ZBn2SoOGTxmlbfd39t6YvkgMf6Q=</signature>
            //                </par:process>
            //            </soapenv:Body>
            //            </soapenv:Envelope>");

            return soapEnvelopeDocument;
        }

        private string GetSoapDataCheckAccount(DataCheckAccount dataCheckAccount, SoapDataCheckAccount soapDataCheckAccount, string privateKey, string publicKey, out string sign)
        {
            var list = new List<DataCheckAccount>();
            list.Add(dataCheckAccount);

            var dataCheckAccountJson = JsonConvert.SerializeObject(list);
            var dataCheckAccountJsonGzip = StringHelper.GzipBase64(dataCheckAccountJson);

            soapDataCheckAccount.data = dataCheckAccountJsonGzip;
            var soapDataCheckAccountJson = JsonConvert.SerializeObject(soapDataCheckAccount);

            var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, privateKey, publicKey);
            sign = rsa.Sign(soapDataCheckAccountJson);

            return soapDataCheckAccountJson;
        }

        private string GetSoapDataRequestPayment(DataRequestPayment dataRequestPayment, SoapDataRequestPayment soapDataRequestPayment, string privateKey, string publicKey, out string sign)
        {
            var list = new List<DataRequestPayment>();
            list.Add(dataRequestPayment);

            var dataRequestPaymentJson = JsonConvert.SerializeObject(list);
            var dataRequestPaymentJsonGzip = StringHelper.GzipBase64(dataRequestPaymentJson);

            soapDataRequestPayment.data = dataRequestPaymentJsonGzip;
            var soapDataRequestPaymentJson = JsonConvert.SerializeObject(soapDataRequestPayment);

            var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, privateKey, publicKey);
            sign = rsa.Sign(soapDataRequestPaymentJson);

            return soapDataRequestPaymentJson;
        }
    }
}

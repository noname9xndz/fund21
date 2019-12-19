using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using AutoMapper.Configuration;
using smartFunds.Common.Options;
using Microsoft.Extensions.Options;
using Amazon.CloudFront;
using System.Security.Cryptography;

namespace smartFunds.Service.Services
{
    public class Helpers
    {

        /// <summary>
        /// Get signed CloudFront URL
        /// </summary>
        /// <param name="photoPath"></param>
        /// <param name="distributionDomain"></param>
        /// <returns></returns>
        public static string GetCannedSignedURLContactBase(string photoPath, string distributionDomain)
        {
            const string keyPairId = "APKAJYLNBTJQ44EXZHTQ";
            const string rsaKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEogIBAAKCAQEAh/8nUGHs5EK5bDFzDKT/LuS2Jy5px2FCNSb6+SDP8j8tDLSe
DS10J/8W0rgK8YoKgqbSwHc7nqkQwaFK3dBhtRZWOeLvr7uCZEr1D0MFzb00lUqK
f4ph9n/A1kgsdOA/6qVeIeaA5xskQhFSngQCMkeAuBXulF5iOZcYGEsBiH5AGsVr
Bo0AJrRqwAP9Kd/e9Vw6J7FFyjTdm7zEhDlcH1SsiM02Ys82d3XHDx0h4THFbYwR
dWt6DtiKmL82xiDsAoNgYGY0csL9nUtsZSWVI0Wz6tAUCxHhpXgm+381WyeJsv0Q
liKDdqOdPykSth0f3kjQ4E3tyVvKIRmDRvR1fwIDAQABAoIBAD3Ld76zsGOL2htK
YN+m/Xj+XbJyCYiaLe5e9v1KppKJmFvEmmicdcJSy7kV/YsBUYNKcEsaIpIWelGx
i4Y80JjrPGTzNDwitL4DvVSg/LsetQz9IO+RHrBWHnZ/twuodgKV/67hwULB93i5
zFOWuLTF/rwm4xCxzmoiuMUZF+o/4QQKaabwd+89h7+DeFQq91jY4XUgcvHyQPV3
P4WiDjPTbIJG31GrP9Cbw4eBswL9KbvhYHNtbu9zuxhI/pqbap1lIuYObnoimWdE
hdm00FIc8ZbmLKkDRiKdQEcYQr4mpIKN5ODheFhA1l3pqMUoZuEcN4ldyIZtkRLW
0NyZzFkCgYEA0ZX07nr5R4JQ7sI8Us8WLcIJ4M6hhui43iDT59dJ9I3RRtuySzYX
pfc+NsklsPgbtpmVGp8r5uFld1CwlFVzvdOfozDsGbIwNgIWsGVsxeOLyKyhf79z
YOFidvId6e0wSarGmZUz4mhbHFClKDJapFBup24jk2qD6uJdig01A3MCgYEAph02
XA4vYdF/+UmMjmAogkpDozOmTpRGqkOJZ5/YmVpKoWq+hnOXddo4GGqT6b1AEnsv
nziuXZZBGSTuMRIfIsbqRRsKNKzGS30isF/PR/AnPJPH6/Zo28nS73xSRxWu8k7W
5pCAgzXnVr3Ob0ZGX09GuTzAatKa6KeoUMdsesUCgYBp3aVrERL53AmlkNeHvLMn
SexTcgyFaOh4y3w+j7D7pucfM7pL43bujoUOh2xSiAD3q3x1hhFW/mOScV/Ajal5
KXxpojygfjF8FnH/iDv5eYcSRqENNlfBiBeEnagekYDitTC8Q3GB29Sp6NKEC2td
tIMwb6HoxNT9wHofUayFoQKBgFimWF9SVqkOZAyG2tMUsCmwNl5/bu6apQeymT7L
CI32qMNyMxGP2LHVboBhSGTCUdJLGYQQfMWHLWCc290mPaWSoG2W26B24DBjLMMR
ro5GtLnYaCYeT6GUGNUj9Mjo4n9/4aIUVfEwMDCThPrPdzjgFu8+Y7XehTtKVkId
21+9AoGABSlZsV1+5n4TKbi4LGAwnKiOE7vEyQz7jFJEzNH8eqK3GKSzJIP3zFEC
MwBDKvyz+WsuhjccRKncswJBFZY3HleBBmQ43zPkL6kOCJLzBdFslk/0bVq0m+dJ
jyweJ5gTWfNgFuk4vrrioGUW10P/yuNU6C2bPQKDJtc7tfkuD7o=
-----END RSA PRIVATE KEY-----";

            var rsaKeyText = new StringReader(rsaKey);
            try
            {
                if (string.IsNullOrWhiteSpace(photoPath))
                    return "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";

                if (photoPath.IndexOf("/") > -1)
                    photoPath = HttpUtility.UrlEncode(photoPath);

                TimeSpan expiry = TimeSpan.FromHours(6);

                var signedUrl = AmazonCloudFrontUrlSigner.GetCannedSignedURL(
                    string.Format("https://{0}/{1}", distributionDomain, photoPath),
                    rsaKeyText,
                    keyPairId,
                    DateTime.Now.AddSeconds((int)expiry.TotalSeconds));

                return signedUrl;
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        public static string CreateCheckSum(string accessCode, string secretKey, params string[] listparam)
        {
            var checkSumStr = accessCode + String.Join("", listparam);

            //HMAC-SHA1
            Byte[] secretBytes = UTF8Encoding.UTF8.GetBytes(secretKey);
            HMACSHA1 hmac = new HMACSHA1(secretBytes);

            Byte[] dataBytes = UTF8Encoding.UTF8.GetBytes(checkSumStr);
            Byte[] calcHash = hmac.ComputeHash(dataBytes);
            string checkSumHash = Convert.ToBase64String(calcHash).Replace(" ", "+").Replace("=", "%3D").Replace("+", "%2B");

            return checkSumHash;
        }

        public static string CreateCheckSumSHA256(string accessCode, params string[] listparam)
        {
            var checkSumStr = accessCode + String.Join("", listparam);

            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(checkSumStr));

            string checkSum = BitConverter.ToString(hashedDataBytes).Replace("-", "").ToLower();

            return checkSum;
        }
    }
}

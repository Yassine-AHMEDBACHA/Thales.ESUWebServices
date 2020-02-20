using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ESU.ActivationWS.Core
{
    public class ActivationHelper : IActivationHelper
    {
        // Key for HMAC/SHA256 signature.
        private static readonly byte[] MacKey = new byte[64] {
            254,  49, 152, 117, 251,  72, 132, 134,
            156, 243, 241, 206, 153, 168, 144, 100,
            171,  87,  31, 202,  71,   4,  80,  88,
            48,   36, 226,  20,  98, 135, 121, 160,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0,
            0,     0,   0,   0,   0,   0,   0,   0
        };

        private const string Action = "http://www.microsoft.com/BatchActivationService/BatchActivate";
        private static readonly Uri Uri = new Uri("https://activation.sls.microsoft.com/BatchActivation/BatchActivation.asmx");
        private readonly ILogger<ActivationHelper> logger;

        public ActivationHelper(ILogger<ActivationHelper> logger)
        {
            this.logger = logger;
        }

        public string RequestConfirmationKey(string installationId, string extendedProductId)
        {
            var soapRequest = CreateSoapRequest(installationId, extendedProductId);
            var webRequest = CreateWebRequest(soapRequest);
            var soapResponse = GetResponse(webRequest);
            return ParseSoapResponse(soapResponse);
        }

        private static XmlDocument GetResponse(HttpWebRequest webRequest)
        {
            var soapResponse = new XmlDocument();
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var asyncResult = webRequest.BeginGetResponse(null, null);
                asyncResult.AsyncWaitHandle.WaitOne();

                // Read data from the response stream.
                using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
                using (StreamReader streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResponse.LoadXml(streamReader.ReadToEnd());
                    stopwatch.Stop();
                }

            }
            catch (Exception ex)
            {
                throw new WebRequestException("Exception calling 'CallWebservice': " + ex.Message, ex);
            }

            return soapResponse;
        }

        private static XmlDocument CreateSoapRequest(string installationId, string extendedProductId)
        {
            // Create an activation request string.
            string activationRequest =
                "<ActivationRequest xmlns =\"http://www.microsoft.com/DRM/SL/BatchActivationRequest/1.0\">" +
                    "<VersionNumber>2.0</VersionNumber>" +
                    "<RequestType>1</RequestType>" +
                    "<Requests>" +
                        "<Request>" +
                            "<PID>" + extendedProductId + "</PID>" +
                            "<IID>" + installationId + "</IID>" +
                        "</Request>" +
                    "</Requests>" +
                "</ActivationRequest>";

            // Get the unicode byte array of activationRequest and convert it to Base64.
            byte[] bytes = Encoding.Unicode.GetBytes(activationRequest);
            string requestXml = Convert.ToBase64String(bytes);

            var soapRequest = new XmlDocument();

            using (var hMACSHA = new HMACSHA256(MacKey))
            {
                // Convert the HMAC hashed data to Base64.
                string digest = Convert.ToBase64String(hMACSHA.ComputeHash(bytes));

                soapRequest.LoadXml(
                    "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" +
                        "<soap:Body>" +
                            "<BatchActivate xmlns=\"http://www.microsoft.com/BatchActivationService\">" +
                                "<request>" +
                                    "<Digest>" + digest + "</Digest>" +
                                    "<RequestXml>" + requestXml + "</RequestXml>" +
                                "</request>" +
                            "</BatchActivate>" +
                        "</soap:Body>" +
                    "</soap:Envelope>"
                );
            }

            return soapRequest;
        }

        private HttpWebRequest CreateWebRequest(XmlDocument soapRequest)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(Uri);
            webRequest.Accept = "text/xml";
            webRequest.ContentType = "text/xml; charset=\"utf-8\"";
            webRequest.Headers.Add("SOAPAction", Action);
            webRequest.Method = "POST";

            try
            {
                // Insert SOAP envelope
                using (Stream stream = webRequest.GetRequestStream())
                {
                    soapRequest.Save(stream);
                }

            }
            catch (Exception ex)
            {
                throw new WebRequestException("Exception calling 'CreateWebRequest': " + ex.Message);
            }

            return webRequest;
        }

        private string ParseSoapResponse(XmlDocument soapResponse)
        {
            var xmlNsManager = new XmlNamespaceManager(soapResponse.NameTable);
            xmlNsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            xmlNsManager.AddNamespace("msbas", "http://www.microsoft.com/BatchActivationService");
            xmlNsManager.AddNamespace("msbar", "http://www.microsoft.com/DRM/SL/BatchActivationResponse/1.0");

            var responseXmlString = soapResponse.SelectSingleNode("/soap:Envelope/soap:Body/msbas:BatchActivateResponse/msbas:BatchActivateResult/msbas:ResponseXml", xmlNsManager).InnerText;

            XmlDocument responseXml = new XmlDocument();
            responseXml.LoadXml(responseXmlString);

            if (responseXml.SelectSingleNode("//msbar:CID", xmlNsManager) != null)
            {
                string confirmationId = responseXml.SelectSingleNode("//msbar:CID", xmlNsManager).InnerText;
                return confirmationId;

            }
            else if (responseXml.SelectSingleNode("//msbar:ErrorCode", xmlNsManager) != null)
            {
                string errorCode = responseXml.SelectSingleNode("//msbar:ErrorCode", xmlNsManager).InnerText;
                throw new MsException("The Confirmation ID could not be retrieved (" + errorCode + ")");

            }

            throw new ParsingException("The SOAP response could not be parsed.");
        }
    }
}

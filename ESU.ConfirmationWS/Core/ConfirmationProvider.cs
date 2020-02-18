using ESU.ConfirmationWS.Core.Models;
using ESU.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ESU.ConfirmationWS.Core
{
    public class ConfirmationProvider : IConfirmationProvider
    {
        private const string Resource = "msactivations";
        private readonly ILogger<ConfirmationProvider> logger;
        private readonly IConfiguration configuration;
        private readonly RestClient restClient;
        private readonly string url;

        public ConfirmationProvider(IConfiguration configuration, ILogger<ConfirmationProvider> logger)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.url = this.configuration.GetValue<string>("Url");
            this.restClient = new RestClient(url);
        }

        public Confirmation GetConfirmation(string installationId, string extendedProductId)
        {
            var confirmation = new Confirmation
            {
                RequestDate = DateTime.Now
            };

            var isSucces = this.TryCallWebService(installationId, extendedProductId, out var confirmationKey);
            confirmation.ResponseDate = DateTime.Now;
            if(isSucces) 
            {
                confirmation.Status = Status.Success;
                confirmation.Content = confirmationKey;
            }
            else
            {
                confirmation.Status = Status.Failed;
                confirmation.Content = Status.Failed.ToString();
            }

            return confirmation;
        }

        private bool TryCallWebService(string installationId, string extendedProductId, out string confirmationKey)
        {
            confirmationKey = string.Empty;
            try
            {
                var webRequest = WebRequest.CreateHttp(this.url);

                var enveloppeSOAP = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                   "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                       "<soap:Body>" +
                           "<AcquireConfirmationId xmlns=\"http://tempuri.org/\">" +
                               "<installationId>{0}</installationId>" +
                               "<extendedProductId>{1}</extendedProductId>" +
                           "</AcquireConfirmationId>" +
                       "</soap:Body>" +
                   "</soap:Envelope>";

                webRequest.Headers.Add("SOAPAction", "http://tempuri.org/AcquireConfirmationId");
                webRequest.ContentType = "text/xml;charset=\"utf - 8\"";
                webRequest.Accept = "text/xml";
                webRequest.Method = "POST";


                enveloppeSOAP = string.Format(enveloppeSOAP, installationId, extendedProductId);

                byte[] byteArray = Encoding.UTF8.GetBytes(enveloppeSOAP);
                webRequest.ContentLength = byteArray.Length;

                var requestStream = webRequest.GetRequestStream();
                requestStream.Write(byteArray, 0, byteArray.Length);

                requestStream.Close();

                var response = webRequest.GetResponse();

                Stream readerData = response.GetResponseStream();
                StreamReader readerResponse = new StreamReader(readerData);

                string responseFromServer = readerResponse.ReadToEnd();

                //Console.WriteLine(responseFromServer);

                readerResponse.Close();
                requestStream.Close();
                response.Close();


                var doc = new XmlDocument();
                doc.LoadXml(responseFromServer);

                string resp = JsonConvert.SerializeXmlNode(doc);

                var respActi = JsonConvert.DeserializeObject<ResponseActivation>(resp);

                confirmationKey = respActi.envelope.body.AcquireConfirmationIdResponse.AcquireConfirmationIdResult;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, string.Empty);
                return false;
            }

            return true;
        }
    }
}

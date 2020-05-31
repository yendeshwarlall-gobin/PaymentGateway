using System;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace Payment.Gateway.Services.AcquiringBankServices
{
    public class AcquiringBankService:IAcquiringBankService
    {
        private string _acquiringBankUrl;
        public AcquiringBankService()
        {
            _acquiringBankUrl = ConfigurationManager.AppSettings["AcquiringBankUrl"];
        }
        public AcquiringBankResponse ProcessPayment(AcquiringBankRequest acquiringBankRequest)
        {

            var result = new AcquiringBankResponse {PaymentStatus = "Failed"};
            var request = CreateRequest(acquiringBankRequest);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {


                    var responseString = GetResponseAsString(reader.BaseStream);

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        result.PaymentStatus = responseString;
                    }
                    else
                    {
                        result = JsonConvert.DeserializeObject<AcquiringBankResponse>(responseString);

                    }
                }
            }
            catch (WebException e)
            {
                result.PaymentStatus = e.Message;
            }

            return result;
        }

        private string GetResponseAsString(Stream responseStream)
        {
            var streamReader = new StreamReader(responseStream,true);
            return streamReader.ReadToEnd();
        }

        private HttpWebRequest CreateRequest(AcquiringBankRequest postData)
        {
            var request = (HttpWebRequest) WebRequest.Create(_acquiringBankUrl);
            var json = JsonConvert.SerializeObject(postData);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = json.Length;
            using (var writter = new StreamWriter(request.GetRequestStream()))
            {
                writter.Write(json);
            }

            return request;
        }
    }
}

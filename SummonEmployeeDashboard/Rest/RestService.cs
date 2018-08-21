using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Rest
{
    class RestService : IRestService
    {
        public IRestClient Client { get; set; }

        public T RestCall<T>(RestRequest request) where T : new()
        {
            var response = Client.Execute<T>(request);
            if (Successful(response.StatusCode))
            {
                return response.Data;
            } else if (response.ErrorException != null)
            {
                throw response.ErrorException;
            } else if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new Exception(response.ErrorMessage);
            }
            else
            {
                throw new Exception(response.StatusDescription);
            }
        }

        private bool Successful(System.Net.HttpStatusCode code)
        {
            return code == System.Net.HttpStatusCode.OK || code == System.Net.HttpStatusCode.Created ||
                code == System.Net.HttpStatusCode.Accepted || code == System.Net.HttpStatusCode.NonAuthoritativeInformation ||
                code == System.Net.HttpStatusCode.NoContent || code == System.Net.HttpStatusCode.ResetContent ||
                code == System.Net.HttpStatusCode.PartialContent
                ;
        }

        public string RestCall(RestRequest request)
        {
            var response = Client.Execute(request);
            if (Successful(response.StatusCode))
            {
                return response.Content;
            }
            else
            {
                throw new Exception(response.StatusDescription);
            }
        }
    }
}

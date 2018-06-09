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

        public async Task<T> RestCall<T>(RestRequest request)
        {
            var response = await Client.ExecuteTaskAsync<T>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw new Exception(response.StatusDescription);
            }
        }

        public async Task<string> RestCall(RestRequest request)
        {
            var response = await Client.ExecuteTaskAsync(request);
            if (response.IsSuccessful)
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

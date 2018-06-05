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
            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                return response.Data;
            }
            else
            {
                throw response.ErrorException;
            }
        }
    }
}

using RestSharp;
using SummonEmployeeDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Rest
{
    class SummonRequestService : RestService
    {
        public SummonRequestService()
        {
        }

        public async Task<Person> GetPerson(int personId, string accessToken)
        {
            var request = new RestRequest("people/{id}");
            request.AddUrlSegment("id", personId);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<Person>(request);
        }

        public async Task<SummonRequest> Accept(int requestId, string accessToken)
        {
            var request = new RestRequest("summonrequests/{id}/accept")
            {
                Method = Method.PUT
            };
            request.AddUrlSegment("id", requestId);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<SummonRequest>(request);
        }

        public async Task<SummonRequest> Reject(int requestId, string accessToken)
        {
            var request = new RestRequest("summonrequests/{id}/reject")
            {
                Method = Method.PUT
            };
            request.AddUrlSegment("id", requestId);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<SummonRequest>(request);
        }

        public async Task<SummonRequest> Cancel(int requestId, string accessToken)
        {
            var request = new RestRequest("summonrequests/{id}/cancel")
            {
                Method = Method.PUT
            };
            request.AddUrlSegment("id", requestId);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<SummonRequest>(request);
        }

        public async Task<SummonRequest> AddSummonRequest(AddSummonRequest add, string accessToken)
        {
            var request = new RestRequest("summonrequests")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(add);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<SummonRequest>(request);
        }

        public async Task<List<SummonRequest>> ListIncomingRequests(int targetId, string accessToken)
        {
            var request = new RestRequest("people/{id}/incomingRequests");
            request.AddUrlSegment("id", targetId);
            request.AddQueryParameter("filter[include]", "caller");
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<SummonRequest>>(request);
        }

        public async Task<List<SummonRequest>> ListOutgoingRequests(int callerId, string accessToken)
        {
            var request = new RestRequest("people/{id}/outgoingRequests");
            request.AddUrlSegment("id", callerId);
            request.AddQueryParameter("filter[include]", "target");
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<SummonRequest>>(request);
        }
    }
}

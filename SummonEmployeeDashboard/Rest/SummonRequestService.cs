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

        public Person GetPerson(int personId, string accessToken)
        {
            var request = new RestRequest("people/{id}");
            request.AddUrlSegment("id", personId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<Person>(request);
        }

        public SummonRequest Accept(int requestId, string accessToken)
        {
            var request = new RestRequest("summonrequests/{id}/accept")
            {
                Method = Method.PUT
            };
            request.AddUrlSegment("id", requestId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<SummonRequest>(request);
        }

        public SummonRequest Reject(int requestId, string accessToken)
        {
            var request = new RestRequest("summonrequests/{id}/reject")
            {
                Method = Method.PUT
            };
            request.AddUrlSegment("id", requestId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<SummonRequest>(request);
        }

        public SummonRequest Cancel(int requestId, string accessToken)
        {
            var request = new RestRequest("summonrequests/{id}/cancel")
            {
                Method = Method.PUT
            };
            request.AddUrlSegment("id", requestId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<SummonRequest>(request);
        }

        public SummonRequest AddSummonRequest(AddSummonRequest add, string accessToken)
        {
            var request = new RestRequest("summonrequests")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(add);
            request.AddHeader("Authorization", accessToken);
            return RestCall<SummonRequest>(request);
        }

        public List<SummonRequest> ListIncomingRequests(int targetId, string accessToken)
        {
            var request = new RestRequest("people/{id}/incomingRequests");
            request.AddUrlSegment("id", targetId.ToString());
            request.AddQueryParameter("filter[include]", "caller");
            request.AddQueryParameter("filter[order]", "requested DESC");
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<SummonRequest>>(request);
        }

        public List<SummonRequest> ListOutgoingRequests(int callerId, string accessToken)
        {
            var request = new RestRequest("people/{id}/outgoingRequests");
            request.AddUrlSegment("id", callerId.ToString());
            request.AddQueryParameter("filter[include]", "target");
            request.AddQueryParameter("filter[order]", "requested DESC");
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<SummonRequest>>(request);
        }
    }
}

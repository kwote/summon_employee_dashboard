using RestSharp;
using SummonEmployeeDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Rest
{
    class PeopleService : RestService
    {
        public PeopleService()
        {
        }

        public async Task<List<Person>> ListPeople(int? departmentId, string accessToken)
        {
            var request = new RestRequest("people");
            if (departmentId != null)
            {
                request.AddQueryParameter("departmentId", departmentId.ToString());
            }
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<Person>>(request);
        }

        public async Task<AccessToken> Login(LoginCredentials credentials)
        {
            var request = new RestRequest("people/login")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(credentials);
            request.AddQueryParameter("include", "user");
            return await RestCall<AccessToken>(request);
        }

        public async Task<Boolean> Ping(string accessToken)
        {
            var request = new RestRequest("people/ping")
            {
                Method = Method.PUT
            };
            request.AddHeader("Authorization", accessToken);
            return await RestCall<Boolean>(request);
        }

        public async Task<Person> Register(RegisterPerson registerPerson)
        {
            var request = new RestRequest("people")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(registerPerson);
            return await RestCall<Person>(request);
        }
    }
}

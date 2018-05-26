using RestSharp;
using SummonEmployeeDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Rest
{
    class PeopleService : IRestService
    {
        public PeopleService()
        {
        }

        public IRestClient Client { get; set; }

        public async Task<List<Person>> ListPeople(int? departmentId)
        {
            var request = new RestRequest("people");
            if (departmentId != null)
            {
                request.AddQueryParameter("departmentId", departmentId.ToString());
            }
            var response = await Client.ExecuteTaskAsync<List<Person>>(request);
            return response.Data;
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
            var response = await Client.ExecuteTaskAsync<AccessToken>(request);
            return response.Data;
        }

        public async Task<Person> Register(RegisterPerson registerPerson)
        {
            var request = new RestRequest("people")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(registerPerson);
            var response = await Client.ExecuteTaskAsync<Person>(request);
            return response.Data;
        }
    }
}

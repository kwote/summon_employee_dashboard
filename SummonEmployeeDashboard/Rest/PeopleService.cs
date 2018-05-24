using RestSharp;
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

        public async Task<List<Person>> ListPeople(int departmentId)
        {
            var request = new RestRequest("people");
            request.AddQueryParameter("departmentId", departmentId.ToString());
            var response = await Client.ExecuteTaskAsync<List<Person>>(request);
            return response.Data;
        }
    }
}

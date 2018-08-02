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

        public async Task<List<Person>> ListPeople(string accessToken)
        {
            var request = new RestRequest("people");
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<Person>>(request);
        }

        public async Task<List<Person>> ListSummonPeople(string accessToken)
        {
            var request = new RestRequest("people/summon");
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<Person>>(request);
        }

        public async Task<List<Role>> ListRoles()
        {
            var request = new RestRequest("Roles");
            request.AddQueryParameter("filter[fields]", "id");
            request.AddQueryParameter("filter[fields]", "name");
            return await RestCall<List<Role>>(request);
        }

        public async Task<Role> GetRole(int personId, string accessToken)
        {
            var request = new RestRequest("people/role");
            request.AddQueryParameter("userId", personId.ToString());
            request.AddHeader("Authorization", accessToken);
            return await RestCall<Role>(request);
        }

        public async Task<Person> GetPerson(int personId, string accessToken)
        {
            var request = new RestRequest("people/{id}");
            request.AddUrlSegment("id", personId);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<Person>(request);
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

        public async Task<List<Stat>> GetStatistics(int userId, string accessToken)
        {
            List<Stat> stats = new List<Stat>();
            Task[] tasks = new Task[7];
            for (int i = 0; i < 7; ++i)
            {
                var date = DateTime.Now.AddDays(-7);
                tasks[i] = Task.Run(async () => {
                    var request = new RestRequest("people/statistics");
                    request.AddQueryParameter("personId", userId.ToString());
                    request.AddQueryParameter("date", Utils.GetStringTime(date));
                    request.AddHeader("Authorization", accessToken);
                    var stat = await RestCall<Stat>(request);
                    stats.Add(stat);
                });
            }
            await Task.WhenAll(tasks);
            stats.Sort((s1, s2) => { return s1.Date.Value.CompareTo(s2.Date.Value); });
            return stats;
        }

        public async Task<string> Logout(string accessToken)
        {
            var request = new RestRequest("people/logout")
            {
                Method = Method.POST
            };
            request.AddHeader("Authorization", accessToken);
            request.JsonSerializer = new CustomJsonSerializer();
            return await RestCall(request);
        }

        public async Task<Boolean> Ping(string accessToken)
        {
            var request = new RestRequest("people/ping")
            {
                Method = Method.PUT
            };
            request.AddQueryParameter("token", accessToken);
            return await RestCall<Boolean>(request);
        }

        public async Task<Boolean> ChooseRole(int personId, string role, string accessToken)
        {
            var request = new RestRequest("people/chooseRole")
            {
                Method = Method.POST
            };
            request.AddQueryParameter("userId", personId.ToString());
            request.AddQueryParameter("role", role);
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

        public async Task<List<SummonRequest>> ListIncomingRequests(int targetId, string accessToken)
        {
            var request = new RestRequest("people/{id}/incomingRequests");
            request.AddUrlSegment("id", targetId);
            var date = DateTime.Now.AddDays(-1);
            var filter = new Dictionary<string, object>()
            {
                ["where"] = new Dictionary<string, object>()
                {
                    ["requested"] = new Dictionary<string, object>()
                    {
                        ["gt"] = Utils.GetStringTime(date)
                    }
                },
                ["include"] = "caller",
                ["order"] = "requested DESC"
            };
            var filterStr = SimpleJson.SimpleJson.SerializeObject(filter);
            request.AddQueryParameter("filter", filterStr);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<SummonRequest>>(request);
        }

        public async Task<List<SummonRequest>> ListOutgoingRequests(int callerId, string accessToken)
        {
            var request = new RestRequest("people/{id}/outgoingRequests");
            request.AddUrlSegment("id", callerId);
            var date = DateTime.Now.AddDays(-1);
            var filter = new Dictionary<string, object>()
            {
                ["where"] = new Dictionary<string, object>()
                {
                    ["requested"] = new Dictionary<string, object>()
                    {
                        ["gt"] = Utils.GetStringTime(date)
                    }
                },
                ["include"] = "target",
                ["order"] = "requested DESC"
            };
            var filterStr = SimpleJson.SimpleJson.SerializeObject(filter);
            request.AddQueryParameter("filter", filterStr);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<SummonRequest>>(request);
        }
    }
}

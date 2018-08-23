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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PeopleService));
        public PeopleService()
        {
        }

        public List<Person> ListPeople(string accessToken)
        {
            var request = new RestRequest("people");
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<Person>>(request);
        }

        public List<Person> ListSummonPeople(string accessToken)
        {
            var request = new RestRequest("people/summon");
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<Person>>(request);
        }

        public List<Role> ListRoles()
        {
            var request = new RestRequest("Roles");
            request.AddQueryParameter("filter[fields]", "id");
            request.AddQueryParameter("filter[fields]", "name");
            return RestCall<List<Role>>(request);
        }

        public Role GetRole(int personId, string accessToken)
        {
            var request = new RestRequest("people/role");
            request.AddQueryParameter("userId", personId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<Role>(request);
        }

        public Person GetPerson(int personId, string accessToken)
        {
            var request = new RestRequest("people/{id}");
            request.AddUrlSegment("id", personId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<Person>(request);
        }

        public AccessToken Login(LoginCredentials credentials)
        {
            var request = new RestRequest("people/login")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(credentials);
            request.AddQueryParameter("include", "user");
            return RestCall<AccessToken>(request);
        }

        public List<Stat> GetStatistics(int userId, string accessToken)
        {
            List<Stat> stats = new List<Stat>();
            Task[] tasks = new Task[7];
            for (int i = 0; i < 7; ++i)
            {
                var date = DateTime.Now.AddDays(-i);
                tasks[i] = Task.Factory.StartNew(() => {
                    try
                    {
                        var request = new RestRequest("people/statistics");
                        request.AddQueryParameter("personId", userId.ToString());
                        request.AddQueryParameter("date", Utils.GetStringTime(date));
                        request.AddHeader("Authorization", accessToken);
                        var stat = RestCall<Stat>(request);
                        stats.Add(stat);
                    } catch (Exception e)
                    {
                        log.Error("Failed to load stat", e);
                    }
                });
            }
            Task.WaitAll(tasks);
            stats.Sort((s1, s2) => {
                if (s1.Date.HasValue) {
                    return s2.Date.HasValue ? s1.Date.Value.CompareTo(s2.Date.Value) : 1;
                }
                return -1;
            });
            return stats;
        }

        public void Logout(string accessToken)
        {
            var request = new RestRequest("people/logout")
            {
                Method = Method.POST
            };
            request.AddHeader("Authorization", accessToken);
            request.JsonSerializer = new CustomJsonSerializer();
            RestCall(request);
        }

        public bool Ping(string accessToken)
        {
            var request = new RestRequest("people/ping")
            {
                Method = Method.PUT
            };
            request.AddQueryParameter("token", accessToken);
            return RestCall<Boolean>(request);
        }

        public bool ChooseRole(int personId, string role, string accessToken)
        {
            var request = new RestRequest("people/chooseRole")
            {
                Method = Method.POST
            };
            request.AddQueryParameter("userId", personId.ToString());
            request.AddQueryParameter("role", role);
            request.AddHeader("Authorization", accessToken);
            return RestCall<Boolean>(request);
        }

        public Person Register(RegisterPerson registerPerson)
        {
            var request = new RestRequest("people")
            {
                Method = Method.POST
            };
            request.JsonSerializer = new CustomJsonSerializer();
            request.AddJsonBody(registerPerson);
            return RestCall<Person>(request);
        }

        public List<SummonRequest> ListIncomingRequests(int targetId, string accessToken)
        {
            var request = new RestRequest("people/{id}/incomingRequests");
            request.AddUrlSegment("id", targetId.ToString());
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
            var filterStr = SimpleJson.SerializeObject(filter);
            request.AddQueryParameter("filter", filterStr);
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<SummonRequest>>(request);
        }

        public List<SummonRequest> ListOutgoingRequests(int callerId, string accessToken)
        {
            var request = new RestRequest("people/{id}/outgoingRequests");
            request.AddUrlSegment("id", callerId.ToString());
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
            var filterStr = SimpleJson.SerializeObject(filter);
            request.AddQueryParameter("filter", filterStr);
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<SummonRequest>>(request);
        }
    }
}

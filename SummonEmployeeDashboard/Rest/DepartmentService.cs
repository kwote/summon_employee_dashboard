using RestSharp;
using SummonEmployeeDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonEmployeeDashboard.Rest
{
    class DepartmentService : RestService
    {
        public DepartmentService()
        {
        }

        public async Task<List<Department>> ListDepartments(string accessToken)
        {
            var request = new RestRequest("departments");
            request.AddHeader("Authorization", accessToken);
            return await RestCall<List<Department>>(request);
        }

        public async Task<Department> GetDepartment(int departmentId, string accessToken)
        {
            var request = new RestRequest("departments/{id}");
            request.AddUrlSegment("id", departmentId);
            request.AddHeader("Authorization", accessToken);
            return await RestCall<Department>(request);
        }
    }
}

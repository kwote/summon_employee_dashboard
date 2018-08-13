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

        public List<Department> ListDepartments(string accessToken)
        {
            var request = new RestRequest("departments");
            request.AddHeader("Authorization", accessToken);
            return RestCall<List<Department>>(request);
        }

        public Department GetDepartment(int departmentId, string accessToken)
        {
            var request = new RestRequest("departments/{id}");
            request.AddUrlSegment("id", departmentId.ToString());
            request.AddHeader("Authorization", accessToken);
            return RestCall<Department>(request);
        }
    }
}

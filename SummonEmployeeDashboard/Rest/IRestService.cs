using RestSharp;

namespace SummonEmployeeDashboard.Rest
{
    public interface IRestService
    {
        IRestClient Client { get; set; }
    }
}
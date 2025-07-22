using System.Threading.Tasks;
using XRoadTestApp.Models;

namespace XRoadTestApp.Services
{
    public interface IXRoadService
    {
        Task<string> SendRequestAsync(XRoadServiceInfo service, string soapBody, string userId = null);
        Task<string> TestConnectionAsync();
    }
}
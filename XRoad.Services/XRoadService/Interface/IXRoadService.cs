using System.Threading.Tasks;
using XRoadTestApp.Models;

namespace XRoadTestApp.XRoad.Services.XRoadService.Interface
{
    public interface IXRoadService
    {
        Task<string> SendRequestAsync(XRoadServiceInfo service, string soapBody, string userId = null); 
    }
}
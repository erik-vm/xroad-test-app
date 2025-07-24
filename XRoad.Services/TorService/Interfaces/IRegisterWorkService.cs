using System.Collections.Generic;
using System.Threading.Tasks;
using XRoadTestApp.XRoad.DTO.EMTA.TOR;

namespace XRoadTestApp.XRoad.Services.TorService.Interfaces
{
    public interface IRegisterWorkService
    {
        public Task<string> RegisterWorkAsync(List<RegisterWorkRequest> requests, string userId);
    }
}
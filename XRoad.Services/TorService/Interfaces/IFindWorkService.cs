using System;
using System.Threading.Tasks;
using XRoadTestApp.XRoad.DTO.EMTA.TOR;

namespace XRoadTestApp.XRoad.Services.TorService.Interfaces
{
    public interface IFindWorkService
    {
        public Task<string> FindActiveWorkAsync(string employerCode, int fromRecord, string userId);
        public Task<string> FindAllWorkAsync(string employerCode, int fromRecord, string userId);

        public Task<string> FindWorkByPeriodAsync(string employerCode, DateTime periodStart, DateTime periodEnd,
            int fromRecord, string userId);

        public Task<string> SearchWorkAsync(WorkSearchRequest request, string userId = null);

        public Task<string> GetNextPageAsync(string employerCode, string queryType, int fromRecord,
            DateTime? periodStart, DateTime? periodEnd, string userId);
        
    }
}
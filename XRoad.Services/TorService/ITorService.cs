using System;
using System.Threading.Tasks;

namespace XRoadTestApp.XRoad.Services.TorService
{
    public interface ITorService
    {
        Task<string> RegisterWorkAsync(string personalCode, DateTime startDate, double workTimeRatio,
            string userId = null);

        Task<string> FindWorkAsync(string employerCode, string queryType, int fromRecord, string userId);
    }
}
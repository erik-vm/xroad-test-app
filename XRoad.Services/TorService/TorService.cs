using System;
using System.Threading.Tasks;
using XRoadTestApp.Models;
using XRoadTestApp.Services;
using XRoadTestApp.XRoad.Data.EMTA;

namespace XRoadTestApp.XRoad.Services.TorService
{
    public class TorService:ITorService
    
    
    {
    
        private readonly IXRoadService _xroadService;

        public TorService(IXRoadService xroadService)
        {
            _xroadService = xroadService;
        }

        public async Task<string> RegisterWorkAsync(string personalCode, DateTime startDate, double workTimeRatio = 1.0,
            string userId = null)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var startDateStr = startDate.ToString("yyyy-MM-dd");

            var soapBody = $@"<tns:TOOTREG>
        <request>
            <kanne_jada>
                <item>
                    <kande_tyyp>R</kande_tyyp>
                    <kande_aeg>{timestamp}</kande_aeg>
                    <isikukood>{personalCode}</isikukood>
                    <tootamise_algus>{startDateStr}</tootamise_algus>
                    <tooaja_maar>{workTimeRatio:F1}</tooaja_maar>
                </item>
            </kanne_jada>
        </request>
    </tns:TOOTREG>";

            return await _xroadService.SendRequestAsync(Tor.TOOTREG, soapBody, userId ?? "48906090292");
        }

        public async Task<string> FindWorkAsync(string employerCode, string queryType = "A", int fromRecord = 1,
            string userId = null)
        {
            var soapBody = $@"<tns:TORRGNO>
        <request>
            <paringu_liik>{queryType}</paringu_liik>
            <tvoim_id>{employerCode}</tvoim_id>
            <alates_kandest>{fromRecord}</alates_kandest>
        </request>
    </tns:TORRGNO>";

            return await _xroadService.SendRequestAsync(Tor.TORRGNO, soapBody, userId ?? "48906090292");
        }
    }
}
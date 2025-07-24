using System;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using XRoadTestApp.XRoad.Core.Constants.ServicesInfo.EMTA;
using XRoadTestApp.XRoad.DTO.EMTA.TOR;
using XRoadTestApp.XRoad.Services.TorService.Interfaces;
using XRoadTestApp.XRoad.Services.XRoadService.Interface;

namespace XRoadTestApp.XRoad.Services.TorService.Services
{
    public class FindWorkService : IFindWorkService
    {
        private readonly IXRoadService _xRoadService;


        public FindWorkService(IXRoadService xRoadService)
        {
            _xRoadService = xRoadService ?? throw new ArgumentNullException(nameof(xRoadService));
        }
        
          public async Task<string> FindActiveWorkAsync(string employerCode, int fromRecord = 1, string userId = null)
        {
            var request = new WorkSearchRequest
            {
                ParinguLiik = "A",
                TvoimId = employerCode,
                AlatesKandest = fromRecord
            };
            
            return await SearchWorkAsync(request, userId);
        }
        
        /// <summary>
        /// Find all work records as of today
        /// </summary>
        public async Task<string> FindAllWorkAsync(string employerCode, int fromRecord = 1, string userId = null)
        {
            var request = new WorkSearchRequest
            {
                ParinguLiik = "T",
                TvoimId = employerCode,
                AlatesKandest = fromRecord
            };
            
            return await SearchWorkAsync(request, userId);
        }
        
        /// <summary>
        /// Find work records active during a specific period
        /// </summary>
        public async Task<string> FindWorkByPeriodAsync(string employerCode, DateTime periodStart, DateTime periodEnd, 
            int fromRecord = 1, string userId = null)
        {
            var request = new WorkSearchRequest
            {
                ParinguLiik = "P",
                TvoimId = employerCode,
                TootAlgus = periodStart,
                TootLopp = periodEnd,
                AlatesKandest = fromRecord
            };
            
            return await SearchWorkAsync(request, userId);
        }
        
        /// <summary>
        /// Generic work search method
        /// </summary>
        public async Task<string> SearchWorkAsync(WorkSearchRequest request, string userId = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
                
            if (string.IsNullOrWhiteSpace(request.TvoimId))
                throw new ArgumentException("Employer code is required", nameof(request));
                
            var soapBody = BuildSoapBody(request);
            return await _xRoadService.SendRequestAsync(Tor.TORRGNO, soapBody, userId);
        }
        
        /// <summary>
        /// Get next page of results
        /// </summary>
        public async Task<string> GetNextPageAsync(string employerCode, string queryType, int fromRecord, 
            DateTime? periodStart = null, DateTime? periodEnd = null, string userId = null)
        {
            var request = new WorkSearchRequest
            {
                ParinguLiik = queryType,
                TvoimId = employerCode,
                TootAlgus = periodStart,
                TootLopp = periodEnd,
                AlatesKandest = fromRecord
            };
            
            return await SearchWorkAsync(request, userId);
        }
        
        private string BuildSoapBody(WorkSearchRequest request)
        {
            var body = new StringBuilder();
            body.Append("<tns:TORRGNO>");
            body.Append("<request>");
            
            // Query type is mandatory
            AppendField(body, "paringu_liik", request.ParinguLiik);
            
            // Work period dates (required for P type)
            AppendFieldIfNotNull(body, "toot_algus", request.TootAlgus?.ToString("yyyy-MM-dd"));
            AppendFieldIfNotNull(body, "toot_lopp", request.TootLopp?.ToString("yyyy-MM-dd"));
            
            // Employer code (mandatory)
            AppendField(body, "tvoim_id", request.TvoimId);
            
            // From record number
            AppendField(body, "alates_kandest", request.AlatesKandest);
            
            body.Append("</request>");
            body.Append("</tns:TORRGNO>");
            
            return body.ToString();
        }
        
        private void AppendField(StringBuilder sb, string fieldName, object value)
        {
            if (value != null)
            {
                sb.Append($"<{fieldName}>{SecurityElement.Escape(value.ToString())}</{fieldName}>");
            }
        }
        
        private void AppendFieldIfNotNull(StringBuilder sb, string fieldName, object value)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                AppendField(sb, fieldName, value);
            }
        }
    }

    /// <summary>
    /// Constants for work search query types
    /// </summary>
    public static class WorkSearchQueryTypes
    {
        /// <summary>
        /// Active records as of today
        /// </summary>
        public const string Active = "A";
        
        /// <summary>
        /// All work records as of today
        /// </summary>
        public const string All = "T";
        
        /// <summary>
        /// Active records during work period
        /// </summary>
        public const string Period = "P";
        
        /// <summary>
        /// Get all valid query types
        /// </summary>
        public static readonly string[] ValidQueryTypes = { Active, All, Period };
        
        /// <summary>
        /// Check if query type is valid
        /// </summary>
        public static bool IsValid(string queryType)
        {
            return ValidQueryTypes.Contains(queryType?.ToUpper());
        }
        
        /// <summary>
        /// Get description for query type
        /// </summary>
        public static string GetDescription(string queryType)
        {
            return queryType?.ToUpper() switch
            {
                Active => "Active records as of today",
                All => "All work records as of today", 
                Period => "Active records during work period",
                _ => "Unknown query type"
            };
        }
    }
        
    }

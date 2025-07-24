using System;
using System.ComponentModel.DataAnnotations;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class FindWorkRequest
    {
        /// <summary>
        /// Business registry code or personal code for whom work data is requested
        /// </summary>
        [Required]
        public string EmployerCode { get; set; }

        /// <summary>
        /// Query type: A=Active records as of today, T=All work records as of today, P=Active records during work period
        /// </summary>
        public string QueryType { get; set; } = "A";

        /// <summary>
        /// Start of work period (required when QueryType is P)
        /// </summary>
        public DateTime? WorkPeriodStart { get; set; }

        /// <summary>
        /// End of work period (required when QueryType is P)
        /// </summary>
        public DateTime? WorkPeriodEnd { get; set; }

        /// <summary>
        /// From which record number to show records in response (always set to 1 for first query)
        /// </summary>
        public int FromRecord { get; set; } = 1;

        /// <summary>
        /// User ID for the request
        /// </summary>
        public string UserId { get; set; }
    }
}
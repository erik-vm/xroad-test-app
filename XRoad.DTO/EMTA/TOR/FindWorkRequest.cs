using System.ComponentModel.DataAnnotations;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class FindWorkRequest
    {
        [Required] [StringLength(20)] public string EmployerCode { get; set; }

        /// <summary>
        /// Query type: A = Active records, H = History, etc.
        /// </summary>
        [StringLength(1)]
        public string QueryType { get; set; } = "A";

        /// <summary>
        /// Starting record number for pagination
        /// </summary>
        [Range(1, int.MaxValue)]
        public int FromRecord { get; set; } = 1;

        /// <summary>
        /// User ID making the request
        /// </summary>
        public string UserId { get; set; }
    }
}
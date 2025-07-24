using System;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class EmploymentSuspension
    {
        /// <summary>
        /// Employment suspension ID in employment register
        /// </summary>
        public long SuspensionId { get; set; }

        /// <summary>
        /// Employment suspension start date
        /// </summary>
        public DateTime? SuspensionStart { get; set; }

        /// <summary>
        /// Employment suspension reason code
        /// </summary>
        public string SuspensionReasonCode { get; set; }

        /// <summary>
        /// Employment suspension reason name
        /// </summary>
        public string SuspensionReasonName { get; set; }

        /// <summary>
        /// Employment suspension end date
        /// </summary>
        public DateTime? SuspensionEnd { get; set; }

        /// <summary>
        /// Last time suspension message was sent to EHK
        /// </summary>
        public DateTime? LastEhkSuspensionTime { get; set; }
    }
}
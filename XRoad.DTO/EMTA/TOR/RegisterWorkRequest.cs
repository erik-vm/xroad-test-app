using System;
using System.ComponentModel.DataAnnotations;

namespace XRoadTestApp.Models
{
    public class RegisterWorkRequest
    {
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string PersonalCode { get; set; }

        [Required] public DateTime StartDate { get; set; }

        [Range(0.1, 1.0)] public double WorkTimeRatio { get; set; } = 1.0;

        public string UserId { get; set; }
    }
}
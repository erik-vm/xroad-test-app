using System;
using System.Collections.Generic;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class RegisterWorkRequest
    {
        public string PersonalCode { get; set; }
        public DateTime? StartDate { get; set; }
        public double WorkTimeRatio { get; set; } = 1.0;
        public string UserId { get; set; }
        
        // Extended single record properties
        public long? WorkId { get; set; } // For modifications/cancellations
        public string RecordType { get; set; } = "R"; // R=Registration, M=Modification, T=Cancellation
        public DateTime? BirthDate { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? EmploymentType { get; set; }
        public string Contract { get; set; }
        public string ActualEmployerCode { get; set; }
        public string OccupationCode { get; set; }
        public int? WorkLocationAddressId { get; set; }
        public string ImoCode { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TerminationReasonCode { get; set; }
        public DateTime? SuspensionStartDate { get; set; }
        public DateTime? InitialSuspensionStartDate { get; set; }
        public int? SuspensionReasonCode { get; set; }
        public DateTime? SuspensionEndDate { get; set; }
        public string WorkProviderRemarks { get; set; }
        public string CountryCode { get; set; }
        public int? DefenseServiceMark { get; set; }
        public int? BackgroundCheckMark { get; set; }
        public string OccupationFreeText { get; set; }
        public string OccupationComment { get; set; }
        public string AddressFreeText { get; set; }
        
    }
}
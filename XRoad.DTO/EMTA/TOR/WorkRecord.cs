using System;
using System.Collections.Generic;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class WorkRecord
    {
        /// <summary>
        /// Work record ID in employment register
        /// </summary>
        public long WorkId { get; set; }

        /// <summary>
        /// Personal code
        /// </summary>
        public string PersonalCode { get; set; }

        /// <summary>
        /// Birth date of working person
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Citizenship code (ISO 3166-1 numeric)
        /// </summary>
        public string CitizenshipCode { get; set; }

        /// <summary>
        /// Citizenship country name
        /// </summary>
        public string CitizenshipName { get; set; }

        /// <summary>
        /// Work enabling person's identifier
        /// </summary>
        public string EmployerCode { get; set; }

        /// <summary>
        /// Work enabling person's name
        /// </summary>
        public string EmployerName { get; set; }

        /// <summary>
        /// Actual work enabling person's identifier
        /// </summary>
        public string ActualEmployerCode { get; set; }

        /// <summary>
        /// Actual work enabling person's name
        /// </summary>
        public string ActualEmployerName { get; set; }

        /// <summary>
        /// Start of employment
        /// </summary>
        public DateTime? EmploymentStart { get; set; }

        /// <summary>
        /// Employment type code
        /// </summary>
        public string EmploymentTypeCode { get; set; }

        /// <summary>
        /// Employment type name
        /// </summary>
        public string EmploymentTypeName { get; set; }

        /// <summary>
        /// Work time ratio
        /// </summary>
        public decimal? WorkTimeRatio { get; set; }

        /// <summary>
        /// Contract identifier
        /// </summary>
        public string Contract { get; set; }

        /// <summary>
        /// End of employment
        /// </summary>
        public DateTime? EmploymentEnd { get; set; }

        /// <summary>
        /// Employment termination reason code
        /// </summary>
        public string TerminationReasonCode { get; set; }

        /// <summary>
        /// Employment termination reason name
        /// </summary>
        public string TerminationReasonName { get; set; }

        /// <summary>
        /// Occupation code from Statistics classifier
        /// </summary>
        public string OccupationCode { get; set; }

        /// <summary>
        /// Occupation name from Statistics classifier
        /// </summary>
        public string OccupationName { get; set; }

        /// <summary>
        /// Work location ID according to ADS
        /// </summary>
        public long? LocationId { get; set; }

        /// <summary>
        /// Work location (work address) according to ADS
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Country code of work location
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Employment suspensions
        /// </summary>
        public List<EmploymentSuspension> Suspensions { get; set; } = new List<EmploymentSuspension>();

        /// <summary>
        /// Record status code
        /// </summary>
        public string RecordStatusCode { get; set; }

        /// <summary>
        /// Record status name
        /// </summary>
        public string RecordStatusName { get; set; }

        /// <summary>
        /// Work enabling person's remark
        /// </summary>
        public string EmployerRemark { get; set; }

        /// <summary>
        /// Official's remark
        /// </summary>
        public string OfficialRemark { get; set; }

        /// <summary>
        /// Defense service mark
        /// </summary>
        public int? DefenseServiceMark { get; set; }

        /// <summary>
        /// Background check mark for essential service provider
        /// </summary>
        public int? BackgroundCheckMark { get; set; }

        /// <summary>
        /// Start date decision type code
        /// </summary>
        public string StartDateDecisionTypeCode { get; set; }

        /// <summary>
        /// Start date decision type name
        /// </summary>
        public string StartDateDecisionTypeName { get; set; }

        /// <summary>
        /// Additional field for court decision number (start date)
        /// </summary>
        public string StartDateDecision { get; set; }

        /// <summary>
        /// Employment type decision type code
        /// </summary>
        public string EmploymentTypeDecisionTypeCode { get; set; }

        /// <summary>
        /// Employment type decision type name
        /// </summary>
        public string EmploymentTypeDecisionTypeName { get; set; }

        /// <summary>
        /// Additional field for court decision number (employment type)
        /// </summary>
        public string EmploymentTypeDecision { get; set; }

        /// <summary>
        /// End date decision type code
        /// </summary>
        public string EndDateDecisionTypeCode { get; set; }

        /// <summary>
        /// End date decision type name
        /// </summary>
        public string EndDateDecisionTypeName { get; set; }

        /// <summary>
        /// Additional field for court decision number (end date)
        /// </summary>
        public string EndDateDecision { get; set; }

        /// <summary>
        /// Termination reason decision type code
        /// </summary>
        public string TerminationReasonDecisionTypeCode { get; set; }

        /// <summary>
        /// Termination reason decision type name
        /// </summary>
        public string TerminationReasonDecisionTypeName { get; set; }

        /// <summary>
        /// Additional field for court decision number (termination reason)
        /// </summary>
        public string TerminationReasonDecision { get; set; }

        /// <summary>
        /// Record entry time
        /// </summary>
        public DateTime? EntryTime { get; set; }

        /// <summary>
        /// Record modification time
        /// </summary>
        public DateTime? ModificationTime { get; set; }

        /// <summary>
        /// Phone number for simplified registration (SMS or phone call)
        /// </summary>
        public string RegistrationPhoneNumber { get; set; }

        /// <summary>
        /// Last time employment message was sent to EHK
        /// </summary>
        public DateTime? LastEhkMessageTime { get; set; }

        /// <summary>
        /// Type of last employment message sent to EHK (K/L/I)
        /// </summary>
        public string LastEhkMessageType { get; set; }

        /// <summary>
        /// Occupation in free text
        /// </summary>
        public string OccupationFreeText { get; set; }

        /// <summary>
        /// Occupation comment
        /// </summary>
        public string OccupationComment { get; set; }

        /// <summary>
        /// Work location address in free text
        /// </summary>
        public string AddressFreeText { get; set; }
    }
}
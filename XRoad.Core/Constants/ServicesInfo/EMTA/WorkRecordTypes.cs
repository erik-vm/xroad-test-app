using System.Linq;

namespace XRoadTestApp.XRoad.Core.Constants.ServicesInfo.EMTA
{
    public class WorkRecordTypes
    {
        /// <summary>
        /// Initial registration
        /// </summary>
        public const string Registration = "R";

        /// <summary>
        /// Record modification
        /// </summary>
        public const string Modification = "M";

        /// <summary>
        /// Record cancellation
        /// </summary>
        public const string Cancellation = "T";

        /// <summary>
        /// Get all valid record types
        /// </summary>
        public static readonly string[] ValidRecordTypes = { Registration, Modification, Cancellation };

        /// <summary>
        /// Check if record type is valid
        /// </summary>
        public static bool IsValid(string recordType)
        {
            return ValidRecordTypes.Contains(recordType?.ToUpper());
        }

        /// <summary>
        /// Get description for record type
        /// </summary>
        public static string GetDescription(string recordType)
        {
            return recordType?.ToUpper() switch
            {
                Registration => "Initial registration",
                Modification => "Record modification",
                Cancellation => "Record cancellation",
                _ => "Unknown record type"
            };
        }
    }
}
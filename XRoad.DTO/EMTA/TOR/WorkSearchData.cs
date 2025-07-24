using System.Collections.Generic;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class WorkSearchData
    {
        public string FaultCode { get; set; }
        public string FaultString { get; set; }
        public int Code { get; set; }
        public int TotalRecordsFound { get; set; }
        public int FromRecord { get; set; }
        public int ToRecord { get; set; }
        public List<WorkRecord> WorkRecords { get; set; } = new List<WorkRecord>();
    }
}
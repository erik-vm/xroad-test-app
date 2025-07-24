using System.Collections.Generic;
using XRoadTestApp.XRoad.DTO.Validation;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class FindWorkResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public WorkSearchData Data { get; set; }
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    }
}
using System.Collections.Generic;
using XRoadTestApp.XRoad.DTO.Validation;

namespace XRoadTestApp.XRoad.DTO.Shared
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public List<ValidationError> Errors { get; set; } 
    }
}
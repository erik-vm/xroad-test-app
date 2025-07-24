using System;
using System.Collections.Generic;
using XRoadTestApp.XRoad.DTO.Shared;

namespace XRoadTestApp.XRoad.Services.SoapService.Interfaces
{
    public interface ISoapService
    {
        public string BuildSoapBody<T>(IEnumerable<T> items, SoapBodyConfig config,
            Func<T, Dictionary<string, object>> fieldMapper);
    }
}
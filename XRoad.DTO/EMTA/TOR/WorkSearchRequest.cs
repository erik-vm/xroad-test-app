using System;

namespace XRoadTestApp.XRoad.DTO.EMTA.TOR
{
    public class WorkSearchRequest
    {
        public string ParinguLiik { get; set; } // A, T, P
        public DateTime? TootAlgus { get; set; }
        public DateTime? TootLopp { get; set; }
        public string TvoimId { get; set; }
        public int AlatesKandest { get; set; } = 1;
    }
}
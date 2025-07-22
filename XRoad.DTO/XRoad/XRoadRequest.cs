namespace XRoadTestApp.Models
{
    public class XRoadRequest
    {
        public XRoadServiceInfo Service { get; set; }
        public string UserId { get; set; }
        public object RequestData { get; set; }
    }
}
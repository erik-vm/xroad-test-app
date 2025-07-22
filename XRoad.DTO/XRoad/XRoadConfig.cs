namespace XRoadTestApp.Models
{
    public class XRoadConfig
    {
        public string SecurityServerUrl { get; set; }
        public string ClientInstance { get; set; }
        public string ClientMemberClass { get; set; }
        public string ClientMemberCode { get; set; }
        public string ClientSubsystemCode { get; set; }
        public string ProtocolVersion { get; set; }
        public bool IgnoreSslErrors { get; set; }
        public string ClientCertificatePath { get; set; }
        public string ClientCertificatePassword { get; set; }
        public int Timeout { get; set; } = 30;
    }
}
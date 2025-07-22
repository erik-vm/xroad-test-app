using XRoadTestApp.Models;

namespace XRoadTestApp.XRoad.Data.EMTA
{
    public abstract class Tor
    {
        public static XRoadServiceInfo TOOTREG => new XRoadServiceInfo
        {
            Instance = "ee-test",
            MemberClass = "GOV",
            MemberCode = "70000349",
            SubsystemCode = "tor",
            ServiceCode = "TOOTREG",
            ServiceVersion = "v2"
        };

        public static XRoadServiceInfo TORRGNO => new XRoadServiceInfo
        {
            Instance = "ee-test",
            MemberClass = "GOV",
            MemberCode = "70000349",
            SubsystemCode = "tor",
            ServiceCode = "TORRGNO",
            ServiceVersion = "v2"
        };
    }
}
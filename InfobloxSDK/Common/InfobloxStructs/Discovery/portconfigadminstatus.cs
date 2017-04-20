using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Discovery
{
    public class portconfigadminstatus
    {
        public portcontroltaskdetails details { get; set; }
        public UpDownEnum status { get; set; }
    }
}

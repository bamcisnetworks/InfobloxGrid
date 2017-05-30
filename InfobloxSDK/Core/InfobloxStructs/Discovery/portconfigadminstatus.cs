using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class portconfigadminstatus
    {
        public portcontroltaskdetails details { get; set; }
        public UpDownEnum status { get; set; }
    }
}

using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class servicestatus
    {
        public string description { get; set; }
        public ServiceEnum service { get; set; }
        public ServiceStatusEnum status { get; set; }
    }
}

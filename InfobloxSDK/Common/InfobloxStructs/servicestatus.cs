using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class servicestatus
    {
        public string description { get; set; }
        public ServiceEnum service { get; set; }
        public ServiceStatusEnum status { get; set; }
    }
}

using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Discovery
{
    public class portcontroltaskdetails
    {
        public uint id { get; set; }
        public bool is_synchronized { get; set; }
        public TaskStatusEnum status { get; set; }
    }
}

using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class portcontroltaskdetails
    {
        public uint id { get; set; }
        public bool is_synchronized { get; set; }
        public TaskStatusEnum status { get; set; }
    }
}

using BAMCIS.Infoblox.Core.Enums;
using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class statusinfo
    {
        public StatusInfoEnum status { get; set; }
        public DateTime timestamp { get; set; }
    }
}

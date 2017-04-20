using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Discovery
{
    public class statusinfo
    {
        public StatusInfoEnum status { get; set; }
        public DateTime timestamp { get; set; }
    }
}

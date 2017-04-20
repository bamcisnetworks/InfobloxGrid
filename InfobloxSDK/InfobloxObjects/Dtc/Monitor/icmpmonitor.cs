using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor
{
    [Name("dtc:monitor:icmp")]
    public class icmpmonitor : BaseMonitorClass
    {
        public icmpmonitor(string name)
        {
            base.comment = String.Empty;
            base.interval = 5;
            base.name = name;
            base.retry_down = 1;
            base.retry_up = 1;
            base.timeout = 15;
        }
    }
}

using BAMCIS.Infoblox.Core;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor
{
    [Name("dtc:monitor:pdp")]
    public class pdpmonitor : tcpmonitor
    {
        public pdpmonitor(string name) : base(name)
        {
            this.port = 2123;
        }
    }
}

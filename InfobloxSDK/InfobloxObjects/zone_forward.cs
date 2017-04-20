using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using BAMCIS.Infoblox.Common.InfobloxStructs;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("zone_forward")]
    public class zone_forward : BaseZone
    {
        [Required]
        public extserver[] forward_to { get; set; }
        public bool forwarders_only { get; set; }
        public forwardingmemberserver[] forwarding_servers { get; set; }

        public zone_forward(extserver[] forward_to, string fqdn) : base(fqdn)
        {
            this.forward_to = forward_to;
            this.forwarders_only = false;
            this.forwarding_servers = new forwardingmemberserver[0];
        }
    }
}

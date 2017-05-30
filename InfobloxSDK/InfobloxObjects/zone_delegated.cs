using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("zone_delegated")]
    public class zone_delegated : BaseZone
    {
        [Required]
        public extserver[] delegate_to { get; set; }
        public uint delegated_ttl { get; set; }
        public bool enable_rfc2317_exception { get; set; }
        
        public zone_delegated(extserver[] delegate_to, string fqdn) : base(fqdn)
        {
            this.delegate_to = delegate_to;
            this.enable_rfc2317_exception = false;
        }
    }
}

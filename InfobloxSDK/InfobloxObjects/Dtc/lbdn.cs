using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs.Dtc;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc
{
    [Name("dtc:lbdn")]
    public class lbdn : BaseNameCommentObject
    {
        public zone_auth[] auth_zones { get; set; }
        public bool disable { get; set; }
        [ReadOnlyAttribute]
        public health health { get; internal protected set; }
        [Required]
        public LbMethodEnum lb_method { get; set; }
        public string[] patterns { get; set; }
        public uint persistence { get; set; }
        public poollink[] pools { get; set; }
        public string topology { get; set; }
        public uint ttl { get; set; }
        public bool use_ttl { get; set; }

        public lbdn(string name, LbMethodEnum lb_method)
        {
            if (lb_method != LbMethodEnum.TOPOLOGY)
            {
                this.auth_zones = new zone_auth[0];
                this.disable = false;
                this.lb_method = lb_method;
                base.name = name;
                this.patterns = new string[0];
                this.persistence = 0;
            }
            else
            {
                throw new ArgumentException("You must specify topology and pools when the load balancing method is \"Topology\". Use a different constructor.");
            }
        }

        public lbdn(string name, LbMethodEnum lb_method, string topology, poollink[] pools)
        {
            this.auth_zones = new zone_auth[0];
            this.disable = false;
            this.lb_method = lb_method;
            base.name = name;
            this.patterns = new string[0];
            this.persistence = 0;
        }
    }
}

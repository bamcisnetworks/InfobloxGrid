using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs.Dtc;
using BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc
{
    [Name("dtc:pool")]
    public class pool : BaseNameCommentObject
    {
        public PoolAvailabilityEnum availability { get; set; }

        public bool disable { get; set; }

        [ReadOnlyAttribute]
        public health health { get; internal protected set; }

        public LbMethodEnum lb_alternate_method { get; set; }

        public string lb_alternate_topology { get; set; }

        [Required]
        public LbMethodEnum lb_preferred_method { get; set; }

        public string lb_preferred_topology { get; set; }

        public tcpmonitor[] monitors { get; set; }

        public uint quorum { get; set; }

        public serverlink[] servers { get; set; }

        public uint ttl { get; set; }

        public bool use_ttl { get; set; }

        public pool(string name, LbMethodEnum lb_preferred_method)
        {
            this.disable = false;
            this.lb_alternate_topology = String.Empty;
            this.lb_preferred_method = lb_preferred_method;
            this.lb_preferred_topology = String.Empty;
            this.monitors = new tcpmonitor[0];
            this.name = name;
            this.use_ttl = false;
        }
    }
}

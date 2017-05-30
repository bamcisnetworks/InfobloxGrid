using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Dtc
{
    public class topologyrulesource
    {
        public DtcTopologyRuleSourceOpEnum source_op { get; set; }
        public DtcTopologyRuleSourceTypeEnum source_type { get; set; }
        public string source_value { get; set; }
    }
}

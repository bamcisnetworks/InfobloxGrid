using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Dtc
{
    public class topologyrulesource
    {
        public DtcTopologyRuleSourceOpEnum source_op { get; set; }
        public DtcTopologyRuleSourceTypeEnum source_type { get; set; }
        public string source_value { get; set; }
    }
}

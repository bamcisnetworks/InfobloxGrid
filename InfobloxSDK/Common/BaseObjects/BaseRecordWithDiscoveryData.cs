using BAMCIS.Infoblox.Common.InfobloxStructs;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseRecordWithDiscoveryData : BaseRecord
    {
        [ReadOnlyAttribute]
        [SearchableAttribute(ContainsSearchable = true)]
        public discoverydata discovered_data { get; internal protected set; }
    }
}

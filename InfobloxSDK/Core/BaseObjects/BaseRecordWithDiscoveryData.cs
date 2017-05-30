using BAMCIS.Infoblox.Core.InfobloxStructs;

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseRecordWithDiscoveryData : BaseRecord
    {
        [ReadOnlyAttribute]
        [SearchableAttribute(ContainsSearchable = true)]
        public discoverydata discovered_data { get; internal protected set; }
    }
}

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:aaaa")]
    public class aaaa : BaseRecordWithDiscoveryData
    {

        private string _ipv6addr;

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                NetworkAddressTest.isIPv4WithException(value, out this._ipv6addr);
            }
        }
        
        public aaaa(string ipv6addr, string name)
        {
            this.ipv6addr = ipv6addr;
            base.name = name;
        }
    }
}

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS.Shared
{
    [Name("sharedrecord:aaaa")]
    public class sharedrecord_aaaa : BaseSharedRecord
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
                NetworkAddressTest.isIPv6WithException(value, out this._ipv6addr);
            }
        }

        public sharedrecord_aaaa(string ipv6addr, string name, string shared_record_group) : base(shared_record_group)
        {
            this.ipv6addr = ipv6addr;
            base.name = name;
        }
    }
}

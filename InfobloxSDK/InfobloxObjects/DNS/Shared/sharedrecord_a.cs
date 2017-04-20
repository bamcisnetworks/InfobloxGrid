using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS.Shared
{
    [Name("sharedrecord:a")]
    public class sharedrecord_a : BaseSharedRecord
    {
        private string _ipv4addr;

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                NetworkAddressTest.isIPv4WithException(value, out this._ipv4addr);
            }
        }

        public sharedrecord_a(string ipv4addr, string name, string shared_record_group) : base(shared_record_group)
        {
            this.ipv4addr = ipv4addr;
            base.name = name;
        }
    }
}

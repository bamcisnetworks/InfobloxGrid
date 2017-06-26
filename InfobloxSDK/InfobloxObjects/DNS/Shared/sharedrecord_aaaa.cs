using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS.Shared
{
    [Name("sharedrecord:aaaa")]
    public class sharedrecord_aaaa : BaseSharedRecord
    {
        private string _ipv6addr;

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv6Address(value, out IP, false, true))
                {
                    this._ipv6addr = IP.ToString();
                }
            }
        }

        public sharedrecord_aaaa(string ipv6addr, string name, string shared_record_group) : base(shared_record_group)
        {
            this.ipv6addr = ipv6addr;
            base.name = name;
        }
    }
}

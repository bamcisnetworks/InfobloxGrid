using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:aaaa")]
    public class aaaa : BaseRecordWithDiscoveryData
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
        
        public aaaa(string ipv6addr, string name)
        {
            this.ipv6addr = ipv6addr;
            base.name = name;
        }
    }
}

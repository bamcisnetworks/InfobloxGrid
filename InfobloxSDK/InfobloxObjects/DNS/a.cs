using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:a")]
    public class a : BaseRecordWithDiscoveryData
    {
        private string _ipv4addr;

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string ipv4addr 
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, false, true))
                {
                    this._ipv4addr = IP.ToString();
                }
            }
        }

        public a(string name, string ipv4addr)
        {
            base.name = name;
            this.ipv4addr = ipv4addr;
        }
    }
}

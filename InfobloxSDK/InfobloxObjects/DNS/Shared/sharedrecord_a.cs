using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS.Shared
{
    [Name("sharedrecord:a")]
    public class sharedrecord_a : BaseSharedRecord
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

        public sharedrecord_a(string ipv4addr, string name, string shared_record_group) : base(shared_record_group)
        {
            this.ipv4addr = ipv4addr;
            base.name = name;
        }
    }
}

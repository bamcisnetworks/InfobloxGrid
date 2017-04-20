using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:a")]
    public class a : BaseRecordWithDiscoveryData
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

        public a(string name, string ipv4addr)
        {
            base.name = name;
            this.ipv4addr = ipv4addr;
        }
    }
}

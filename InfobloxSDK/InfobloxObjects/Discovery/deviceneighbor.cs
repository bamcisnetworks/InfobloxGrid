using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    [Name("discovery:deviceneighbor")]
    public class deviceneighbor : RefObject
    {
        private string _address;
        private string _mac;

        [ReadOnly]
        [Basic]
        public string address
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPAddress(value, out this._address, false, true);
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string address_ref { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string device { get; internal protected set; }

        [ReadOnlyAttribute]
        public string @interface { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public string mac
        {
            get
            {
                return this._mac;
            }
            internal protected set
            {
                NetworkAddressTest.IsMAC(value, out this._mac, true, true);
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string name { get; internal protected set; }

        [ReadOnlyAttribute]
        public vlaninfo[] vlan_infos { get; internal protected set; }
    }
}

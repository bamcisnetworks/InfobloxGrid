using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.InfobloxStructs.Discovery;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    [Name("discovery:deviceneighbor")]
    public class deviceneighbor
    {
        private string _address;
        private string _mac;

        public string address
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.isIPWithException(value, out this._address);
            }
        }
        [ReadOnlyAttribute]
        public string address_ref { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string device { get; internal protected set; }
        [ReadOnlyAttribute]
        public string @interface { get; internal protected set; }
        [ReadOnlyAttribute]
        public string mac
        {
            get
            {
                return this._mac;
            }
            internal protected set
            {
                NetworkAddressTest.IsMACWithExceptionAllowEmpty(value, out this._mac);
            }
        }
        [ReadOnlyAttribute]
        public string name { get; internal protected set; }
        [ReadOnlyAttribute]
        public vlaninfo[] vlan_infos { get; internal protected set; }
    }
}

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("restartservicestatus")]
    public class restartservicestatus
    {
        private string _member;

        [ReadOnlyAttribute]
        public RestartStatusEnum dhcp_status { get; internal protected set; }
        [ReadOnlyAttribute]
        public RestartStatusEnum dns_status { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string member
        {
            get
            {
                return this._member;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdnWithException(value, "member", out this._member);
            }
        }
        [ReadOnlyAttribute]
        public RestartStatusEnum reporting_status { get; internal protected set; }
    }
}

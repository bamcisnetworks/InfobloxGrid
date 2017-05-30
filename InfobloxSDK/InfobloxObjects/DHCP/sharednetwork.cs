using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("sharednetwork")]
    public class sharednetwork : BaseNetwork
    {
        private string _name;

        [ReadOnlyAttribute]
        public uint dhcp_utilization { get; internal protected set; }
        [ReadOnlyAttribute]
        public DHCPUtilizationEnum dhcp_utilization_status { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint dynamic_hosts { get; internal protected set; }
        public bool ignore_client_identifier { get; set; }
        [Required]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValueWithException("name");
            }
        }
        [Required]
        public ipv4network[] networks { get; set; }
        [ReadOnlyAttribute]
        public uint static_hosts { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint total_hosts { get; internal protected set; }
        public bool use_ignore_client_identifier { get; set; }

        public sharednetwork(string name, ipv4network[] networks)
        {
            this.name = name;
            this.networks = networks;
            this.use_ignore_client_identifier = false;
        }
    }
}

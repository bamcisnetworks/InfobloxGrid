using BAMCIS.Infoblox.Core.InfobloxStructs;

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseNetworkLow : BaseCommentObject
    {
        private string _network_view;

        public bool ddns_generate_hostname { get; set; }

        public bool ddns_server_always_updates { get; set; }

        public uint ddns_ttl { get; set; }

        public bool disable { get; set; }

        public bool enable_ddns { get; set; }

        [Basic]
        public string network_view
        {
            get
            {
                return this._network_view;
            }
            set
            {
                this._network_view = value.TrimValue();
            }
        }

        public dhcpoption[] options { get; set; }

        public bool update_dns_on_lease_renewal { get; set; }

        public bool use_ddns_generate_hostname { get; set; }

        public bool use_ddns_ttl { get; set; }

        public bool use_enable_ddns { get; set; }

        public bool use_options { get; set; }

        public bool use_update_dns_on_lease_renewal { get; set; }

        public BaseNetworkLow()
        {
            this.ddns_generate_hostname = false;
            this.ddns_server_always_updates = true;
            this.ddns_ttl = 0;
            this.disable = false;
            this.enable_ddns = false;
            this.network_view = "default";
            this.options = dhcpoption.DefaultArray();
            this.update_dns_on_lease_renewal = false;
            this.use_ddns_generate_hostname = false;
            this.use_ddns_ttl = false;
            this.use_enable_ddns = false;
            this.use_options = false;
            this.use_update_dns_on_lease_renewal = false;
        }
    }
}

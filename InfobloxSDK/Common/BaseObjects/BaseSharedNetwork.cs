using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseSharedNetwork : BaseNetworkMid
    {
        private List<string> _domain_name_servers;


        public string[] domain_name_servers
        {
            get
            {
                return this._domain_name_servers.ToArray();
            }
            set
            {
                this._domain_name_servers = new List<string>();

                foreach (string item in value)
                {
                    string temp = String.Empty;
                    NetworkAddressTest.isIPWithException(item, out temp);
                    this._domain_name_servers.Add(temp);
                }
            }
        }
        public uint preferred_lifetime { get; set; }
        public bool use_domain_name { get; set; }
        public bool use_domain_name_servers { get; set; }
        public bool use_preferred_lifetime { get; set; }
        public bool use_valid_lifetime { get; set; }
        public uint valid_lifetime { get; set; }

        public BaseSharedNetwork()
        {
            this.domain_name_servers = new string[0];
            this.preferred_lifetime = 27000;
            this.use_domain_name = false;
            this.use_domain_name_servers = false;
            this.use_preferred_lifetime = false;
            this.use_valid_lifetime = false;
            this.valid_lifetime = 43200;
        }
    }
}

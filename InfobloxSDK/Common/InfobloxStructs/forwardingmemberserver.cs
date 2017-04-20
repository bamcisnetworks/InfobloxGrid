using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class forwardingmemberserver
    {
        private string _name;

        public extserver forward_to { get; set; }
        public bool forwarders_only { get; set; }
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (NetworkAddressTest.IsFqdn(value))
                {
                    this._name = value;
                }
                else
                {
                    throw new ArgumentException("The name must be a valid FQDN.");
                }
            }
        }
        public bool use_override_forwarders { get; set; }
        
        public forwardingmemberserver()
        {
            this.forwarders_only = false;
            this.use_override_forwarders = false;
        }
    }
}

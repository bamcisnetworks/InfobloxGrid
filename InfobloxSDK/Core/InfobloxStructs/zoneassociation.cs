using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class zoneassociation
    {
        private string _fqdn;
        private string _view;

        public string fqdn
        {
            get
            {
                return this._fqdn;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "fqdn", out this._fqdn, false, true);
            }
        }
        public bool is_default { get; set; }
        public string view
        {
            get
            {
                return this._view;
            }
            set
            {
                this._view = value.TrimValue();
            }
        }
    }
}

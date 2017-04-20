using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
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
                NetworkAddressTest.IsFqdnWithException(value, "fqdn", out this._fqdn);
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

using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class dnssectrustedkey
    {
        private string _algorithm;
        private string _fqdn;
        private string _key;

        public string algorithm
        {
            get
            {
                return this._algorithm;
            }
            set
            {
                this._algorithm = value.TrimValue();
            }
        }
        public string fqdn
        {
            get
            {
                return this._fqdn;
            }
            set
            {
                if (NetworkAddressTest.IsFqdn(value))
                {
                    this._fqdn = value;
                }
                else
                {
                    throw new ArgumentException("The provided FQDN is not valid.");
                }
            }
        }
        public string key
        {
            get
            {
                return this._key;
            }
            set
            {
                this._key = value.TrimValue();
            }
        }
        public bool secure_entry_point { get; set; }

        public dnssectrustedkey()
        {
            secure_entry_point = true;
        }
    }
}

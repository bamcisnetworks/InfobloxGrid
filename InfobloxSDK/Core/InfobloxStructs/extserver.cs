using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class extserver
    {
        private string _address;
        private string _name;
        private string _tsig_key;
        private string _tsig_key_alg;
        private string _tsig_key_name;

        public string address
        {
            get
            {
                return this._address;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    this._address = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The address must be a valid IPv4 or IPv6 address.");
                }
        
            }
        }
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValue();
            }
        }
        public bool shared_with_ms_parent_delegation { get; set; }
        public bool stealth { get; set; }
        public string tsig_key
        {
            get
            {
                return this._tsig_key;
            }
            set
            {
                this._tsig_key = value.TrimValue();
            }
        }
        public string tsig_key_alg
        {
            get
            {
                return this._tsig_key_alg;
            }
            set
            {
                if (!String.IsNullOrEmpty(value) && ( value.Equals("HMAC-MD5") || value.Equals("HMAC-SHA256")))
                {
                    this._tsig_key_alg = value;
                }
                else
                {
                    throw new ArgumentException("The TSIG key algorithm must be valid.");
                }
            }
        }
        public string tsig_key_name
        {
            get
            {
                return this._tsig_key_name;
            }
            set
            {
                this._tsig_key_name = value.TrimValue();
            }
        }

        public bool use_tsig_key_name { get; set; }

        public extserver()
        {
            this.stealth = false;
            this.tsig_key_alg = "HMAC-MD5";
            this.use_tsig_key_name = false;
        }
    }
}

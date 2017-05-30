using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class tsigac
    {
        private string _tsig_key;
        private string _tsig_key_alg;
        private string _tsig_key_name;

        public string tsig_key 
        { 
            get
            {
                return this._tsig_key;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this._tsig_key = value.TrimValue();
                }
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
                if (!String.IsNullOrEmpty(value) && (value.Equals("HMAC-MD5", StringComparison.OrdinalIgnoreCase) || value.Equals("HMAC-SHA256", StringComparison.OrdinalIgnoreCase)))
                {
                    this._tsig_key_alg = value.ToUpper();
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
                if (!String.IsNullOrEmpty(value))
                {
                    this._tsig_key_name = value.TrimValue();
                }
            }
        }
        public bool use_tsig_key_name { get; set; }
    
        public tsigac()
        {
            this.tsig_key_alg = TsigAlgorithmConst.HMAC_MD5;
            this.use_tsig_key_name = false;
        }
    }
}

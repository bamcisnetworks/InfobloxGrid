using BAMCIS.Infoblox.Core.Enums;
using System.Security;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class snmp3credential : RefObject
    {
        private SecureString _authentication_password;
        private string _comment;
        private SecureString _privacy_password;
        private string _user;

        [NotReadableAttribute]
        public string authentication_password 
        {
            get
            {
                return this._authentication_password.ToReadableString();
            }
            set
            {
                this._authentication_password = value.TrimValue().ToSecureString();
            }
        }
        public AuthenticationProtocolEnum authentication_protocol { get; set; }
        public string comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value.TrimValue();
            }
        }
        [NotReadableAttribute]
        public string privacy_password 
        { 
            get
            {
                return this._privacy_password.ToReadableString();
            }
            set
            {
                this._privacy_password = value.TrimValue().ToSecureString();
            }
        }
        public PrivacyProtocolEnum privacy_protocol { get; set; }
        public string user 
        {
            get
            {
                return this.user;
            }
            set
            {
                this._user = value.TrimValue();
            }
        }
    }
}

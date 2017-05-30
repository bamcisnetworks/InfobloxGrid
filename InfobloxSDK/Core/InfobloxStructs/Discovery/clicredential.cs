using BAMCIS.Infoblox.Core.Enums;
using System.Security;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class clicredential
    {
        private string _comment;
        private SecureString _password;
        private string _user;

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
        public CredentialTypeEnum credential_type { get; set; }
        public uint id { get; set; }
        [NotReadableAttribute]
        public string password
        {
            get
            {
                return SecureStringHelper.ToReadableString(this._password);
            }
            set
            {
                this._password = value.TrimValue().ToSecureString();
            }
        }
        public string user 
        { 
            get
            {
                return this._user;
            }
            set
            {
                this._user = value.TrimValue();
            }
        }
    }
}

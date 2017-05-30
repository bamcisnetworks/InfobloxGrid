using System;
using System.Security;

namespace BAMCIS.Infoblox.Core
{
    public class InfobloxCredential
    {
        public string UserName { get; set; }
        public SecureString Password { get; set; }

        public InfobloxCredential()
        {
            this.UserName = String.Empty;
            this.Password = null;
        }

        public InfobloxCredential(string userName)
        {
            if (!String.IsNullOrEmpty(userName))
            {
                this.UserName = userName;
            }
            else
            {
                throw new ArgumentNullException("userName", "The username cannot be null or empty.");
            }
        }

        public InfobloxCredential(string userName, SecureString password) : this(userName)
        {
            if (password != null)
            {
                this.Password = password;
            }
            else
            {
                throw new ArgumentNullException("password", "The password cannot be null.");
            }
        }
    }
}

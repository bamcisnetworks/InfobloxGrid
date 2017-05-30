using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Linq;
using System.Reflection;
using System.Security;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("adminuser")]
    public class adminuser : BaseNameCommentObject
    {
        private string[] _admin_groups;
        private string _email;
        private SecureString _password;
        private string _time_zone;

        [Required]
        [SearchableAttribute(Equality = true)]
        public string[] admin_groups
        {
            get
            {
                return this._admin_groups;
            }
            set
            {
                if (value.Length <= 1)
                {
                    this._admin_groups = value;
                }
                else
                {
                    throw new ArgumentException("Currently a user's admin groups property only supports 1 group.");
                }
            }
        }
        public bool disable { get; set; }
        public string email
        {
            get
            {
                return this._email;
            }
            set
            {
                NetworkAddressTest.IsValidEmail(value, out this._email, false, true);
            }
        }
        [Required]
        [NotReadableAttribute]
        public string password
        {
            get
            {
                return SecureStringHelper.ToReadableString(this._password);
            }
            set
            {
                this._password = SecureStringHelper.ToSecureString(value.TrimValue());
            }
        }
        public string time_zone
        {
            get
            {
                return this._time_zone;
            }
            set
            {
                if (typeof(TimeZoneConst).GetTypeInfo().GetFields().Where(x => x.IsStatic && !x.IsInitOnly).Select(x => x.GetValue(x).ToString().ToUpper()).Contains(value.ToUpper()))
                {
                    this._time_zone = value;
                }
                else
                {
                    throw new ArgumentException("The time zone string provided is not valid for this object.");
                }
            }
        }
        public bool use_time_zone { get; set; }

        public adminuser(string name)
        {
            this.disable = false;
            this.name = name;
            this.time_zone = TimeZoneConst.UTC;
            this.use_time_zone = false;
        }
    }
}

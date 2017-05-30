using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("macaddressfilter")]
    public class macaddressfilter : BaseCommentObject
    {
        private DateTime _authentication_time;
        private DateTime _expiration_time;
        private string _guest_custom_field1;
        private string _guest_custom_field2;
        private string _guest_custom_field3;
        private string _guest_custom_field4;
        private string _guest_email;
        private string _guest_first_name;
        private string _guest_last_name;
        private string _guest_middle_name;
        private string _guest_phone;
        private string _mac;
        private string _reserved_for_infoblox;
        private string _username;

        [SearchableAttribute(Negative = true, LessThan = true, Equality = true, GreaterThan = true)]
        public long authentication_time 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._authentication_time);
            }
            set
            {
                this._authentication_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [SearchableAttribute(Negative = true, LessThan = true, Equality = true, GreaterThan = true)]
        public long expiration_time 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._expiration_time);
            }
            set
            {
                this._expiration_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string filter { get; set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string fingerprint { get; internal protected set; }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_custom_field1 
        {
            get
            {
                return this._guest_custom_field1;
            }
            set
            {
                this._guest_custom_field1 = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_custom_field2
        {
            get
            {
                return this._guest_custom_field2;
            }
            set
            {
                this._guest_custom_field2 = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_custom_field3
        {
            get
            {
                return this._guest_custom_field3;
            }
            set
            {
                this._guest_custom_field3 = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_custom_field4
        {
            get
            {
                return this._guest_custom_field4;
            }
            set
            {
                this._guest_custom_field4 = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_email 
        {
            get
            {
                return this._guest_email;
            }
            set
            {
                NetworkAddressTest.IsValidEmail(value, out this._guest_email, true, true);
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_first_name
        {
            get
            {
                return this._guest_first_name;
            }
            set
            {
                this._guest_first_name = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_last_name
        {
            get
            {
                return this._guest_last_name;
            }
            set
            {
                this._guest_last_name = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_middle_name
        {
            get
            {
                return this._guest_middle_name;
            }
            set
            {
                this._guest_middle_name = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string guest_phone
        {
            get
            {
                return this._guest_phone;
            }
            set
            {
                this._guest_phone = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public bool is_registered_user { get; internal protected set; }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string mac 
        {
            get
            {
                return this._mac;
            }
            set
            {
                NetworkAddressTest.IsMAC(value, out this._mac, true, true);
            }
        }
        [SearchableAttribute(Equality = true)]
        public bool never_expires { get; set; }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string reserved_for_infoblox
        {
            get
            {
                return this._reserved_for_infoblox;
            }
            set
            {
                this._reserved_for_infoblox = value.TrimValue();
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string username
        {
            get
            {
                return this._username;
            }
            set
            {
                this._username = value.TrimValue();
            }
        }

        public macaddressfilter(string filter, string mac)
        {
            this.filter = filter;
            this.guest_custom_field1 = String.Empty;
            this.guest_custom_field2 = String.Empty;
            this.guest_custom_field3 = String.Empty;
            this.guest_custom_field4 = String.Empty;
            this.guest_email = String.Empty;
            this.guest_first_name = String.Empty;
            this.guest_last_name = String.Empty;
            this.guest_middle_name = String.Empty;
            this.guest_phone = String.Empty;
            this.mac = mac;
            this.reserved_for_infoblox = String.Empty;
            this.username = String.Empty;
        }
    }
}

using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;
using System.Linq;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:naptr")]
    public class naptr : BaseRecord
    {
        private string _dns_replacement;
        private string _flags;
        private DateTime _last_queried;
        private uint _order;
        private uint _preference;
        private string _regexp;
        private string _replacment;
        private string _services;

        [ReadOnlyAttribute]
        public string dns_replacement
        {
            get
            {
                return this._dns_replacement;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_replacement", out this._dns_replacement, false, true);
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string flags
        {
            get
            {
                return this._flags;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    string[] allowed = new string[] { "U", "S", "P", "A" };
                    if (allowed.Contains(value.ToUpper()))
                    {
                        this._flags = value.ToUpper();
                    }
                }
                else
                {
                    this._flags = String.Empty;
                }
            }
        }
        [ReadOnlyAttribute]
        public long last_queried
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_queried);
            }
            internal protected set
            {
                this._last_queried = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [Required]
        [SearchableAttribute(LessThan = true, Equality = true, GreaterThan = true)]
        public uint order
        {
            get
            {
                return this._order;
            }
            set
            {
                if (value >= 0 && value <= 65535)
                {
                    this._order = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("The order property must be between 0 and 65535 inclusive, {0} was provided.", value));
                }
            }
        }
        [Required]
        [SearchableAttribute(LessThan = true, Equality = true, GreaterThan = true)]
        public uint preference
        {
            get
            {
                return this._preference;
            }
            set
            {
                if (value >= 0 && value <= 65535)
                {
                    this._preference = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("The preference property must be between 0 and 65535 inclusive, {0} was provided.", value));
                }
            }
        }
        public string regexp
        {
            get
            {
                return this._regexp;
            }
            set
            {
                this._regexp = value.TrimValue();
            }
        }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string replacement
        {
            get
            {
                return this._replacment;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "replacement", out this._replacment, false, true);
            }
        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string services
        {
            get
            {
                return this._services;
            }
            set
            {
                if (value.TrimValue().Length <= 128)
                {
                    this._services = value.TrimValue();
                }
            }
        }

        public naptr(string name, uint order, uint preference, string replacement)
        {
            this.flags = String.Empty;
            base.name = name;
            this.order = order;
            this.preference = preference;
            this.regexp = String.Empty;
            this.replacement = replacement;
            this.services = String.Empty;
        }
    }
}

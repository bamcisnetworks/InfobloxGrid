using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:mx")]
    public class mx : BaseRecord
    {
        private string _dns_mail_exchanger;
        private string _mail_exchanger;
        private uint _preference;

        [ReadOnlyAttribute]
        public string dns_mail_exchanger
        {
            get
            {
                return this._dns_mail_exchanger;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_mail_exchanger", out this._dns_mail_exchanger, false, true);
            }
        }

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string mail_exchanger
        {
            get
            {
                return this._mail_exchanger;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "mail_exchanger", out this._mail_exchanger, false, true);
            }
        }

        [Required]
        [SearchableAttribute(LessThan = true, Equality = true, GreaterThan = true)]
        [Basic]
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
                    throw new ArgumentException(String.Format("The preference value must be between 0 and 65535 inclusive, {0} was provided.", value));
                }
            }
        }
  
        public mx(string mail_exchanger, string name, uint preference)
        {
            this.mail_exchanger = mail_exchanger;
            this.name = name;
            this.preference = preference;
        }
    }
}

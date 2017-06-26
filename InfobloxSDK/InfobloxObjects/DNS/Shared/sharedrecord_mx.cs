using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS.Shared
{
    [Name("sharedrecord:mx")]
    public class sharedrecord_mx : BaseSharedRecord
    {
        private string _dns_mail_exchanger;
        private string _mail_exchanger;
        private uint _preference;

        [ReadOnlyAttribute]
        [Basic]
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
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
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
                    throw new ArgumentException(String.Format("The preference value must be between 0 and 65535, {0} was provided."));
                }
            }
        }
        
        public sharedrecord_mx(string mail_exchanger, string name, uint preference, string shared_record_group) : base(shared_record_group)
        {
            this.mail_exchanger = mail_exchanger;
            base.name = name;
            this.preference = preference;
        }
    }
}

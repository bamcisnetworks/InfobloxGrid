using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:srv")]
    public class srv : BaseRecord
    {
        private string _dns_target;
        private uint _port;
        private uint _priority;
        private string _target;
        private uint _weight;

        [ReadOnlyAttribute]
        public string dns_target
        {
            get
            {
                return this._dns_target;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_target", out this._dns_target, false, true);
            }
        }
        [Required]
        [SearchableAttribute(Equality = true)]
        public uint port
        {
            get
            {
                return this._port;
            }
            set
            {
                if (value >= 0 && value <= 65535)
                {
                    this._port = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("The port value must be between 0 and 65535 inclusive, {0} was provided.", value));
                }
            }
        }
        [Required]
        [SearchableAttribute(LessThan = true, Equality = true, GreaterThan = true)]
        public uint priority
        {
            get
            {
                return this._priority;
            }
            set
            {
                if (value >= 0 && value <= 65535)
                {
                    this._priority = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("The priority value must be between 0 and 65535 inclusive, {0} was provided.", value));
                }
            }
        }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string target
        {
            get
            {
                return this._target;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "target", out this._target, false, true);
            }
        }
        [Required]
        [SearchableAttribute(LessThan = true, Equality = true, GreaterThan = true)]
        public uint weight
        {
            get
            {
                return this._weight;
            }
            set
            {
                if (value >= 0 && value <= 65535)
                {
                    this._weight = value;
                }
                else
                {
                    throw new ArgumentException(String.Format("The weight value must be between 0 and 65535 inclusive, {0} was provided.", value));
                }
            }
        }
        public srv(string name, uint port, uint priority, string target, uint weight)
        {
            base.name = name;
            this.port = port;
            this.priority = priority;
            this.target = target;
            this.weight = weight;
        }
    }
}

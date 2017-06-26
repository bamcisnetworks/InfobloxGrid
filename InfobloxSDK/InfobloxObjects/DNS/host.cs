using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:host")]
    public class host : BaseRecord
    {
        private List<string> _aliases;
        private string _device_description;
        private string _device_location;
        private string _device_type;
        private string _device_vendor;
        private List<string> _dns_aliases;
        private List<host_ipv4addr> _ipv4addrs;
        private List<host_ipv6addr> _ipv6addrs;
        private string _rrset_order;

        public string[] aliases
        {
            get
            {
                return this._aliases.ToArray();
            }
            set
            {
                this._aliases = new List<string>();

                foreach (string item in value)
                {
                    if (NetworkAddressTest.IsFqdn(item))
                    {
                        this._aliases.Add(item);
                    }
                    else
                    {
                        throw new ArgumentException("Each alias must be a valid FQDN.");
                    }
                }
            }
        }

        public bool allow_telnet { get; set; }

        public clicredential[] cli_credentials { get; set; }

        public bool configure_for_dns { get; set; }

        [SearchableAttribute(Equality = true, CaseInsensitive = true, Regex = true)]
        public string device_description
        {
            get
            {
                return this._device_description;
            }
            set
            {
                this._device_description = value.TrimValue();
            }
        }

        [SearchableAttribute(Equality = true, CaseInsensitive = true, Regex = true)]
        public string device_location
        {
            get
            {
                return this._device_location;
            }
            set
            {
                this._device_location = value.TrimValue();
            }
        }

        [SearchableAttribute(Equality = true, CaseInsensitive = true, Regex = true)]
        public string device_type
        {
            get
            {
                return this._device_type;
            }
            set
            {
                this._device_type = value.TrimValue();
            }
        }

        [SearchableAttribute(Equality = true, CaseInsensitive = true, Regex = true)]
        public string device_vendor
        {
            get
            {
                return this._device_vendor;
            }
            set
            {
                this._device_vendor = value.TrimValue();
            }
        }

        public bool disable_discovery { get; set; }

        public string[] dns_aliases
        {
            get
            {
                return this._dns_aliases.ToArray();
            }
            set
            {
                this._dns_aliases = new List<string>();
                foreach(string item in value)
                {
                    IdnMapping idn = new IdnMapping();
                    this._dns_aliases.Add(idn.GetAscii(item));
                }
            }
        }

        [NotReadableAttribute]
        public bool enable_immediate_discovery { get; set; }

        [Basic]
        public host_ipv4addr[] ipv4addrs
        {
            get
            {
                return this._ipv4addrs.ToArray();
            }
            set
            {
                this._ipv4addrs = new List<host_ipv4addr>();
                foreach (host_ipv4addr item in value)
                {
                    this._ipv4addrs.Add(item);
                }
            }
        }

        [Basic]
        public host_ipv6addr[] ipv6addrs
        {
            get
            {
                return this._ipv6addrs.ToArray();
            }
            set
            {
                this._ipv6addrs = new List<host_ipv6addr>();

                foreach (host_ipv6addr item in value)
                {
                    this._ipv6addrs.Add(item);
                }
            }
        }

        [NotReadableAttribute]
        public bool restart_if_needed { get; set; }

        public string rrset_order
        {
            get
            {
                return this._rrset_order;
            }
            set
            {
                string[] terms = new string[] { "cyclic", "random", "fixed" };
                if (terms.Contains(value.ToLower()))
                {
                    this._rrset_order = value.ToLower();
                }
                else
                {
                    throw new ArgumentException(String.Format("The rrset order must be one of the following: {0}", String.Join(", ", terms)));
                }
            }
        }

        public snmp3credential snmp3_credential { get; set; }

        public snmpcredential snmp_credential { get; set; }

        public bool use_cli_credentials { get; set; }

        public bool use_snmp3_credential { get; set; }

        public bool use_snmp_credential { get; set; }


        [JsonConstructor]
        private host(string name)
        {
            this.aliases = new string[0];
            this.allow_telnet = false;
            this.cli_credentials = new clicredential[0];
            this.configure_for_dns = true;
            this.device_description = String.Empty;
            this.device_location = String.Empty;
            this.device_type = String.Empty;
            this.device_type = String.Empty;
            this.disable_discovery = false;
            this.dns_aliases = new string[0];
            base.name = name;
            this.restart_if_needed = false;
            this.rrset_order = "cyclic";
            this.use_cli_credentials = false;
            this.use_snmp3_credential = false;
            this.use_snmp_credential = false;
        }

        public host(host_ipv4addr[] host_ipv4addrs, string name) : this(name)
        {
            this.ipv4addrs = host_ipv4addrs;
        }

        public host(host_ipv6addr[] host_ipv6addrs, string name) : this(name)
        {
            this.ipv6addrs = host_ipv6addrs;
        }
    }
}

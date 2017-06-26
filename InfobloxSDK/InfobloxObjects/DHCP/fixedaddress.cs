using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Net;
using System.Net.Sockets;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("fixedaddress")]
    public class fixedaddress : BaseFixedAddressObject
    {
        private string _agent_circuit_id;
        private string _agent_remote_id;
        private string _bootserver;
        private string _ddns_domainname;
        private string _ddns_hostname;
        private string _dhcp_client_identifier;
        private string _ipv4addr;
        private string _mac;
        private string _name;
        private string _network;
        private string _nextserver;
        private MatchClientEnum _match_client;

        public string agent_circuit_id
        {
            get
            {
                return this._agent_circuit_id;
            }
            set
            {
                this._agent_circuit_id = value.TrimValue();
            }
        }
        public string agemt_remote_id 
        {
            get
            {
                return this._agent_remote_id;
            }
            set
            {
                this._agent_remote_id = value.TrimValue();
            }
        }
        public bool always_update_dns { get; set; }
        public string bootfile { get; set; }
        public string bootserver 
        {
            get
            {
                return this._bootserver;
            }
            set
            {
                IPAddress ip;
                if (NetworkAddressTest.IsIPv4Address(value, out ip))
                {
                    this._bootserver = ip.ToString();
                }
                else if (NetworkAddressTest.IsFqdn(value))
                {
                    this._bootserver = value;
                }
                else
                {
                    throw new ArgumentException("The bootserver property must be a valid IPv4 address or FQDN.");
                }
            }
        }
        public bool client_identifier_prepend_zero { get; set; }
        public string ddns_domainname 
        {
            get
            {
                return this._ddns_domainname;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ddns_domainname", out this._ddns_domainname, true, true);
            }
        }
        public string ddns_hostname 
        {
            get
            {
                return this._ddns_hostname;
            }
            set
            {
                this._ddns_hostname = value.TrimValue();
            }
        }
        public bool deny_bootp { get; set; }
        public string dhcp_client_identifier 
        {
            get
            {
                return this._dhcp_client_identifier;
            }
            set
            {
                this._dhcp_client_identifier = value.TrimValue();
            }
        }
        public bool enable_ddns { get; set; }
        public bool ignore_dhcp_option_list_request { get; set; }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        [Basic]
        public string ipv4addr 
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this._ipv4addr = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The ipv4addr property must be a valid IPv4 address.");
                }
            }
        }
        [ReadOnlyAttribute]
        public bool is_invalid_mac { get; internal protected set; }
        [SearchableAttribute(Equality = true, Regex = true)]
        public string mac 
        {
            get
            {
                return this._mac;
            }
            set
            {
                if (NetworkAddressTest.IsMAC(value, out this._mac))
                { }
                else
                {
                    throw new ArgumentException("The mac address must be valid.");
                }
            }
        }
        [SearchableAttribute(Equality = true)]
        public MatchClientEnum match_client
        {
            get
            {
                return this._match_client;
            }
            set
            {
                switch (value)
                {
                    default:
                    case MatchClientEnum.MAC_ADDRESS:
                        if (!String.IsNullOrEmpty(this.mac))
                        {
                            this._match_client = value;
                        }
                        else
                        {
                            throw new ArgumentException("The match client cannot be set to MAC_ADDRESS unless a mac address is defined.");
                        }
                        break;
                    case MatchClientEnum.CIRCUIT_ID:
                        if (!String.IsNullOrEmpty(this.agent_circuit_id))
                        {
                            this._match_client = value;
                        }
                        else
                        {
                            throw new ArgumentException("The match client cannot be set to CIRCUIT_ID unless an agent circuit ID is defined.");
                        }
                        break;
                    case MatchClientEnum.CLIENT_ID:
                        if (!String.IsNullOrEmpty(this.dhcp_client_identifier))
                        {
                            this._match_client = value;
                        }
                        else
                        {
                            throw new ArgumentException("The match client cannot be set to CLIENT_ID unless a dhcp client identifier is defined.");
                        }
                        break;
                    case MatchClientEnum.REMOTE_ID:
                        if (!String.IsNullOrEmpty(this.agemt_remote_id))
                        {
                            this._match_client = value;
                        }
                        else
                        {
                            throw new ArgumentException("The match client cannot be set to REMOTE_ID unless an agent remote ID is defined.");
                        }
                        break;
                    case MatchClientEnum.RESERVED:
                        this.mac = "00:00:00:00:00:00:00";
                        this._match_client = value;
                        break;
                }
            }
        }
        public msdhcpoption[] ms_options { get; set; }
        [SearchableAttribute(Equality = true)]
        public msdhcpserver ms_server { get; set; }
        public override string name 
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValue();
            }
        }
        [SearchableAttribute(Equality = true, Regex = true)]
        public string network 
        {
            get
            {
                return this._network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv4Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The network property must be a valid IPv4/CIDR notation.");
                }
            }
        }
        public string nextserver 
        {
            get
            {
                return this._nextserver;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    this._nextserver = ip.ToString();
                }
                else if (NetworkAddressTest.IsFqdn(value))
                {
                    this._nextserver = value;
                }
                else
                {
                    throw new ArgumentException("The nextserver property must be a valid IPv4 address or FQDN.");
                }
            }
        }
        public uint pxe_lease_time { get; set; }
        public string reserved_interface { get; set; }
        public bool use_bootfile { get; set; }
        public bool use_bootserver { get; set; }
        public bool use_ddns_domainname { get; set; }
        public bool use_deny_bootp { get; set; }
        public bool use_enable_ddns { get; set; }
        public bool use_ignore_dhcp_option_list_request { get; set; }
        public bool use_nextserver { get; set; }

        public fixedaddress(string ipv4addr, string mac)
        {
            this.always_update_dns = false;
            this.bootfile = String.Empty;
            this.client_identifier_prepend_zero = false;
            this.ddns_hostname = String.Empty;
            this.deny_bootp = false;
            this.enable_ddns = false;
            this.ignore_dhcp_option_list_request = false;
            this.ipv4addr = ipv4addr;
            this.mac = mac;
            this.match_client = MatchClientEnum.MAC_ADDRESS;
            this.ms_options = new msdhcpoption[0];
            this.nextserver = String.Empty;
            this.reserved_interface = String.Empty;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_ddns_domainname = false;
            this.use_deny_bootp = false;
            this.use_enable_ddns = false;
            this.use_ignore_dhcp_option_list_request = false;
            this.use_nextserver = false;
        }
    }
}

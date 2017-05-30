using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:host_ipv4addr")]
    public class host_ipv4addr : BaseHostRecord
    {
        private string _bootfile;
        private string _bootserver;
        private string _ipv4addr;
        private DateTime _last_queried;
        private string _mac;
        private string _network;
        private string _nextserver;

        public string bootfile
        {
            get
            {
                return this._bootfile;
            }
            set
            {
                this._bootfile = value.TrimValue();
            }
        }
        public string bootserver
        {
            get
            {
                return this._bootserver;
            }
            set
            {
                NetworkAddressTest.IsIPv4AddressOrFQDN(value, "bootserver", out this._bootserver, true, true);
            }
        }
        public bool deny_bootp { get; set; }
        public bool enable_pxe_lease_time { get; set; }
        public bool ignore_client_requested_options { get; set; }
        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv4Address(value, out IP, false, true))
                {
                    this._ipv4addr = IP.ToString();
                }
            }
        }
        [ReadOnlyAttribute]
        public bool is_invalid_mac { get; internal protected set; }
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
        public MatchClientEnum match_client { get; set; }
        [ReadOnlyAttribute]
        public string network
        {
            get
            {
                return this._network;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPv4Cidr(value, out this._network, false, true);
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
                NetworkAddressTest.IsIPv4AddressOrFQDN(value, "nextserver", out this._nextserver, true, true);
            }
        }
        public uint pxe_lease_time { get; set; }
        public bool use_bootfile { get; set; }
        public bool use_bootserver { get; set; }
        public bool use_deny_bootp { get; set; }
        public bool use_ignore_client_requested_options { get; set; }
        public bool use_nextserver { get; set; }
        public bool use_pxe_lease_time { get; set; }

        public host_ipv4addr(string ipv4addr)
        {
            this.bootfile = String.Empty;
            this.bootserver = String.Empty;
            this.deny_bootp = false;
            this.enable_pxe_lease_time = false;
            this.ignore_client_requested_options = false;
            this.ipv4addr = ipv4addr;
            this.mac = String.Empty;
            this.match_client = MatchClientEnum.MAC_ADDRESS;
            this.nextserver = String.Empty;
            this.reserved_interface = String.Empty;
            this.use_bootfile = false;
            this.use_bootserver = false;
            this.use_deny_bootp = false;
            this.use_ignore_client_requested_options = false;
            this.use_nextserver = false;
            this.use_pxe_lease_time = false;
        }
    }
}

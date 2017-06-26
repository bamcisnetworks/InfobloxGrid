using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;
using System.Net;
using System.Net.Sockets;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:ptr")]
    public class ptr : BaseRecordWithDiscoveryData
    {
        private string _dns_ptrdname;
        private string _ipv4addr;
        private string _ipv6addr;
        private string _ptrdname;

        [ReadOnlyAttribute]
        public string dns_ptrdname
        {
            get
            {
                return this._dns_ptrdname;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_ptrdname", out this._dns_ptrdname, false, true);
            }
        }

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
                if (NetworkAddressTest.IsIPv4Address(value, out IP, true, true))
                {
                    this._ipv4addr = IP.ToString();
                }
            }
        }

        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6addr
        {
            get
            {
                return this._ipv6addr;
            }
            set
            {
                IPAddress IP;
                if (NetworkAddressTest.IsIPv6Address(value, out IP, true, true))
                {
                    this._ipv6addr = IP.ToString();
                }
            }
        }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public override string name
        {
            get
            {
                return base.name;
            }
            set
            {
                string temp = String.Empty;
                NetworkAddressTest.IsFqdn(value, "name", out temp, false, true);
                base.name = temp;
            }
        }

        [Required]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string ptrdname
        {
            get
            {
                return this._ptrdname;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "ptrdname", out this._ptrdname, false, true);
            }
        }

        public ptr(string ipaddress, string ptrdname)
        {
            IPAddress ip;
            if (IPAddress.TryParse(ipaddress, out ip))
            {
                switch (ip.AddressFamily)
                {
                    default:
                    case AddressFamily.InterNetwork:
                        this.ipv4addr = ip.ToString();
                        this.ipv6addr = String.Empty;
                        break;
                    case AddressFamily.InterNetworkV6:
                        this.ipv6addr = ip.ToString();
                        this.ipv4addr = String.Empty;
                        break;
                }

                this.ptrdname = ptrdname;
            }
        }

        public ptr(string ipaddress, string name, string ptrdname)
        {
            IPAddress ip;
            if (IPAddress.TryParse(ipaddress, out ip))
            {
                switch (ip.AddressFamily)
                {
                    default:
                    case AddressFamily.InterNetwork:
                        this.ipv4addr = ip.ToString();
                        this.ipv6addr = String.Empty;
                        break;
                    case AddressFamily.InterNetworkV6:
                        this.ipv6addr = ip.ToString();
                        this.ipv4addr = String.Empty;
                        break;
                }

                this.ptrdname = ptrdname;
                base.name = name;
            }
        }
    }
}

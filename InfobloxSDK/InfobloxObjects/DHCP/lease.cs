using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DHCP
{
    [Name("lease")]
    public class lease : RefObject
    {
        private string _address;
        private string _billing_class;
        private string _client_hostname;
        private DateTime _cltt;
        private DateTime _ends;
        private string _ipv6_duid;
        private string _ipv6_iaid;
        private string _on_commit;
        private string _on_expiry;
        private string _on_release;
        private string _option;
        private string _served_by;
        private string _server_host_name;
        private DateTime _starts;
        private DateTime _tsfp;
        private DateTime _tstp;
        private string _uid;
        private string _username;
        private string _variable;

        [ReadOnlyAttribute]
        [SearchableAttribute(Negative = true, LessThan = true, Equality = true, GreaterThan = true, Regex = true)]
        public string address 
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.isIPWithException(value, out this._address);
            }
        }
        [ReadOnlyAttribute]
        public string billing_class 
        {
            get
            {
                return this._billing_class;
            }
            internal protected set
            {
                this._billing_class = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public BindingStateEnum binding_state { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string client_hostname 
        {
            get
            {
                return this._client_hostname;
            }
            internal protected set
            {
                this._client_hostname = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public long cltt 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._cltt);
            }
            internal protected set
            {
                this._cltt = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(ContainsSearchable = true)]
        public discoverydata discovered_data { get; internal protected set; }
        [ReadOnlyAttribute]
        public long ends 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._ends);
            }
            internal protected set
            {
                this._ends = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string hardware { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string ipv6_duid 
        {
            get
            {
                return this._ipv6_duid;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPv6DUIDWithExceptionAllowEmpty(value, out this._ipv6_duid);
            }
        }
        [ReadOnlyAttribute]
        public string ipv6_iaid 
        {
            get
            {
                return this._ipv6_iaid;
            }
            internal protected set
            {
                this._ipv6_iaid = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public int ipv6_preferred_lifetime { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public uint ipv6_prefix_bits { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool is_invalid_mac { get; internal protected set; }
        [ReadOnlyAttribute]
        public string network { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string network_view { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool never_ends { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool never_starts { get; internal protected set; }
        [ReadOnlyAttribute]
        public BindingStateEnum next_binding_state { get; internal protected set; }
        [ReadOnlyAttribute]
        public string on_commit 
        {
            get
            {
                return this._on_commit;
            }
            internal protected set
            {
                this._on_commit = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public string on_expiry 
        {
            get
            {
                return this._on_expiry;
            }
            internal protected set
            {
                this._on_expiry = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public string on_release 
        {
            get
            {
                return this._on_release;
            }
            internal protected set
            {
                this._on_release = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public string option 
        { 
            get
            {
                return this._option;
            }
            internal protected set
            {
                this._option = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public IpProtocolEnum protocol { get; internal protected set; }
        [ReadOnlyAttribute]
        public string served_by 
        {
            get
            {
                return this._served_by;
            }
            internal protected set
            {
                NetworkAddressTest.isIPWithException(value, out this._served_by);
            }
        }
        [ReadOnlyAttribute]
        public string server_host_name 
        { 
            get
            {
                return this._server_host_name;
            }
            internal protected set
            {
                this._server_host_name = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public long starts 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._starts);
            }
            internal protected set
            {
                this._starts = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public long tsfp 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._tsfp);
            }
            internal protected set
            {
                this._tsfp = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public long tstp 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._tstp);
            }
            internal protected set
            {
                this._tstp = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public string uid 
        {
            get
            {
                return this._uid;
            }
            internal protected set
            {
                this._uid = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string username 
        {
            get
            {
                return this._username;
            }
            internal protected set
            {
                this._username = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public string variable 
        {
            get
            {
                return this._variable;
            }
            internal protected set
            {
                this._variable = value.TrimValue();
            }
        }
    }
}

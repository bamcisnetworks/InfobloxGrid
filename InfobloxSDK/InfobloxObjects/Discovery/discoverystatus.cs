using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    [Name("discovery:discoverystatus")]
    public class discoverystatus
    {
        private string _address;
        private DateTime _first_seen;
        private DateTime _last_seen;
        private DateTime _last_timestamp;

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true)]
        [Basic]
        public string address
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPAddress(value, out this._address, false, true);
            }
        }

        [ReadOnlyAttribute]
        public bool cli_collection_enabled { get; internal protected set; }

        [ReadOnlyAttribute]
        public statusinfo existence_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool fingerprint_enabled { get; internal protected set; }

        [ReadOnlyAttribute]
        public statusinfo fingerprint_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public long first_seen
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._first_seen);
            }
            internal protected set
            {
                this._first_seen = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [ReadOnlyAttribute]
        public string last_action { get; internal protected set; }

        [ReadOnlyAttribute]
        public long last_seen
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_seen);
            }
            internal protected set
            {
                this._last_seen = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [ReadOnlyAttribute]
        public long last_timestamp
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_timestamp);
            }
            internal protected set
            {
                this._last_timestamp = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true)]
        [Basic]
        public string name { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public string network_view { get; internal protected set; }

        [ReadOnlyAttribute]
        public statusinfo reachable_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool snmp_collection_enabled { get; internal protected set; }

        [ReadOnlyAttribute]
        public statusinfo snmp_collection_info { get; internal protected set; }

        [ReadOnlyAttribute]
        public snmpcredential snmp_credential_info { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public string status { get; internal protected set; }

        [ReadOnlyAttribute]
        public string type { get; internal protected set; }
    }
}

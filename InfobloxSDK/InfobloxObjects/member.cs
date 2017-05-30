using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Setting;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("member")]
    public class member : RefObject
    {
        private string _host_name;

        [ReadOnlyAttribute]
        public bool external_syslog_server_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string host_name 
        {
            get
            {
                return this._host_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "host_name", out this._host_name, true, true);
            }
        }
        [ReadOnlyAttribute]
        public nodeinfo node_info { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool remote_console_access_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        public snmp snmp_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool support_access_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        public string syslog_proxy_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public syslogserver[] syslog_servers { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint syslog_size { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool use_remote_console_access_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool use_snmp_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool use_support_access_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool use_syslog_proxy_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public network vip_setting { get; internal protected set; }
    }
}

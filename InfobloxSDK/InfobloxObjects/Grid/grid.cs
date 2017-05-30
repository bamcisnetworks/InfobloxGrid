using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Setting;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid
{
    [Name("grid")]
    public class grid : RefObject
    {
        [ReadOnlyAttribute]
        public bool audit_to_syslog_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool external_syslog_server_enable { get; internal protected set; }
        [ReadOnlyAttribute]
        public password password_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public securitybanner security_banner_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public security security_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public snmp snmp_setting { get; internal protected set; }
        [ReadOnlyAttribute]
        public SyslogFacilityEnum syslog_facility { get; internal protected set; }
        [ReadOnlyAttribute]
        public syslogserver[] syslog_servers { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint syslog_size { get; internal protected set; }
    }
}

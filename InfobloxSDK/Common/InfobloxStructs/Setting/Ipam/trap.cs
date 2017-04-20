
namespace BAMCIS.Infoblox.Common.InfobloxStructs.Setting.Ipam
{
    public class trap
    {
        public bool enable_email_warnings { get; set; }
        public bool enable_snmp_warnings { get; set; }

        public trap()
        {
            this.enable_email_warnings = false;
            this.enable_snmp_warnings = false;
        }
    }
}

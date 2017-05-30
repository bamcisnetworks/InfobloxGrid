
namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting
{
    public class snmp
    {
        public string[] engine_id { get; set; }
        public string queries_community_string { get; set; }
        public bool queries_enable { get; set; }
        public bool snmpv3_queries_enable { get; set; }
        public queriesuser snmpv3_queries_users { get; set; }
        public bool snmpv3_traps_enable { get; set; }
        public string[] syscontact { get; set; }
        public string[] sysdescr { get; set; }
        public string[] syslocation { get; set; }
        public string[] sysname { get; set; }
        public trapreceiver[] trap_receivers { get; set; }
        public bool traps_community_string { get; set; }
        public bool traps_enable { get; set; }
    }
}

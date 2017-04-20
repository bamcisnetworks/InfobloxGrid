using System.ComponentModel;

namespace BAMCIS.Infoblox.Common.Enums
{
    public enum ActionEnum
    {
        [Description("Add")]
        ADD,
        [Description("Configure Grid")]
        CONFIGURE_GRID,
        [Description("Convert IPv4 Lease")]
        CONVERT_IPV4_LEASE,
        [Description("Convert IPv6 Lease")]
        CONVERT_IPV6_LEASE,
        [Description("Delete")]
        DELETE,
        [Description("Lock/Unlock Zone")]
        LOCK_UNLOCK_ZONE,
        [Description("Modify")]
        MODIFY,
        [Description("Network Discovery")]
        NETWORK_DISCOVERY,
        [Description("Reset Grid")]
        RESET_GRID,
        [Description("Restart Services")]
        RESTART_SERVICES,
        [Description("Upgrade Grid")]
        UPGRADE_GRID
    }
}

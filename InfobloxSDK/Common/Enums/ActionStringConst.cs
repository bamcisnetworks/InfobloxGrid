using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Common.Enums
{
    public class ActionStringConst
    {
        public const string ADD = "Add";
        public const string CONFIGURE_GRID = "Configure Grid";
        public const string CONVERT_IPV4_LEASE = "Convert IPv4 Lease";
        public const string CONVERT_IPV6_LEASE = "Convert IPv6 Lease";
        public const string DELETE = "Delete";
        public const string LOCK_UNLOCK_ZONE = "Lock/Unlock Zone";
        public const string MODIFY = "Modify";
        public const string NETWORK_DISCOVERY = "Network_Discovery";
        public const string RESET_GRID = "Reset Grid";
        public const string RESTART_SERVICES = "Restart Services";
        public const string UPDATE_GRID = "Update Grid";

        public static bool Contains(string value)
        {
            return typeof(ActionStringConst).GetTypeInfo().GetFields().Where(x => x.IsLiteral && !x.IsInitOnly && x.IsStatic)
                 .Select(x => (string)x.GetValue(x)).Contains(value);
        }
    }
}

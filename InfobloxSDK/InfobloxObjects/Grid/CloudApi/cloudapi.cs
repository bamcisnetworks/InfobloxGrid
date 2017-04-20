using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs.Grid.CloudApi;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi
{
    [Name("grid:cloudapi")]
    public class cloudapi : RefObject
    {
        public AllowApiAdminsEnum allow_api_admins { get; set; }
        public user[] allowed_api_admins { get; set; }
        public bool enable_recycle_bin { get; set; }
    }
}

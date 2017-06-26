using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi
{
    [Name("grid:cloudapi")]
    public class cloudapi : RefObject
    {
        [Basic]
        public AllowApiAdminsEnum allow_api_admins { get; set; }

        [Basic]
        public user[] allowed_api_admins { get; set; }

        [Basic]
        public bool enable_recycle_bin { get; set; }
    }
}

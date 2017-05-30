using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi
{
    [Name("grid:member:cloudapi")]
    public class cloudapimember : RefObject
    {
        public AllowApiAdminsEnum allow_api_admins { get; set; }
        public user[] allowed_api_admins { get; set; }
        public bool enable_service { get; set; }
        [ReadOnlyAttribute]
        public dhcpmember member { get; internal protected set; }
        [ReadOnlyAttribute]
        public CloudApiStatusEnum status { get; internal protected set; }

        public cloudapimember()
        {
            this.allow_api_admins = AllowApiAdminsEnum.ALL;
            this.allowed_api_admins = new user[0];
        }
    }
}

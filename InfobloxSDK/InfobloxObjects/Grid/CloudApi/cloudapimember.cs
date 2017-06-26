using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi
{
    [Name("grid:member:cloudapi")]
    public class cloudapimember : RefObject
    {
        [Basic]
        public AllowApiAdminsEnum allow_api_admins { get; set; }

        [Basic]
        public user[] allowed_api_admins { get; set; }

        [Basic]
        public bool enable_service { get; set; }

        [ReadOnlyAttribute]
        [Basic]
        public dhcpmember member { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public CloudApiStatusEnum status { get; internal protected set; }

        public cloudapimember()
        {
            this.allow_api_admins = AllowApiAdminsEnum.ALL;
            this.allowed_api_admins = new user[0];
        }
    }
}

using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid
{
    [Name("grid:dns")]
    public class dns : RefObject
    {
        public loggingcategories logging_categories { get; set; }

        public dns()
        {
            this.logging_categories = new loggingcategories() { log_dtc_gslb = false, log_dtc_health = false};
        }
    }
}

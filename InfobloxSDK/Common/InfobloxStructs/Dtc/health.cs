using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Dtc
{
    public class health
    {
        public DtcAvailabilityEnum availability { get; set; }
        public string description { get; set; }
        public DtcEnabledStateEnum enabled_state { get; set; }


        public health()
        {
            this.availability = DtcAvailabilityEnum.NONE;
            this.description = String.Empty;
            this.enabled_state = DtcEnabledStateEnum.NONE;
        }
    }
}

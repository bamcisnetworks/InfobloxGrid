using BAMCIS.Infoblox.Common.InfobloxStructs.Setting;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Properties
{
    public class blackoutsetting
    {
        public uint blackout_duration { get; set; }
        public schedule blackout_schedule { get; set; }
        public bool enable_blackout { get; set; }
    }
}

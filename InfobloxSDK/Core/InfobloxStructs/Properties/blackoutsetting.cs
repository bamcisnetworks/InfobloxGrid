using BAMCIS.Infoblox.Core.InfobloxStructs.Setting;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Properties
{
    public class blackoutsetting
    {
        public uint blackout_duration { get; set; }
        public schedule blackout_schedule { get; set; }
        public bool enable_blackout { get; set; }
    }
}

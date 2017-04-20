
namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class memberserver
    {
        public bool enable_preferred_primaries { get; set; }
        public bool grid_replicate { get; set; }
        public bool lead { get; set; }
        public string name { get; set; }
        public extserver[] preferred_primaries { get; set; }
        public bool stealth { get; set; }
    }
}


namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting.Ipam
{
    public class threshold
    {
        public uint reset_value { get; set; }
        public uint trigger_value { get; set; }

        public threshold()
        {
            this.reset_value = 85;
            this.trigger_value = 95;
        }
    }
}

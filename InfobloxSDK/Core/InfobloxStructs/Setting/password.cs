
namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting
{
    public class password
    {
        public uint chars_to_change { get; set; }
        public uint expire_days { get; set; }
        public bool expire_enable { get; set; }
        public bool force_reset_enable { get; set; }
        public uint num_lower_char { get; set; }
        public uint num_numeric_char { get; set; }
        public uint num_symbol_char { get; set; }
        public uint num_upper_char { get; set; }
        public uint password_min_length { get; set; }
        public uint reminder_days { get; set; }
    }
}

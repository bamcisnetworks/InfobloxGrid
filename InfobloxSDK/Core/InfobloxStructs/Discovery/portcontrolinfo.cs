using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class portcontrolinfo
    {
        private string _description;

        [NotReadableAttribute]
        public UpDownEnum admin_status { get; set; }
        [NotReadableAttribute]
        public vlaninfo data_vlan_info { get; set; }
        [NotReadableAttribute]
        public string description
        {
            get 
            {
                return this._description;
            }
            set
            {
                this._description = value.TrimValue();
            }
        }
        [NotReadableAttribute]
        public string device { get; set; }
        [NotReadableAttribute]
        public string @interface { get; set; }
        [NotReadableAttribute]
        public string parent { get; set; }
        [NotReadableAttribute]
        public vlaninfo voice_vlan_info { get; set; }
    }
}

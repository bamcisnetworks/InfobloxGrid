using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Setting
{
    public class securitybanner
    {
        private string _message;

        public ColorEnum color { get; set; }
        public bool enable { get; set; }
        public ClassificationLevelEnum level { get; set; }
        public string message 
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value.TrimValue();
            }
        }
    }
}

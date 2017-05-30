using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting
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

using BAMCIS.Infoblox.Common.Enums;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Discovery
{
    public class jobprocessdetails
    {
        private string _stream;

        public uint end_line { get; set; }
        public DiscoveryJobProcessStatusEnum status { get; set; }
        public string stream
        {
            get
            {
                return this._stream;
            }
            set
            {
                this._stream = value.TrimValue();
            }
        }
    }
}

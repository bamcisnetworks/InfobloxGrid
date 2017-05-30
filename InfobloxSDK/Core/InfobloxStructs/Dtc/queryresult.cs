using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Dtc
{
    public class queryresult
    {
        public string rdata { get; set; }
        public uint ttl { get; set; }
        public DtcPoolLinkRecordTypeEnum type { get; set; }
        public bool use_ttl { get; set; }
    }
}

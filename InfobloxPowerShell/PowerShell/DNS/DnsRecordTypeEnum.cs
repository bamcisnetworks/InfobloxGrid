using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.InfobloxObjects.DNS;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public enum DnsRecordTypeEnum
    {
        [ObjectInfo(Name = "record:a", ObjectType = typeof(a))]
        A,
        [ObjectInfo(Name = "record:aaaa", ObjectType = typeof(aaaa))]
        AAAA,
        [ObjectInfo(Name = "record:cname", ObjectType = typeof(cname))]
        CNAME,
        [ObjectInfo(Name = "record:dtclbdn", ObjectType = typeof(dtclbdn))]
        DTCLBDN,
        [ObjectInfo(Name = "record:mx", ObjectType = typeof(mx))]
        MX,
        [ObjectInfo(Name = "record:naptr", ObjectType = typeof(naptr))]
        NAPTR,
        [ObjectInfo(Name = "record:ptr", ObjectType = typeof(ptr))]
        PTR,
        [ObjectInfo(Name = "record:srv", ObjectType = typeof(srv))]
        SRV,
        [ObjectInfo(Name = "record:txt", ObjectType = typeof(txt))]
        TXT
    }
}

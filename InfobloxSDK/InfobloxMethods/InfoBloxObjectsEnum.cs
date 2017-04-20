using BAMCIS.Infoblox.InfobloxObjects;
using BAMCIS.Infoblox.InfobloxObjects.ADSites;
using BAMCIS.Infoblox.InfobloxObjects.DHCP;
using BAMCIS.Infoblox.InfobloxObjects.Discovery;
using BAMCIS.Infoblox.InfobloxObjects.DNS;
using BAMCIS.Infoblox.InfobloxObjects.DNS.Shared;
using BAMCIS.Infoblox.InfobloxObjects.Dtc;
using BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor;
using BAMCIS.Infoblox.InfobloxObjects.Dtc.Topology;
using BAMCIS.Infoblox.InfobloxObjects.Grid;
using BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi;
using BAMCIS.Infoblox.InfobloxObjects.IPAM;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public enum InfoBloxObjectsEnum
    {
        [ObjectInfo(Name = "All", ObjectType = null, SupportsGlobalSearch = true)]
        ALL,
        [ObjectInfo(Name = "AllNetwork", ObjectType = null, SupportsGlobalSearch = true)]
        ALL_NETWORK,
        [ObjectInfo(Name = "AllZone", ObjectType = null, SupportsGlobalSearch = true)]
        ALL_ZONE,
        [ObjectInfo(Name = "IPAMObjects", ObjectType = null, SupportsGlobalSearch = true)]
        IPAM_OBJECTS,
        [ObjectInfo(Name = "admingroup", ObjectType = typeof(admingroup), SupportsGlobalSearch = true)]
        ADMIN_GROUP,
        [ObjectInfo(Name = "adminuser", ObjectType = typeof(adminuser), SupportsGlobalSearch = true)]
        ADMIN_USER,
        [ObjectInfo(Name = "allrecords", ObjectType = typeof(allrecords), SupportsGlobalSearch = false)]
        ALL_RECORDS,
        [ObjectInfo(Name = "discovery:device", ObjectType = typeof(device), SupportsGlobalSearch = true)]
        DISCOVERY_DEVICE,
        [ObjectInfo(Name = "discovery:devicecomponent", ObjectType = typeof(devicecomponent), SupportsGlobalSearch = false)]
        DISCOVERY_DEVICE_COMPONENT,
        [ObjectInfo(Name = "disovery:deviceinterface", ObjectType = typeof(deviceinterface), SupportsGlobalSearch = true)]
        DISCOVERY_DEVICE_INTERFACE,
        [ObjectInfo(Name = "discovery:deviceneighbor", ObjectType = typeof(deviceneighbor), SupportsGlobalSearch = false)]
        DISCOVERY_DEVICE_NEIGHBOR,
        [ObjectInfo(Name = "dtc:certificate", ObjectType = typeof(dtccertificate), SupportsGlobalSearch = false)]
        DTC_CERTIFICATE,
        [ObjectInfo(Name = "dtc:lbdn", ObjectType = typeof(lbdn), SupportsGlobalSearch = true)]
        DTC_LBDN,
        [ObjectInfo(Name = "dtc:monitor", ObjectType = typeof(dtcmonitor), SupportsGlobalSearch = false)]
        DTC_MONITOR,
        [ObjectInfo(Name = "dtc:monitor:http", ObjectType = typeof(httpmonitor), SupportsGlobalSearch = true)]
        DTC_MONITOR_HTTP,
        [ObjectInfo(Name = "dtc:monitor:icmp", ObjectType = typeof(icmpmonitor), SupportsGlobalSearch = true)]
        DTC_MONITOR_ICMP,
        [ObjectInfo(Name = "dtc:monitor:pdp", ObjectType = typeof(pdpmonitor), SupportsGlobalSearch = true)]
        DTC_MONITOR_PDP,
        [ObjectInfo(Name = "dtc:monitor:sip", ObjectType = typeof(sipmonitor), SupportsGlobalSearch = true)]
        DTC_MONITOR_SIP,
        [ObjectInfo(Name = "dtc:monitor:tcp", ObjectType = typeof(tcpmonitor), SupportsGlobalSearch = true)]
        DTC_MONITOR_TCP,
        [ObjectInfo(Name = "dtc:object", ObjectType = typeof(dtcobject), SupportsGlobalSearch = false)]
        DTC_OBJECT,
        [ObjectInfo(Name = "dtc:pool", ObjectType = typeof(pool), SupportsGlobalSearch = true)]
        DTC_POOL,
        [ObjectInfo(Name = "dtc:server", ObjectType = typeof(dtcserver), SupportsGlobalSearch = true)]
        DTC_SERVER,
        [ObjectInfo(Name = "dtc:topology", ObjectType = typeof(dtctopology), SupportsGlobalSearch = true)]
        DTC_TOPOLOGY,
        [ObjectInfo(Name = "dtc:topology:label", ObjectType = typeof(dtctopologylabel), SupportsGlobalSearch = true)]
        DTC_TOPOLOGY_LABEL,
        [ObjectInfo(Name = "dtc:topology:rule", ObjectType = typeof(rule), SupportsGlobalSearch = false)]
        DTC_TOPOLOGY_RULE,
        [ObjectInfo(Name = "fixedaddress", ObjectType = typeof(fixedaddress), SupportsGlobalSearch = true)]
        FIXED_ADDRESS,
        [ObjectInfo(Name = "grid", ObjectType = typeof(grid), SupportsGlobalSearch = false)]
        GRID,
        [ObjectInfo(Name = "grid:cloudapi", ObjectType = typeof(cloudapi), SupportsGlobalSearch = false)]
        GRID_CLOUD_API,
        [ObjectInfo(Name = "grid:cloudapi:cloudstatistics", ObjectType = typeof(cloudstatistics), SupportsGlobalSearch = false)]
        GRID_CLOUD_API_CLOUD_STATISTICS,
        [ObjectInfo(Name = "grid:cloudapi:tenant", ObjectType = typeof(tenant), SupportsGlobalSearch = false)]
        GRID_CLOUD_API_TENANT,
        [ObjectInfo(Name = "grid:cloudapi:vmaddress", ObjectType = typeof(vmaddress), SupportsGlobalSearch = false)]
        GRID_CLOUD_API_VM_ADDRESS,
        [ObjectInfo(Name = "grid:dhcpproperties", ObjectType = typeof(InfobloxObjects.Grid.dhcpproperties), SupportsGlobalSearch = true)]
        GRID_DHCP_PROPERTIES,
        [ObjectInfo(Name = "grid:dns", ObjectType = typeof(dns), SupportsGlobalSearch = true)]
        GRID_DNS,
        [ObjectInfo(Name = "grid:maxminddbinfo", ObjectType = typeof(maxminddbinfo), SupportsGlobalSearch = false)]
        GRID_MAX_MIND_DB_INFO,
        [ObjectInfo(Name = "grid:member:cloudapi", ObjectType = typeof(cloudapimember), SupportsGlobalSearch = true)]
        GRID_MEMBER_CLOUD_API,
        [ObjectInfo(Name = "grid:x509certificate", ObjectType = typeof(x509certificate), SupportsGlobalSearch = true)]
        GRID_X509_CERTIFICATE,
        [ObjectInfo(Name = "ipv4address", ObjectType = typeof(ipv4addr), SupportsGlobalSearch = false)]
        IPV4_ADDRESS,
        [ObjectInfo(Name = "ipv6address", ObjectType = typeof(ipv6addr), SupportsGlobalSearch = false)]
        IPV6_ADDRESS,
        [ObjectInfo(Name = "ipv6fixedaddress", ObjectType = typeof(ipv6fixedaddress), SupportsGlobalSearch = true)]
        IPV6_FIXED_ADDRESS,
        [ObjectInfo(Name = "ipv6network", ObjectType = typeof(ipv6network), SupportsGlobalSearch = true)]
        IPV6_NETWORK,
        [ObjectInfo(Name = "ipv6networkcontainer", ObjectType = typeof(ipv6networkcontainer), SupportsGlobalSearch = true)]
        IPV6_NETWORK_CONTAINER,
        [ObjectInfo(Name = "ipv6networktemplate", ObjectType = typeof(ipv6networktemplate), SupportsGlobalSearch = true)]
        IPV6_NETWORK_TEMPLATE,
        [ObjectInfo(Name = "ipv6range", ObjectType = typeof(InfobloxObjects.DHCP.ipv6range), SupportsGlobalSearch = true)]
        IPV6_RANGE,
        [ObjectInfo(Name = "ipv6sharednetwork", ObjectType = typeof(ipv6sharednetwork), SupportsGlobalSearch = true)]
        IPV6_SHARED_NETWORK,
        [ObjectInfo(Name = "lease", ObjectType = typeof(lease), SupportsGlobalSearch = true)]
        LEASE,
        [ObjectInfo(Name = "macaddressfilter", ObjectType = typeof(macaddressfilter), SupportsGlobalSearch = true)]
        MAC_ADDRESS_FILTER,
        [ObjectInfo(Name = "member", ObjectType = typeof(member), SupportsGlobalSearch = true)]
        MEMBER,
        [ObjectInfo(Name = "member:dhcpproperties", ObjectType = typeof(InfobloxObjects.dhcpproperties), SupportsGlobalSearch = true)]
        MEMBER_DHCP_PROPERTIES,
        [ObjectInfo(Name = "member:dns", ObjectType = typeof(dnsmember), SupportsGlobalSearch = true)]
        MEMBER_DNS,
        [ObjectInfo(Name = "msserver:adsites:domain", ObjectType = typeof(domain), SupportsGlobalSearch = true)]
        MS_SERVER_AD_SITES_DOMAIN,
        [ObjectInfo(Name = "msserver:adsites:site", ObjectType = typeof(site), SupportsGlobalSearch = true)]
        MS_SERVER_AD_SITES_SITE,
        [ObjectInfo(Name = "namedacl", ObjectType = typeof(namedacl), SupportsGlobalSearch = true)]
        NAMED_ACL,
        [ObjectInfo(Name = "network", ObjectType = typeof(ipv4network), SupportsGlobalSearch = true)]
        NETWORK,
        [ObjectInfo(Name = "networkcontainer", ObjectType = typeof(networkcontainer), SupportsGlobalSearch = true)]
        NETWORK_CONTAINER,
        [ObjectInfo(Name = "networktemplate", ObjectType = typeof(networktemplate), SupportsGlobalSearch = true)]
        NETWORK_TEMPLATE,
        [ObjectInfo(Name = "networkview", ObjectType = typeof(networkview), SupportsGlobalSearch = true)]
        NETWORK_VIEW,
        [ObjectInfo(Name = "permission", ObjectType = typeof(ibxpermission), SupportsGlobalSearch = false)]
        PERMISSION,
        [ObjectInfo(Name = "range", ObjectType = typeof(ipv4range), SupportsGlobalSearch = true)]
        RANGE,
        [ObjectInfo(Name = "record:a", ObjectType = typeof(a), SupportsGlobalSearch = true)]
        RECORD_A,
        [ObjectInfo(Name = "record:aaaa", ObjectType = typeof(aaaa), SupportsGlobalSearch = true)]
        RECORD_AAAA,
        [ObjectInfo(Name = "record:cname", ObjectType = typeof(cname), SupportsGlobalSearch = true)]
        RECORD_CNAME,
        [ObjectInfo(Name = "record:dtclbdn", ObjectType = typeof(dtclbdn), SupportsGlobalSearch = false)]
        RECORD_DTCLBDN,
        [ObjectInfo(Name = "record:host", ObjectType = typeof(host), SupportsGlobalSearch = true)]
        RECORD_HOST,
        [ObjectInfo(Name = "record:host_ipv4addr", ObjectType = typeof(host_ipv4addr), SupportsGlobalSearch = true)]
        RECORD_HOST_IPV4ADDR,
        [ObjectInfo(Name = "record:host_ipv6addr", ObjectType = typeof(host_ipv6addr), SupportsGlobalSearch = true)]
        RECORD_HOST_IPV6ADDR,
        [ObjectInfo(Name = "record:mx", ObjectType = typeof(mx), SupportsGlobalSearch = true)]
        RECORD_MX,
        [ObjectInfo(Name = "record:naptr", ObjectType = typeof(naptr), SupportsGlobalSearch = true)]
        RECORD_NAPTR,
        [ObjectInfo(Name = "record:ptr", ObjectType = typeof(ptr), SupportsGlobalSearch = true)]
        RECORD_PTR,
        [ObjectInfo(Name = "record:srv", ObjectType = typeof(srv), SupportsGlobalSearch = true)]
        RECORD_SRV,
        [ObjectInfo(Name = "record:txt", ObjectType = typeof(txt), SupportsGlobalSearch = true)]
        RECORD_TXT,
        [ObjectInfo(Name = "roaminghost", ObjectType = typeof(roaminghost), SupportsGlobalSearch = true)]
        ROAMING_HOST,
        [ObjectInfo(Name = "scheduledtask", ObjectType = typeof(scheduledtask), SupportsGlobalSearch = false)]
        SCHEDULED_TASK,
        [ObjectInfo(Name = "sharednetwork", ObjectType = typeof(sharednetwork), SupportsGlobalSearch = true)]
        SHARED_NETWORK,
        [ObjectInfo(Name = "sharedrecord:a", ObjectType = typeof(sharedrecord_a), SupportsGlobalSearch = true)]
        SHARED_RECORD_A,
        [ObjectInfo(Name = "sharedrecord:aaaa", ObjectType = typeof(sharedrecord_aaaa), SupportsGlobalSearch = true)]
        SHARED_RECORD_AAAA,
        [ObjectInfo(Name = "sharedrecord:mx", ObjectType = typeof(sharedrecord_mx), SupportsGlobalSearch = true)]
        SHARED_RECORD_MX,
        [ObjectInfo(Name = "sharedrecord:srv", ObjectType = typeof(sharedrecord_srv), SupportsGlobalSearch = true)]
        SHARED_RECORD_SRV,
        [ObjectInfo(Name = "sharedrecord:txt", ObjectType = typeof(sharedrecord_txt), SupportsGlobalSearch = true)]
        SHARED_RECORD_TXT,
        [ObjectInfo(Name = "snmpuser", ObjectType = typeof(snmpuser), SupportsGlobalSearch = false)]
        SNMP_USER,
        [ObjectInfo(Name = "view", ObjectType = typeof(view), SupportsGlobalSearch = true)]
        VIEW,
        [ObjectInfo(Name = "zone_auth", ObjectType = typeof(zone_auth), SupportsGlobalSearch = false)]
        ZONE_AUTH,
        [ObjectInfo(Name = "zone_auth_discrepancy", ObjectType = typeof(zone_auth_discrepancy), SupportsGlobalSearch = false)]
        ZONE_AUTH_DISCREPANCY,
        [ObjectInfo(Name = "zone_delegated", ObjectType = typeof(zone_delegated), SupportsGlobalSearch = false)]
        ZONE_DELEGATED,
        [ObjectInfo(Name = "zone_forward", ObjectType = typeof(zone_forward), SupportsGlobalSearch = false)]
        ZONE_FORWARD,
        [ObjectInfo(Name = "zone_stub", ObjectType = typeof(zone_stub), SupportsGlobalSearch = false)]
        ZONE_STUB
    }
}

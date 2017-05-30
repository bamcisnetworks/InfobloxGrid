using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.InfobloxObjects.DNS;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using System.Threading.Tasks;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public class DnsMethods : IBXCommonMethods
    {
        public DnsMethods(string gridMaster, string apiVersion, string username, SecureString password, TimeSpan? timeout = null) : base(gridMaster, apiVersion, username, password, timeout) {}

        public DnsMethods(InfobloxSession session, TimeSpan? timeout = null) : base(session, timeout) { }

        public DnsMethods(TimeSpan? timeout = null) : base() { }

        #region Host Records

        public async Task<string> NewDnsHostRecord(string hostName, string ipAddress, bool addToDns = true, bool enableDhcp = false, bool setHostName = false, string mac = "")
        {
            IPAddress IP;
            string FQDN = String.Empty;
            string MACAddress = String.Empty;

            if (NetworkAddressTest.IsIPv4Address(ipAddress, out IP, false, true) && NetworkAddressTest.IsMAC(mac, out MACAddress, true, true) &&
                NetworkAddressTest.IsFqdn(hostName, "hostname", out FQDN, false, true))
            { 
                string Data = "{\"name\":\"" + hostName + "\",\"ipv4addrs\":[{\"ipv4addr\":\"" + IP.ToString() + "\"" +
                    ((!String.IsNullOrEmpty(MACAddress)) ? ",\"mac\":\"" + MACAddress + "\"" : "") +
                    ",\"configure_for_dhcp\":" + ((enableDhcp) ? "true" : "false") +
                    ((setHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + FQDN + "\"}]" : "") +
                    "}],\"configure_for_dns\":" + addToDns.ToString().ToLower() + "}";

                return await base.NewIbxObject(typeof(host), Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> NewDnsHostRecordWithNextAvailableIP(string hostName, string network, bool addToDns = true, bool enableDhcp = false, bool setHostName = false, string mac = "")
        {
            string FormattedMAC = String.Empty;
            string FormattedFQDN = String.Empty;

            if (NetworkAddressTest.IsMAC(mac, out FormattedMAC, true, true) && NetworkAddressTest.IsFqdn(hostName, "hostname", out FormattedFQDN, false, true))
            {
                string Data = "{\"name\":\"" + FormattedFQDN + "\",\"ipv4addrs\":[{\"ipv4addr\":\"func:nextavailableip:" + network + "\"" +
                    ((!String.IsNullOrEmpty(FormattedMAC)) ? ",\"mac\":\"" + FormattedMAC + "\"" : "") +
                    ",\"configure_for_dhcp\":" + ((enableDhcp) ? "true" : "false") +
                    ((setHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + FormattedFQDN + "\"}]" : "") + "}]" +
                    ",\"configure_for_dns\":" + addToDns.ToString().ToLower() + "}";

                return await base.NewIbxObject(typeof(host), Data);
            }
            else
            {
                return null;
            }
        }

        public async Task<string> SetDnsHostRecordIP(string reference, string ipAddress, bool enableDhcp = false, bool setHostName = false, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                if (!String.IsNullOrEmpty(ipAddress))
                {
                    IPAddress IP;
                    string MACAddress = String.Empty;

                    if (NetworkAddressTest.IsIPv4Address(ipAddress, out IP, false, true) && NetworkAddressTest.IsMAC(mac, out MACAddress, true, true))
                    {
                        string HostName = String.Empty;

                        if (setHostName)
                        {
                            host Host = await GetIbxObject<host>(reference, new string[] { "ALL" });
                            HostName = Host.name;
                        }

                        string Data = "{\"ipv4addrs\":[{\"ipv4addr\":\"" + IP.ToString() + "\"" +
                            ",\"configure_for_dhcp\":" + ((enableDhcp) ? "true" : "false") +
                            ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + mac + "\"" : "") +
                            ((setHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + HostName + "\"}]" : "") +
                            "}]}";

                        return await base.UpdateIbxObject<host>(reference, Data);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new ArgumentNullException("ipAddress", "The DNS host record IP address cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public async Task<string> SetDnsHostRecordName(string reference, string hostName)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                string FQDN = String.Empty;

                if (NetworkAddressTest.IsFqdn(hostName, "hostname", out FQDN, false, true))
                {
                    string Data = $"{{\"name\":\"{FQDN}\"}}";

                    return await base.UpdateIbxObject<host>(reference, Data);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public async Task<string> SetDnsHostRecordNextAvailableIP(string reference, string network, bool enableDhcp = false, bool setHostName = false, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                string MACAddress = String.Empty;
                string TempNetwork = String.Empty;

                if (NetworkAddressTest.IsMAC(mac, out MACAddress, true, true) && NetworkAddressTest.IsIPv4Cidr(network, out TempNetwork, false, true))
                {
                    string HostName = String.Empty;

                    if (setHostName)
                    {
                        host Host = await GetIbxObject<host>(reference, new string[] { "ALL" });
                        HostName = Host.name;
                    }

                    string Data = "{\"ipv4addrs\":[{\"ipv4addr\":\"func:nextavailableip:" + TempNetwork + "\"" +
                        ",\"configure_for_dhcp\":" + ((enableDhcp) ? "true" : "false") +
                        ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + MACAddress + "\"" : "") +
                        ((setHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + HostName + "\"}]" : "") +
                        "}]}";

                    return await base.UpdateIbxObject<host>(reference, Data);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public async Task<string> AddDnsHostRecordIPAddresses(string reference, string[] ipAddresses)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                if (ipAddresses != null && ipAddresses.Length > 0)
                {
                    List<string> IPs = new List<string>();

                    foreach (string Address in ipAddresses)
                    {
                        IPAddress IP;

                        if (NetworkAddressTest.IsIPv4Address(Address, out IP, false, true))
                        {
                            IPs.Add($"{{\"ipv4addr\":\"{IP.ToString()}\"}}");
                        }
                    }

                    string Data = "{\"ipv4addrs+\":[" + String.Join(",", IPs) + "]}";

                    return await base.UpdateIbxObject<host>(reference, Data);
                }
                else
                {
                    throw new ArgumentException("The IP addresses cannot be null or empty", "ipAddresses");
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public async Task<string> AddDnsHostRecordIPAddress(string reference, string ipAddress, bool enableDhcp = false, bool setHostName = false, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                if (!String.IsNullOrEmpty(ipAddress))
                {
                    IPAddress IP = IPAddress.Parse(ipAddress);

                    string HostName = String.Empty;

                    if (setHostName)
                    {
                        host Host = await base.GetIbxObject<host>(reference, new string[] { "ALL" });
                        HostName = Host.name;
                    }
                
                    string Data = "{\"ipv4addrs+\":[{\"ipv4addr\":\"" + IP.ToString() + "\"" +
                           ",\"configure_for_dhcp\":" + ((enableDhcp) ? "true" : "false") +
                           ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + mac + "\"" : "") +
                           ((setHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + HostName + "\"}]" : "") +
                           "}]}";

                    return await base.UpdateIbxObject<host>(reference, Data);
                }
                else
                {
                    throw new ArgumentNullException("ipAddress", "The IP address to add cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public async Task<string> AddDnsHostRecordNextAvailableIPAddress(string reference, string network, bool enableDhcp = false, bool setHostName = false, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                if (!String.IsNullOrEmpty(network))
                {
                    string HostName = String.Empty;

                    if (setHostName)
                    {
                        host Host = await base.GetIbxObject<host>(reference, new string[] { "ALL" });
                        HostName = Host.name;
                    }

                    string Data = "{\"ipv4addrs+\":[{\"ipv4addr\":\"func:nextavailableip:" + network + "\"" +
                           ",\"configure_for_dhcp\":" + ((enableDhcp) ? "true" : "false") +
                           ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + mac + "\"" : "") +
                           ((setHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + HostName + "\"}]" : "") +
                           "}]}";

                    return await base.UpdateIbxObject<host>(reference, Data);
                }
                else
                {
                    throw new ArgumentNullException("network", "The network parameter cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public async Task<string> RemoveDnsHostRecordIPAddresses(string reference, string[] ipAddresses)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                if (ipAddresses != null && ipAddresses.Length > 0)
                {
                    List<string> IPs = new List<string>();

                    foreach (string Address in ipAddresses)
                    {
                        IPAddress IP;

                        if (NetworkAddressTest.IsIPv4Address(Address, out IP, false, true))
                        {
                            IPs.Add($"{{\"ipv4addr\":\"{IP.ToString()}\"}}");
                        }
                    }
                    
                    string Data = $"{{\"ipv4addrs-\":[{String.Join(",", IPs)}]}}";

                    return await base.UpdateIbxObject<host>(reference, Data);
                }
                else
                {
                    throw new ArgumentException("The IP Addresses cannot be null or empty.", "ipAddresses");
                }
            }
            else
            {
                throw new ArgumentNullException("reference", "The reference to remove an IP address from cannot be null or empty.");
            }
        }

        #endregion
    }
}

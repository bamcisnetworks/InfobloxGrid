using System;
using System.Security;
using System.Collections.Generic;
using BAMCIS.Infoblox.Common;
using System.Net;
using BAMCIS.Infoblox.InfobloxObjects.DNS;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public class DnsMethods : IBXCommonMethods
    {
        #region Common Commands

        public DnsMethods(string gridMaster, string apiVersion, string username, SecureString password) : base(gridMaster, apiVersion, username, password) {}

        public DnsMethods(string gridMaster, string apiVersion) : base(gridMaster, apiVersion) { }

        #endregion

        #region Host Records

        public string NewDnsHostRecord(string HostName, string IPAddress, bool AddToDns = true, bool EnableDHCP = false, bool SetHostName = false, string mac = "")
        {
            string ip = String.Empty;
            string fqdn = String.Empty;
            string macAddress = String.Empty;

            if (NetworkAddressTest.isIPv4WithException(IPAddress, out ip) && NetworkAddressTest.IsMACWithExceptionAllowEmpty(mac, out macAddress) &&
                NetworkAddressTest.IsFqdnWithException(HostName, "hostname", out fqdn))
            { 
                string data = "{\"name\":\"" + HostName + "\",\"ipv4addrs\":[{\"ipv4addr\":\"" + ip + "\"" +
                    ((!String.IsNullOrEmpty(macAddress)) ? ",\"mac\":\"" + macAddress + "\"" : "") +
                    ",\"configure_for_dhcp\":" + ((EnableDHCP) ? "true" : "false") +
                    ((SetHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + fqdn + "\"}]" : "") +
                    "}],\"configure_for_dns\":" + AddToDns.ToString().ToLower() + "}";

                return base.NewIbxObject(typeof(host), data);
            }
            else
            {
                return null;
            }
        }

        public string NewDnsHostRecordWithNextAvailableIP(string HostName, string Network, bool AddToDns = true, bool EnableDHCP = false, bool SetHostName = false, string mac = "")
        {
            string formattedMAC = String.Empty;
            string formattedFQDN = String.Empty;

            if (NetworkAddressTest.IsMACWithExceptionAllowEmpty(mac, out formattedMAC) && NetworkAddressTest.IsFqdnWithException(HostName, "hostname", out formattedFQDN))
            {
                string data = "{\"name\":\"" + formattedFQDN + "\",\"ipv4addrs\":[{\"ipv4addr\":\"func:nextavailableip:" + Network + "\"" +
                    ((!String.IsNullOrEmpty(formattedMAC)) ? ",\"mac\":\"" + formattedMAC + "\"" : "") +
                    ",\"configure_for_dhcp\":" + ((EnableDHCP) ? "true" : "false") +
                    ((SetHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + formattedFQDN + "\"}]" : "") + "}]" +
                    ",\"configure_for_dns\":" + AddToDns.ToString().ToLower() + "}";

                return base.NewIbxObject(typeof(host), data);
            }
            else
            {
                return null;
            }
        }

        public string SetDnsHostRecordIP(string reference, string ipAddress, bool EnableDHCP = false, bool SetHostName = true, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                string ip = String.Empty;
                string macAddress = String.Empty;

                if (NetworkAddressTest.isIPv4WithException(ipAddress, out ip) && NetworkAddressTest.IsMACWithExceptionAllowEmpty(mac, out macAddress))
                {
                    string hostName = String.Empty;

                    if (SetHostName)
                    {
                        host Host = GetIbxObject<host>(reference);
                        hostName = Host.name;
                    }

                    string data = "{\"ipv4addrs\":[{\"ipv4addr\":\"" + ip + "\"" +
                        ",\"configure_for_dhcp\":" + ((EnableDHCP) ? "true" : "false") +
                        ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + mac + "\"" : "") +
                        ((SetHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + hostName + "\"}]" : "") +
                        "}]}";

                    return base.UpdateIbxObject<host>(reference, data);
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

        public string SetDnsHostRecordName(string reference, string HostName)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                string fqdn = String.Empty;

                if (NetworkAddressTest.IsFqdnWithException(HostName, "hostname", out fqdn))
                {
                    string data = "{\"name\":\"" + fqdn + "\"}";

                    return base.UpdateIbxObject<host>(reference, data);
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

        public string SetDnsHostRecordNextAvailableIP(string reference, string Network, bool EnableDHCP = false, bool SetHostName = false, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                string macAddress = String.Empty;
                string network = String.Empty;

                if (NetworkAddressTest.IsMACWithExceptionAllowEmpty(mac, out macAddress) && NetworkAddressTest.IsIPv4CidrWithException(Network, out network))
                {
                    string hostName = String.Empty;

                    if (SetHostName)
                    {
                        host Host = GetIbxObject<host>(reference);
                        hostName = Host.name;
                    }

                    string data = "{\"ipv4addrs\":[{\"ipv4addr\":\"func:nextavailableip:" + network + "\"" +
                        ",\"configure_for_dhcp\":" + ((EnableDHCP) ? "true" : "false") +
                        ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + macAddress + "\"" : "") +
                        ((SetHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + hostName + "\"}]" : "") +
                        "}]}";

                    return base.UpdateIbxObject<host>(reference, data);
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

        public string AddDnsHostRecordIPAddresses(string reference, string[] IPAddresses)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                List<string> ips = new List<string>();

                foreach (string ip in IPAddresses)
                {
                    string temp = String.Empty;

                    if (NetworkAddressTest.isIPv4WithException(ip, out temp))
                    {
                        ips.Add(String.Format("{\"ipv4addr\":\"{0}\"}", temp));
                    }
                }

                string data = "{\"ipv4addrs+\":[" + String.Join(",", ips) + "]}";

                return base.UpdateIbxObject<host>(reference, data);
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public string AddDnsHostRecordIPAddress(string reference, string ipAddress, bool EnableDHCP = false, bool SetHostName = false, string mac = "")
        {
            IPAddress ip = IPAddress.Parse(ipAddress);

            if (!String.IsNullOrEmpty(reference))
            {
                string hostName = String.Empty;

                if (SetHostName)
                {
                    host Host = base.GetIbxObject<host>(reference);
                    hostName = Host.name;
                }

                string data = "{\"ipv4addrs+\":[{\"ipv4addr\":\"" + ip.ToString() + "\"" +
                       ",\"configure_for_dhcp\":" + ((EnableDHCP) ? "true" : "false") +
                       ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + mac + "\"" : "") +
                       ((SetHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + hostName + "\"}]" : "") +
                       "}]}";

                return base.UpdateIbxObject<host>(reference, data);
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public string AddDnsHostRecordNextAvailableIPAddress(string reference, string Network, bool EnableDHCP = false, bool SetHostName = false, string mac = "")
        {
            if (!String.IsNullOrEmpty(reference))
            {
                string hostName = String.Empty;

                if (SetHostName)
                {
                    host Host = base.GetIbxObject<host>(reference);
                    hostName = Host.name;
                }

                string data = "{\"ipv4addrs+\":[{\"ipv4addr\":\"func:nextavailableip" + Network + "\"" +
                       ",\"configure_for_dhcp\":" + ((EnableDHCP) ? "true" : "false") +
                       ((!String.IsNullOrEmpty(mac)) ? ",\"mac\":\"" + mac + "\"" : "") +
                       ((SetHostName) ? ",\"options\":[{\"name\":\"host-name\",\"num\":12,\"value\":\"" + hostName + "\"}]" : "") +
                       "}]}";

                return base.UpdateIbxObject<host>(reference, data);
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        public string RemoveDnsHostRecordIPAddresses(string reference, string[] IPAddresses)
        {
            List<string> ips = new List<string>();

            foreach(string ip in IPAddresses)
            {
                string temp = String.Empty;

                if (NetworkAddressTest.isIPv4WithException(ip, out temp))
                {
                    ips.Add(String.Format("{\"ipv4addr\":\"{0}\"}", temp));
                }
            }

            string data = "{\"ipv4addrs-\":[" + String.Join(",", ips) + "]}";

            return base.UpdateIbxObject<host>(reference, data);
        }

        #endregion

        #region All Other Records

        /*public string NewDnsRecord<T>(IEnumerable<KeyValuePair<string, string>> args)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                string data = ExtensionMethods.PrepareArgsForSend<T>(args);

                return base.NewIbxObject(typeof(T), data);
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid dns record type, {0} was provided.", typeof(T).Name));
            }
        }*/


        #endregion
    }
}

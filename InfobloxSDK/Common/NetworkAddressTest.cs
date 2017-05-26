using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace BAMCIS.Infoblox.Common
{
    public static class NetworkAddressTest
    {
        private static Regex fqdnRegex = new Regex(@"(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{0,62}[a-zA-Z0-9\*]\.)+[a-zA-Z]{2,63}$)");
        private static Regex ipv4CidrRegex = new Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])(\\/([0-9]|[1-2][0-9]|3[0-2]))$");
        private static Regex ipv6CidrRegex = new Regex("^s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:)))(%.+)?/(12[0-8]|1[01][0-9]|[4-9][0-9]|3[2-9])$");

        public static bool IsFqdn(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return fqdnRegex.IsMatch(value.Trim());
            }
            else
            {
                return false;
            }
        }

        public static bool IsFqdn(this string value, out string fqdn)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (fqdnRegex.IsMatch(value.Trim()))
                {
                    IdnMapping idn = new IdnMapping();
                    fqdn = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    fqdn = null;
                    return false;
                }
            }
            else
            {
                fqdn = null;
                return false;
            }
        }

        public static bool IsFqdnAllowEmpty(this string value, out string fqdn)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (fqdnRegex.IsMatch(value.Trim()))
                {
                    IdnMapping idn = new IdnMapping();
                    fqdn = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    fqdn = null;
                    return false;
                }
            }
            else
            {
                fqdn = String.Empty;
                return true;
            }
        }

        public static bool IsFqdnWithException(this string value, string propertyName, out string fqdn)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (fqdnRegex.IsMatch(value.Trim()))
                {
                    IdnMapping idn = new IdnMapping();
                    fqdn = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    fqdn = null;
                    throw new FormatException(String.Format("{0} must be a valid FQDN, {1} was provided.", propertyName, value.Trim()));
                }
            }
            else
            {
                fqdn = null;
                throw new ArgumentNullException(value, String.Format("The value for {0} cannot be null or empty.", propertyName));
            }
        }

        public static bool IsFqdnWithExceptionAllowEmpty(this string value, string propertyName, out string fqdn)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (fqdnRegex.IsMatch(value.Trim()))
                {
                    IdnMapping idn = new IdnMapping();
                    fqdn = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    fqdn = null;
                    throw new FormatException(String.Format("{0} must be a valid FQDN, {1} was provided.", propertyName, value.Trim()));
                }
            }
            else
            {
                fqdn = String.Empty;
                return true;
            }
        }

        public static bool isIP(this string value)
        {
            IPAddress ip;
            if (IPAddress.TryParse(value, out ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isIP(this string value, out string ipaddress)
        {
            IPAddress ip;
            if (IPAddress.TryParse(value, out ip))
            {
                ipaddress = ip.ToString();
                return true;
            }
            else
            {
                ipaddress = null;
                return false;
            }
        }

        public static bool isIPWithException(this string value, out string ipaddress)
        {
            IPAddress ip;
            if (IPAddress.TryParse(value, out ip))
            {
                ipaddress = ip.ToString();
                return true;
            }
            else
            {
                ipaddress = null;
                throw new FormatException(String.Format("The provided value, {0}, is not a valid IPv4 or IPv6 address.", value));
            }
        }

        public static bool isIPv4(this string value, out IPAddress ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetwork)
            {
                ip = IP;
                return true;
            }
            else
            {
                ip = null;
                return false;
            }
        }

        public static bool isIPv4(this string value, out string ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetwork)
            {
                ip = IP.ToString();
                return true;
            }
            else
            {
                ip = null;
                return false;
            }
        }

        public static bool isIPv4AllowEmpty(this string value, out string ip)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress IP;
                if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = IP.ToString();
                    return true;
                }
                else
                {
                    ip = null;
                    return false;
                }
            }
            else
            {
                ip = String.Empty;
                return true;
            }
        }

        public static bool isIPv4WithException(this string value, out string ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetwork)
            {
                ip = IP.ToString();
                return true;
            }
            else
            {
                throw new FormatException(String.Format("The address must be a valid IPv4 address, {0} was provided.", value));
            }
        }

        public static bool isIPv4WithExceptionAllowEmpty(this string value, out string ip)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress IP;
                if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetwork)
                {
                    ip = IP.ToString();
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The address must be a valid IPv4 address, {0} was provided.", value));
                }
            }
            else
            {
                ip = String.Empty;
                return true;
            }
        }

        public static bool isIPv4WithException(this string value, out IPAddress ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetwork)
            {
                ip = IP;
                return true;
            }
            else
            {
                throw new FormatException(String.Format("The address must be a valid IPv4 address, {0} was provided.", value));
            }
        }

        public static bool isIPv6(this string value, out IPAddress ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ip = IPAddress.Parse(value);
                return true;
            }
            else
            {
                ip = null;
                return false;
            }
        }

        public static bool isIPv6(this string value, out string ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ip = IP.ToString();
                return true;
            }
            else
            {
                ip = null;
                return false;
            }
        }

        public static bool isIPv6AllowEmpty(this string value, out string ip)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress IP;
                if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ip = IP.ToString();
                    return true;
                }
                else
                {
                    ip = null;
                    return false;
                }
            }
            else
            {
                ip = String.Empty;
                return true;
            }
        }

        public static bool isIPv6WithException(this string value, out string ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ip = IP.ToString();
                return true;
            }
            else
            {
                throw new FormatException(String.Format("The address must be a valid IPv6 address, {0} was provided.", value));
            }
        }

        public static bool isIPv6WithExceptionAllowEmpty(this string value, out string ip)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress IP;
                if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ip = IP.ToString();
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The address must be a valid IPv6 address, {0} was provided.", value));
                }
            }
            else
            {
                ip = String.Empty;
                return true;
            }
        }

        public static bool isIPv6WithException(this string value, out IPAddress ip)
        {
            IPAddress IP;
            if (IPAddress.TryParse(value, out IP) && IP.AddressFamily == AddressFamily.InterNetworkV6)
            {
                ip = IP;
                return true;
            }
            else
            {
                throw new ArgumentException(String.Format("The address must be a valid IPv6 address, {0} was provided.", value));
            }
        }

        public static bool IsIPv4Cidr(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return ipv4CidrRegex.IsMatch(value.Trim());
            }
            else
            {
                return false;
            }
        }

        public static bool IsIPv4Cidr(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv4CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value;
                    return true;
                }
                else
                {
                    cidr = null;
                    return false;
                }
            }
            else
            {
                cidr = null;
                return false;
            }
        }

        public static bool IsIPv4CidrAllowEmpty(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv4CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value;
                    return true;
                }
                else
                {
                    cidr = null;
                    return false;
                }
            }
            else
            {
                cidr = String.Empty;
                return true;
            }
        }

        public static bool IsIPv4CidrWithException(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv4CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value;
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The address must be in an IPv4/CIDR notation, {0} was provided.", value));
                }
            }
            else
            {
                throw new ArgumentNullException("value", "The network cannot be null or empty.");
            }
        }

        public static bool IsIPv4CidrWithExceptionAllowEmpty(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv4CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value;
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The address must be in an IPv4/CIDR notation, {0} was provided.", value));
                }
            }
            else
            {
                cidr = String.Empty;
                return true;
            }
        }

        public static bool IsIPv6Cidr(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return ipv6CidrRegex.IsMatch(value.Trim());
            }
            else
            {
                return false;
            }
        }

        public static bool IsIPv6Cidr(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv6CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value.Trim();
                    return true;
                }
                else
                {
                    cidr = null;
                    return false;
                }
            }
            else
            {
                cidr = null;
                return false;
            }
        }

        public static bool IsIPv6CidrAllowEmpty(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv6CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value.Trim();
                    return true;
                }
                else
                {
                    cidr = null;
                    return false;
                }
            }
            else
            {
                cidr = String.Empty;
                return true;
            }
        }

        public static bool IsIPv6CidrWithException(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv6CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value.Trim();
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The address must be in an IPv6/CIDR notation, {0} was provided.", value));
                }
            }
            else
            {
                throw new ArgumentNullException("value", "The address cannot be null or empty.");
            }
        }

        public static bool IsIPv6CidrWithExceptionAllowEmpty(this string value, out string cidr)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (ipv6CidrRegex.IsMatch(value.Trim()))
                {
                    cidr = value.Trim();
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The address must be in an IPv6/CIDR notation, {0} was provided.", value));
                }
            }
            else
            {
                cidr = String.Empty;
                return true;
            }
        }

        public static bool IsMAC(this string value, out string mac)
        {
            if (!String.IsNullOrEmpty(value))
            {
                Regex macRegex = new Regex(@"[^A-Fa-f0-9$]+");
                value = macRegex.Replace(value.ToUpper(), String.Empty);

                try
                {
                    mac = PhysicalAddress.Parse(value).ToString();
                    return true;
                }
                catch (FormatException)
                {
                    mac = null;
                    return false;
                }
            }
            else
            {
                mac = String.Empty;
                return false;
            }
        }

        public static bool IsMACWithExceptionAllowEmpty(this string value, out string mac)
        {
            if (!String.IsNullOrEmpty(value))
            {
                Regex macRegex = new Regex(@"[^A-Fa-f0-9$]+");
                value = macRegex.Replace(value.ToUpper(), String.Empty);

                try
                {
                    mac = PhysicalAddress.Parse(value).ToString();
                    return true;
                }
                catch (FormatException)
                {
                    throw new ArgumentException(String.Format("The MAC address must be in a valid format, {0} was provided.", value));
                }
            }
            else
            {
                mac = String.Empty;
                return true;
            }
        }

        public static bool IsMACWithException(this string value, out string mac)
        {
            Regex macRegex = new Regex(@"[^A-Fa-f0-9$]+");
            value = macRegex.Replace(value.ToUpper(), String.Empty);

            try
            {
                mac = PhysicalAddress.Parse(value).ToString();
                return true;
            }
            catch (FormatException)
            {
                throw new ArgumentException(String.Format("The MAC address must be in a valid format, {0} was provided.", value));
            }
        }

        public static bool IsValidEmailWithException(this string emailAddress, out string email, bool allowEmpty = false)
        {
            try
            {
                return IsValidEmail(emailAddress, out email, allowEmpty);
            }
            catch (Exception)
            {
                throw new FormatException($"The email address must be in a valid format, {emailAddress.Trim()} was provided");
            }
        }

        public static bool IsValidEmail(this string emailAddress, out string email, bool allowEmpty = false)
        {
            email = String.Empty;

            if (String.IsNullOrEmpty(emailAddress) && allowEmpty == false)
            {
                return false;
            }
            else
            {
                // Use IdnMapping class to convert Unicode domain names.
                try
                {
                    emailAddress = Regex.Replace(emailAddress.Trim(), @"(@)(.+)$", DomainMapper,
                                          RegexOptions.None, TimeSpan.FromMilliseconds(200));
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
                catch (ArgumentException)
                {
                    return false;
                }

                // Return true if emailAddress is in valid e-mail format.
                try
                {
                    bool Result = Regex.IsMatch(emailAddress,
                          @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                    if (Result == true)
                    {
                        email = emailAddress;
                    }

                    return Result;
                }
                catch (RegexMatchTimeoutException)
                {
                    return false;
                }
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping Idn = new IdnMapping();

            string DomainName = match.Groups[2].Value;

            DomainName = Idn.GetAscii(DomainName);

            return match.Groups[1].Value + DomainName;
        }

        public static bool IsIPv4OrFqdn(this string value, string propertyName, out string address)
        {
            IPAddress ip;
            if (NetworkAddressTest.isIPv4(value, out ip))
            {
                address = ip.ToString();
                return true;
            }
            else if (NetworkAddressTest.IsFqdn(value.Trim()))
            {
                IdnMapping idn = new IdnMapping();
                address = idn.GetAscii(value.Trim());
                return true;
            }
            else
            {
                address = null;
                return false;
            }
        }

        public static bool IsIPv4OrFqdnAllowEmpty(this string value, string propertyName, out string address)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress ip;
                if (NetworkAddressTest.isIPv4(value, out ip))
                {
                    address = ip.ToString();
                    return true;
                }
                else if (NetworkAddressTest.IsFqdn(value.Trim()))
                {
                    IdnMapping idn = new IdnMapping();
                    address = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    address = null;
                    return false;
                }
            }
            else
            {
                address = String.Empty;
                return true;
            }
        }

        public static bool IsIPv4OrFqdnWithException(this string value, string propertyName, out string address)
        {
            IPAddress ip;
            if (NetworkAddressTest.isIPv4(value, out ip))
            {
                address = ip.ToString();
                return true;
            }
            else if (NetworkAddressTest.IsFqdn(value))
            {
                IdnMapping idn = new IdnMapping();
                address = idn.GetAscii(value.Trim());
                return true;
            }
            else
            {
                address = null;
                throw new FormatException(String.Format("{0} must be a valid IPv4 or FQDN, {1} was provided.", propertyName, value));
            }
        }

        public static bool IsIPv4OrFqdnWithExceptionAllowEmpty(this string value, string propertyName, out string address)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress ip;
                if (NetworkAddressTest.isIPv4(value, out ip))
                {
                    address = ip.ToString();
                    return true;
                }
                else if (NetworkAddressTest.IsFqdn(value))
                {
                    IdnMapping idn = new IdnMapping();
                    address = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    address = null;
                    throw new FormatException(String.Format("{0} must be a valid IPv4 or FQDN, {1} was provided.", propertyName, value));
                }
            }
            else
            {
                address = String.Empty;
                return true;
            }
        }

        public static bool IsIPv6DUID(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                //TODO: Generate DUID regex;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsIPv6DUID(this string value, out string duid)
        {
            if (!String.IsNullOrEmpty(value))
            {
                //TODO: Generate DUID regex;
                duid = value.Trim();
                return true;
            }
            else
            {
                duid = null;
                return false;
            }
        }

        public static bool IsIPv6DUIDAllowEmpty(this string value, out string duid)
        {
            if (!String.IsNullOrEmpty(value))
            {
                //TODO: Generate DUID regex;
                duid = value.Trim();
                return true;
            }
            else
            {
                duid = String.Empty;
                return true;
            }
        }

        public static bool IsIPv6DUIDWithException(this string value, out string duid)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (true)
                {
                    //TODO: Generate DUID regex;
                    duid = value.Trim();
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The IPv6 DUID must be a valid value, {0} was provided.", value));
                }
            }
            else
            {
                throw new ArgumentException("The IPv6 DUID cannot be null or empty.");
            }
        }

        public static bool IsIPv6DUIDWithExceptionAllowEmpty(this string value, out string duid)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (true)
                {
                    //TODO: Generate DUID regex;
                    duid = value.Trim();
                    return true;
                }
                else
                {
                    throw new FormatException(String.Format("The IPv6 DUID must be a valid value, {0} was provided.", value));
                }
            }
            else
            {
                duid = String.Empty;
                return true;
            }
        }

        public static string TrimValue(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return value.Trim();
            }
            else
            {
                return String.Empty;
            }
        }

        public static string TrimValueWithException(this string value, string propertyName)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return value.Trim();
            }
            else
            {
                throw new ArgumentNullException("value", String.Format("The value for {0} cannot be null or empty.", propertyName));
            }
        }
    }
}

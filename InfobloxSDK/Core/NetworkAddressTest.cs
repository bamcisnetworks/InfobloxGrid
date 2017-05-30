using System;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace BAMCIS.Infoblox.Core
{
    public static class NetworkAddressTest
    {
        private static Regex _FQDNRegex = new Regex(@"(?=^.{4,253}$)(^((?!-)[a-zA-Z0-9-]{0,62}[a-zA-Z0-9\*]\.)+[a-zA-Z]{2,63}$)");
        private static Regex _IPv4CIDRRegex = new Regex("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])(\\/([0-9]|[1-2][0-9]|3[0-2]))$");
        private static Regex _IPv6CIDRRegex = new Regex("^s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:)))(%.+)?/(12[0-8]|1[01][0-9]|[4-9][0-9]|3[2-9])$");

        private static bool Invalid;

        public static bool IsFqdn(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return _FQDNRegex.IsMatch(value.Trim());
            }
            else
            {
                return false;
            }
        }

        public static bool IsFqdn(this string value, string propertyName, out string fqdn, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (_FQDNRegex.IsMatch(value.Trim()))
                {
                    IdnMapping idn = new IdnMapping();
                    fqdn = idn.GetAscii(value.Trim());
                    return true;
                }
                else
                {
                    fqdn = null;

                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException(String.Format("{0} must be a valid FQDN, {1} was provided.", propertyName, value.Trim()));
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                fqdn = String.Empty;
                return allowEmptyString;
            }
        }

        public static bool IsIPAddress(this string value)
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

        public static bool IsIPAddress(this string value, out string ipaddress, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
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

                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The provided value, {value}, is not a valid IPv4 or IPv6 address.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                ipaddress = null;

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("value", "The address cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsIPv4Address(this string value, out IPAddress ip, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
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

                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The address must be a valid IPv4 address, {value} was provided.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                ip = null;

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("value", "The address cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsIPv6Address(this string value, out IPAddress ip, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
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

                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The address must be a valid IPv6 address, {value} was provided.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                ip = null;

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("value", "The address cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsIPv4Cidr(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return _IPv4CIDRRegex.IsMatch(value.Trim());
            }
            else
            {
                return false;
            }
        }

        public static bool IsIPv4Cidr(this string value, out string cidr, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (_IPv4CIDRRegex.IsMatch(value.Trim()))
                {
                    cidr = value;
                    return true;
                }
                else
                {
                    cidr = null;

                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The address must be in an IPv4/CIDR notation, {value} was provided.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                cidr = null;

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("value", "The address cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsIPv6Cidr(this string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                return _IPv6CIDRRegex.IsMatch(value.Trim());
            }
            else
            {
                return false;
            }
        }

        public static bool IsIPv6Cidr(this string value, out string cidr, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
            {
                if (_IPv6CIDRRegex.IsMatch(value.Trim()))
                {
                    cidr = value.Trim();
                    return true;
                }
                else
                {
                    cidr = null;

                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The address must be in an IPv6/CIDR notation, {value} was provided.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                cidr = null;

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("value", "The address cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        
        }

        public static bool IsMAC(this string value, out string mac, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
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

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("value", "The MAC address value cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public static bool IsValidEmail(this string emailAddress, out string email, bool allowEmpty = false, bool throwExceptionOnMiss = false)
        {
            Invalid = false;
            email = null;

            if (!String.IsNullOrEmpty(emailAddress))
            {
                // Use IdnMapping class to convert Unicode domain names.
                try
                {
                    emailAddress = Regex.Replace(emailAddress, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
                }
                catch (RegexMatchTimeoutException)
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The email address must be in a valid format, {emailAddress.Trim()} was provided");
                    }
                    else
                    {
                        return false;
                    }
                }

                if (Invalid) //This is set by the DomainMapper function if it misses
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The email address must be in a valid format, {emailAddress.Trim()} was provided");
                    }
                    else
                    {
                        return false;
                    }
                }

                // Return true if strIn is in valid e-mail format.
                try
                {
                    bool Result = Regex.IsMatch(emailAddress,
                          @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                          RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));

                    if (Result)
                    {
                        return true;
                    }
                    else
                    {
                        if (throwExceptionOnMiss)
                        {
                            throw new FormatException($"The email address must be in a valid format, {emailAddress.Trim()} was provided");
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (RegexMatchTimeoutException)
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new FormatException($"The email address must be in a valid format, {emailAddress.Trim()} was provided");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                email = null;

                if (allowEmpty)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException("emailAddress", "The email address to validate cannot be null or empty.");
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;

            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                Invalid = true;
            }

            return match.Groups[1].Value + domainName;
        }

        public static bool IsIPv4AddressOrFQDN(this string value, string propertyName, out string address, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
        {
            if (!String.IsNullOrEmpty(value))
            {
                IPAddress ip;
                if (NetworkAddressTest.IsIPv4Address(value, out ip))
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
                    throw new FormatException($"{propertyName} must be a valid IPv4 or FQDN, {value} was provided.");
                }
            }
            else
            {
                address = null;

                if (allowEmptyString)
                {
                    return true;
                }
                else
                {
                    if (throwExceptionOnMiss)
                    {
                        throw new ArgumentNullException($"The value for {propertyName} was null or empty and not a valid FQDN or IPv4 address.");
                    }
                    else
                    {
                        return false;
                    }
                }
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

        public static bool IsIPv6DUID(this string value, out string duid, bool allowEmptyString = false, bool throwExceptionOnMiss = false)
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
                throw new ArgumentNullException("value", $"The value for {propertyName} cannot be null or empty.");
            }
        }
    }
}

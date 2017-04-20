using BAMCIS.Infoblox.Common;
using System;
using System.Net;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class msdhcpserver
    {
        private string _ipv4addr;

        public string ipv4addr
        {
            get
            {
                return this._ipv4addr;
            }
            set
            {
                IPAddress ip;
                if ((IPAddress.TryParse(value, out ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) || NetworkAddressTest.IsFqdn(value))
                {
                    this._ipv4addr = value;
                }
                else
                {
                    throw new ArgumentException("The ipv4addr property must be a valid IPv4 address or a FQDN.");
                }
            }
        }
    }
}

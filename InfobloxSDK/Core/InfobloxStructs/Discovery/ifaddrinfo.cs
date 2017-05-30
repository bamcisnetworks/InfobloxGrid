using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class ifaddrinfo
    {
        private string _address;
        private string _network;

        public string address
        {
            get
            {
                return this._address;
            }
            set
            {
                if (IPAddress.TryParse(value, out IPAddress ip))
                {
                    this._address = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The provided IP address was not valid for the ifaddrinfo object.");
                }
            }
        }
        public string address_object { get; set; }
        public string network 
        { 
            get
            {
                return this._network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv4Cidr(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new ArgumentException("The network variable was not in a valid IPv4 CIDR notation");
                }
            }
        } 
    }
}

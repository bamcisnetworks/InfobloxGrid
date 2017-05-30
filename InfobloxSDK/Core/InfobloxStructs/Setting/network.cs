using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting
{
    public class network
    {
        private string _address;
        private string _gateway;
        private string _subnet_mask;

        public string address 
        {
            get
            {
                return this._address;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    this._address = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The address must be a valid IPv4 address.");
                }
            }
        }
        public string gateway 
        {
            get
            {
                return this._gateway;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    this._gateway = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The gateway must be a valid IPv4 address.");
                }
            }
        }
        public string subnet_mask 
        { 
            get
            {
                return this._subnet_mask;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    this._subnet_mask = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The subnet mask must be a valid IPv4 address.");
                }
            }
        }
    }
}

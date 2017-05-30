using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class trapreceiver
    {
        private string _address;
        private string _comment;

        public string address 
        { 
            get
            {
                return this._address;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    this._address = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The address must be a valid IPv4 or IPv6 address.");
                }
            }
        }
        public string comment 
        { 
            get 
            {
                return this._comment;
            }
            set
            {
                this._comment = value.TrimValue();
            }
        }
        public string user { get; set; }
    }
}

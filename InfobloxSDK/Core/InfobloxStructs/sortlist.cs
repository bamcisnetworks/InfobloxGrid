using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class sortlist
    {
        private string _address;

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
                    throw new ArgumentException("The provided address was not a valid IPv4 or IPv6 address.");
                }
            }
        }
        public string[] match_list { get; set; }
    }
}

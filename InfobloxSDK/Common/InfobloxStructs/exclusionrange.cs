using System;
using System.Net;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class exclusionrange
    {
        private string _comment;
        private string _end_address;
        private string _start_address;

        public string comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                if (value.TrimValue().Length <= 256)
                {
                    this._comment = value.TrimValue();
                }
                else
                {
                    throw new ArgumentException("The comment must be less than or equal to 256 characters.");
                }
            }
        }
        public string end_address
        {
            get
            {
                return this._end_address;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        this._end_address = ip.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("The end address must be an IPv4 address.");
                    }
                }
                else
                {
                    throw new ArgumentException("The end address must be a valid IPv4 address.");
                }
            }
        }
        public string start_address
        {
            get
            {
                return this._start_address;
            }
            set
            {
                IPAddress ip;
                if (IPAddress.TryParse(value, out ip))
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        this._start_address = ip.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("The start address must be an IPv4 address.");
                    }
                }
                else
                {
                    throw new ArgumentException("The start address must be a valid IPv4 address.");
                }
            }
        }
    }
}

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using System;
using System.Net;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class syslogserver
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
                    throw new ArgumentException("The address must be a valid IPv4 or IPv6 address.");
                }
            }
        }
        public TcpUdpEnum connection_type { get; set; }
        public LocalInterfaceEnum local_interface { get; set; }
        public MessageNodeIdEnum message_node_id { get; set; }
        public MessageSourceEnum message_source { get; set; }
        public uint port { get; set; }
        public SeverityEnum severity { get; set; }
    }
}

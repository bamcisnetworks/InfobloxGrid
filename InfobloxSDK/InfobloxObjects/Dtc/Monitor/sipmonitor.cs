using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor
{
    [Name("dtc:monitor:sip")]
    public class sipmonitor : tcpmonitor
    {
        private uint _result_code;

        public string ciphers { get; set; }
        public string client_cert { get; set; }
        public string request { get; set; }
        public RequestResultEnum result { get; set; }
        public uint result_code
        {
            get
            {
                return this._result_code;
            }
            set
            {
                if (Enum.IsDefined(typeof(HttpStatusCode), value))
                {
                    this._result_code = value;
                }
                else
                {
                    throw new ArgumentException("The result code must be a valid HTTP status code.");
                }
            }
        }
        public SipTransportEnum transport { get; set; }

        public sipmonitor(string name) : base(name)
        {
            this.ciphers = String.Empty;
            this.client_cert = String.Empty;
            this.port = 5060;
            this.request = String.Empty;
            this.result = RequestResultEnum.CODE_IS;
            this.result_code = 200;
            this.transport = SipTransportEnum.TCP;
        }
    }
}

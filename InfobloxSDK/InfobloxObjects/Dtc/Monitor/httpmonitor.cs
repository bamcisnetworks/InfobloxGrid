using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Linq;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor
{
    [Name("dtc:monitor:http")]
    public class httpmonitor : tcpmonitor
    {
        private string _request;
        private uint _result_code;

        public string ciphers { get; set; }
        public string client_cert { get; set; }
        public string request
        {
            get
            {
                return this._request;
            }
            set
            {
                string[] httpVerbs = new string[] {"GET /", "POST /", "DELETE /", "PUSH /"};
                if (httpVerbs.Contains(value.ToUpper()))
                {
                    this._request = value.ToUpper();
                }
                else
                {
                    throw new ArgumentException("The request property must be a valid HTTP verb.");
                }
            }
        }
        public RequestResultEnum result { get; set; }
        public uint result_code 
        {
            get
            {
                return this._result_code;
            }
            set
            {
                if (System.Enum.IsDefined(typeof(HttpStatusCode), value))
                {
                    this._result_code = value;
                }
                else
                {
                    throw new ArgumentException("The result code must be a valid HTTP status code.");
                }
            }
        }
        public bool secure { get; set; }

        public httpmonitor(string name) : base(name)
        {
            this.ciphers = String.Empty;
            this.client_cert = String.Empty;
            this.port = 80;
            this.request = "GET /";
            this.result = RequestResultEnum.ANY;
            this.result_code = 200;
            this.secure = false;
        }
    }
}

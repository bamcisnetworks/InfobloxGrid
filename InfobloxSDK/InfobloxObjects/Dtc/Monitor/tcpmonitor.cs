using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Monitor
{
    [Name("dtc:monitor:tcp")]
    public class tcpmonitor : BaseMonitorClass
    {
        private uint _port;

        [RequiredNotInherited]
        public uint port
        {
            get
            {
                return this._port;
            }
            set
            {
                if (value >= 1 && value <= 65535)
                {
                    this._port = value;
                }
                else
                {
                    throw new ArgumentException("The port must be a valid integer between 1 and 65535.");
                }
            }
        }

        public tcpmonitor(string name)
        {
            base.comment = String.Empty;
            base.interval = 5;
            base.name = name;
            base.retry_down = 1;
            base.retry_up = 1;
            base.timeout = 15;
        }
    }
}

using BAMCIS.Infoblox.Common.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class msdhcpoption
    {
        private string _type;

        public string name { get; set; }
        public uint num { get; set; }
        public string type
        {
            get
            {
                return this._type;
            }
            set
            {
                if (typeof(DhcpOptionTypeConst).GetTypeInfo().GetFields().Where(x => x.IsLiteral && !x.IsInitOnly).Select(x => x.GetValue(x)).Contains(value))
                {
                    this._type = value;
                }
                else
                {
                    throw new ArgumentException("The option type is not valid.");
                }
            }
        }
        public string user_class { get; set; }
        public string value { get; set; }
        public string vendor_class { get; set; }

        public msdhcpoption()
        {
            this.user_class = "Default User Class";
            this.vendor_class = "DHCP Standard Options";
        }
    }
}

using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class addressac
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
                this._address = value.TrimValue();
            }
        }
        public AllowDenyEnum permission { get; set; }
    }
}

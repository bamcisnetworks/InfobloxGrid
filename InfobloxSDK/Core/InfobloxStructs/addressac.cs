using BAMCIS.Infoblox.Core.Enums;
using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
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

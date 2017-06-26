using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;
using System;

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseRecord : BaseNameCommentObject
    {
        private string _dns_name;
        private string _zone;

        [ReadOnlyAttribute]
        public info cloud_info { get; internal protected set; }

        public bool disable { get; set; }

        [ReadOnlyAttribute]
        public string dns_name
        {
            get
            {
                return this._dns_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_name", out this._dns_name, false, true);
            }
        }

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public override string name
        {
            get
            {
                return base.name;
            }
            set
            {
                string temp = String.Empty;
                NetworkAddressTest.IsFqdn(value, "name", out temp, false, true);
                base.name = temp;
            }
        }

        public uint ttl { get; set; }

        public bool use_ttl { get; set; }

        [SearchableAttribute(Equality = true)]
        [Basic]
        public string view { get; set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string zone 
        {
            get
            {
                return this._zone;
            }
            internal protected set
            {
                this._zone = value.TrimValue();
            }
        }

        public BaseRecord()
        {
            this.disable = false;
            this.use_ttl = false;
            this.view = "default";
        }
    }
}

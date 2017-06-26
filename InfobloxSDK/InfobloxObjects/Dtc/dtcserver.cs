using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs.Dtc;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc
{
    [Name("dtc:server")]
    public class dtcserver : BaseNameCommentObject
    {
        private string _host;
        private string _translation;

        public bool disable { get; set; }

        [ReadOnlyAttribute]
        public health health { get; internal protected set; }

        [Required]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string host
        {
            get
            {
                return this._host;
            }
            set
            {
                this._host = value.TrimValue();
            }
        }

        public string translation
        {
            get
            {
                return this._translation;
            }
            set
            {
                NetworkAddressTest.IsIPv4AddressOrFQDN(value, "translation", out this._translation, false, true);
            }
        }

        public bool use_translation { get; set; }

        public dtcserver(string name)
        {
            this.disable = false;
            base.name = name;
            this.use_translation = false;
        }
    }
}

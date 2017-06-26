using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("snmpuser")]
    public class snmpuser : RefObject
    {
        private string _comment;
        private string _name;

        [ReadOnlyAttribute]
        public AuthenticationProtocolEnum authentication_protocol { get; internal protected set; }

        [ReadOnlyAttribute]
        public string comment
        {
            get
            {
                return this._comment;
            }
            internal protected set
            {
                this._comment = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public bool disbale { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public string name
        {
            get
            {
                return this._name;
            }
            internal protected set
            {
                this._name = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public PrivacyProtocolEnum privacy_protocol { get; internal protected set; }
    }
}


using BAMCIS.Infoblox.Common.Enums;
using System;
namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class extensibleattributedef
    {
        private string _comment;
        private string _default_value;

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string comment 
        { 
            get
            {
                return this._comment;
            }
            set
            {
                if (value.Length <= 256)
                {
                    this._comment = value;
                }
                else
                {
                    throw new ArgumentException("The comment field can hava maximum of 256 characters.");
                }
            }
        }
        public string default_value 
        { 
            get
            {
                return this._default_value;
            }
            set
            {
                if (value.Length <= 256)
                {
                    this._default_value = value;
                }
                else
                {
                    throw new ArgumentException("The default_value field can hava maximum of 256 characters.");
                }
            }

        }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string name { get; set; }
        [SearchableAttribute(Equality = true)]
        public ExtAttrNamespaceEnum @namespace { get; set; }
        [SearchableAttribute(Equality = true)]
        public ObjectTypeEnum type { get; set; }
    }
}

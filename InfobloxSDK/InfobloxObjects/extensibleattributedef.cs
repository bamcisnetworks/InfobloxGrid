using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    public class extensibleattributedef : BaseNameCommentObject
    {
        private string _default_value;

        public string default_value
        {
            get
            {
                return this._default_value;
            }
            set
            {
                Int32 intValue;
                Int64 timestampValue;
                if (Int32.TryParse(value, out intValue))
                {
                    this._default_value = value;
                }
                else if (Int64.TryParse(value, out timestampValue))
                {
                    this._default_value = value;
                }
                else
                {
                    if (value.Length <= 256)
                    {
                        this._default_value = value;
                    }
                    else
                    {
                        throw new ArgumentException("The default_value property must be less than or equal to 256 characters for a string.");
                    }
                }
            }
        }
        [SearchableAttribute(Equality = true)]
        public ExtAttrNamespaceEnum @namespace { get; set; }
        [SearchableAttribute(Equality = true)]
        public ObjectTypeEnum @type { get; set; }
    }
}

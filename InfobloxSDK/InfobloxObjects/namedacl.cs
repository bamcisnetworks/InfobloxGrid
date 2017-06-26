using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("namedacl")]
    public class namedacl : BaseNameCommentObject
    {
        private List<object> _access_list;
        private List<object> _exploded_access_list;

        public object[] access_list
        {
            get
            {
                return this._access_list.ToArray();
            }
            set
            {
                this._access_list = new List<object>();
                ValidateUnknownArray.ValidateHetergenousArray(new List<Type> { typeof(addressac), typeof(tsigac) }, value, out this._access_list);
            }
        }

        [ReadOnlyAttribute]
        public object[] exploded_access_list
        {
            get
            {
                return this._exploded_access_list.ToArray();
            }
            internal protected set
            {
                this._exploded_access_list = new List<object>();
                ValidateUnknownArray.ValidateHetergenousArray(new List<Type> { typeof(addressac), typeof(tsigac) }, value, out this._exploded_access_list);
            }
        }
    }
}

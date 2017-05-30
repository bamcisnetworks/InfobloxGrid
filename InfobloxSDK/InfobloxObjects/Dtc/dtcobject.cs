using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc
{
    [Name("dtc:object")]
    public class dtcobject : RefObject
    {
        private string _comment;
        private string _name;
        private DateTime _status_time;

        [ReadOnlyAttribute]
        public string abstract_type { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string comment
        {
            get
            {
                return this._comment;
            }
            internal protected set
            {
                if (value.TrimValue().Length <= 256)
                {
                    this._comment = value.TrimValue();
                }
                else
                {
                    throw new ArgumentException("The comment must be less than or equal to 256 characters.");
                }
            }

        }
        [ReadOnlyAttribute]
        public string display_type { get; internal protected set; }
        [ReadOnlyAttribute]
        public string[] ipv4_address_list { get; internal protected set; }
        [ReadOnlyAttribute]
        public string[] ipv6_address_list { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
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
        public string @object { get; internal protected set; }
        [ReadOnlyAttribute]
        public DtcColorStatusEnum status { get; internal protected set; }
        [ReadOnlyAttribute]
        public long status_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._status_time);
            }
            internal protected set
            {
                this._status_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
    }
}

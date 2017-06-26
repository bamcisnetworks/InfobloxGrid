using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("zone_auth_discrepancy")]
    public class zone_auth_discrepancy : RefObject
    {
        private DateTime _timestamp;

        [ReadOnlyAttribute]
        [Basic]
        public string description { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public ZoneDiscrepancySeverityEnum severity { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public long timestamp
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._timestamp);
            }
            internal protected set
            {
                this._timestamp = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public string zone { get; internal protected set; }
    }
}

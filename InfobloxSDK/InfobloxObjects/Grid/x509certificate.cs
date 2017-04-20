using BAMCIS.Infoblox.Common;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid
{
    [Name("grid:x509certificate")]
    public class x509certificate : RefObject
    {
        private DateTime _valid_not_after;
        private DateTime _valid_not_before;

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string issuer { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string serial { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string subject { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Negative = true, LessThan = true, Equality = true, GreaterThan = true)]
        public long valid_not_after 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._valid_not_after);
            }
            internal protected set
            {
                this._valid_not_after = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Negative = true, LessThan = true, Equality = true, GreaterThan = true)]
        public long valid_not_before
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._valid_not_before);
            }
            internal protected set
            {
                this._valid_not_before = UnixTimeHelper.FromUnixTime(value);
            }
        }
    }
}

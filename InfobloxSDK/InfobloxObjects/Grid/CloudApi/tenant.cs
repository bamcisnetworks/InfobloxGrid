using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid.CloudApi
{
    [Name("grid:cloudapi:tenant")]
    public class tenant : BaseNameCommentObject
    {
        private DateTime _created_ts;
        private string _id;
        private DateTime _last_event_ts;

        [ReadOnlyAttribute]
        public long created_ts 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._created_ts);
            }
            internal protected set
            {
                this._created_ts = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string id 
        {
            get
            {
                return this._id;
            }
            internal protected set
            {
                this._id = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        public long last_event_ts 
        { 
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_event_ts);
            }
            internal protected set
            {
                this._last_event_ts = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public uint network_count { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint vm_count { get; internal protected set; }
    }
}

using BAMCIS.Infoblox.Common;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid
{
    [Name("grid:maxminddbinfo")]
    public class maxminddbinfo : RefObject
    {
        private DateTime _build_time;
        private DateTime _deployment_time;

        [ReadOnlyAttribute]
        public uint binary_major_version { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint binary_minor_version { get; internal protected set; }
        [ReadOnlyAttribute]
        public long build_time 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._build_time);
            }
            internal protected set
            {
                this._build_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public string database_type { get; internal protected set; }
        [ReadOnlyAttribute]
        public long deployment_time 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._deployment_time);
            }
            internal protected set
            {
                this._deployment_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
    }
}

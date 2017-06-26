using BAMCIS.Infoblox.Core;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Grid
{
    [Name("grid:maxminddbinfo")]
    public class maxminddbinfo : RefObject
    {
        private DateTime _build_time;
        private DateTime _deployment_time;

        [ReadOnlyAttribute]
        [Basic]
        public uint binary_major_version { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public uint binary_minor_version { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
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
        [Basic]
        public string database_type { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
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

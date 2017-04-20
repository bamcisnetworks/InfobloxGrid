using BAMCIS.Infoblox.Common.Enums;
using System;
using System.Linq;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class dnsseckey
    {
        private int[] _allowed = new int[] { 1, 10, 3, 5, 6, 7, 8 };
        private string _algorithm;
        private DateTime _next_event_date;
        private string _public_key;

        public string algorithm 
        {
            get
            {
                return this._algorithm;
            }
            set
            {
                int val;
                if (Int32.TryParse(value, out val))
                {
                    if (_allowed.Contains(val))
                    {
                        this._algorithm = val.ToString();
                    }
                    else
                    {
                        throw new ArgumentException("The provided algorithm was not a valid option.");
                    }
                }
                else
                {
                    throw new ArgumentException("The provided algorithm was not a valid integer value.");
                }
            }
        }
        public long next_event_date
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._next_event_date);
            }
            set
            {
                this._next_event_date = UnixTimeHelper.FromUnixTime(value);
            }
        }
        public string public_key
        {
            get
            {
                return this._public_key;
            }
            set
            {
                this._public_key = value.TrimValue();
            }
        }
        public DnsSecKeyStatusEnum status { get; set; }
        public uint tag { get; set; }
        public DnsSecKeyTypeEnum type { get; set; }
    }
}

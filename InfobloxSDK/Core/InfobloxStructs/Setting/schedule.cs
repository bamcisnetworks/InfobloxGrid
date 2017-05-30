using BAMCIS.Infoblox.Core.Enums;
using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting
{
    public class schedule
    {
        private DateTime _recurring_time;

        public uint day_of_month { get; set; }
        public bool disable { get; set; }
        public uint every { get; set; }
        public FrequencyEnum frequency { get; set; }
        public uint hour_of_day { get; set; }
        public uint minutes_past_hour { get; set; }
        public uint month { get; set; }
        public long recurring_time 
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._recurring_time);
            }
            set
            {
                this._recurring_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        public RepeatEnum repeat { get; set; }
        public string time_zone { get; set; }
        public DaysOfWeekEnum[] weekdays { get; set; }

        public schedule()
        {
            this.day_of_month = 1;
            this.disable = false;
            this.every = 1;
            this.hour_of_day = 1;
            this.minutes_past_hour = 1;
            this.month = 1;
            this.repeat = RepeatEnum.ONCE;
            this.time_zone = "(UTC) Coordinated Universal Time";
        }
    }
}

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("csvimporttask")]
    public class csvimporttask : RefObject
    {
        private DateTime _end_time;
        private DateTime _start_time;

        [NotReadableAttribute]
        public CsvImportActionEnum action { get; set; }
        [ReadOnlyAttribute]
        public string admin_name { get; internal protected set; }
        [ReadOnlyAttribute]
        public long end_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._end_time);
            }
            internal protected set
            {
                this._end_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public string file_name { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint file_size { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public uint import_id { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint lines_failed { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint lines_processed { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint lines_warning { get; internal protected set; }
        public CsvImportOnErrorEnum on_error { get; set; }
        public CsvImportOperationEnum operation { get; set; }
        [ReadOnlyAttribute]
        public CsvImportSeparatorEnum separator { get; internal protected set; }
        [ReadOnlyAttribute]
        public long start_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._start_time);
            }
            internal protected set
            {
                this._start_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public CsvImportStatusEnum status { get; internal protected set; }
        public CsvImportUpdateMethodEnum update_method { get; set; }

        public csvimporttask()
        {
            this.action = CsvImportActionEnum.START;
            this.on_error = CsvImportOnErrorEnum.STOP;
            this.operation = CsvImportOperationEnum.INSERT;
            this.update_method = CsvImportUpdateMethodEnum.OVERRIDE;
        }
    }
}

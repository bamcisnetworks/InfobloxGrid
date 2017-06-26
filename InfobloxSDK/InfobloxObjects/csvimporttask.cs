using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("csvimporttask")]
    public class csvimporttask : RefObject
    {
        private DateTime _end_time;
        private DateTime _start_time;

        [NotReadableAttribute]
        [Basic]
        public CsvImportActionEnum action { get; set; }

        [ReadOnlyAttribute]
        [Basic]
        public string admin_name { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
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
        [Basic]
        public string file_name { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public uint file_size { get; internal protected set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public uint import_id { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public uint lines_failed { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public uint lines_processed { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public uint lines_warning { get; internal protected set; }

        [Basic]
        public CsvImportOnErrorEnum on_error { get; set; }

        [Basic]
        public CsvImportOperationEnum operation { get; set; }

        [ReadOnlyAttribute]
        [Basic]
        public CsvImportSeparatorEnum separator { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
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
        [Basic]
        public CsvImportStatusEnum status { get; internal protected set; }

        [Basic]
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

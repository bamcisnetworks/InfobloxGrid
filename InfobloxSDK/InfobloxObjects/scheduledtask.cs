using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using System;
using System.Collections.Generic;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("scheduledtask")]
    public class scheduledtask
    {
        private string _approver;
        private string _approver_comment;
        private List<string> _execution_details;
        private DateTime _execution_time;
        private DateTime _scheduled_time;
        private DateTime _submit_time;
        private string _submitter;
        private string _submitter_comment;
        private string _ticket_number;

        [SearchableAttribute(Equality = true)]
        public ApprovalStatusEnum approval_status { get; set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string approver
        {
            get
            {
                return this._approver;
            }
            internal protected set
            {
                this._approver = value.TrimValue();
            }
        }
        public string approver_comment
        {
            get
            {
                return this._approver_comment;
            }
            set
            {
                this._approver_comment = value.TrimValue();
            }
        }
        public bool automatic_restart { get; set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(ContainsSearchable = true)]
        public changedobject[] changed_objects { get; internal protected set; }
        [ReadOnlyAttribute]
        public scheduledtask[] dependent_tasks { get; internal protected set; }
        [NotReadableAttribute]
        public bool execute_now { get; set; }
        [ReadOnlyAttribute]
        public string[] execution_details
        {
            get
            {
                return this._execution_details.ToArray();
            }
            internal protected set
            {
                this._execution_details = new List<string>();

                foreach (string item in value)
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        this._execution_details.Add(item);
                    }
                    else
                    {
                        throw new ArgumentException("Execution details cannot be null or empty.");
                    }
                }
            }
        }
        [ReadOnlyAttribute]
        public ExecutionDetailsTypeEnum execution_details_type { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public ExecutionStatusEnum execution_status { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public long execution_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._execution_time);
            }
            internal protected set
            {
                this._execution_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public bool is_network_insight_task { get; internal protected set; }
        [ReadOnlyAttribute]
        public string predecessor_task { get; internal protected set; }
        public bool re_execute_task { get; set; }
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public long scheduled_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._scheduled_time);
            }
            set
            {
                this._scheduled_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public long submit_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._submit_time);
            }
            internal protected set
            {
                this._submit_time = UnixTimeHelper.FromUnixTime(value);
            }
        }
        [ReadOnlyAttribute]
        public string submitter
        {
            get
            {
                return this._submitter;
            }
            internal protected set
            {
                this._submitter = value.TrimValue();
            }
        }
        public string submitter_comment
        {
            get
            {
                return this._submitter_comment;
            }
            set
            {
                this._submitter_comment = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public uint task_id { get; internal protected set; }
        [ReadOnlyAttribute]
        public TaskTypeEnum task_type { get; internal protected set; }
        [ReadOnlyAttribute]
        public string ticket_number
        {
            get
            {
                return this._ticket_number;
            }
            internal protected set
            {
                this._ticket_number = value.TrimValue();
            }
        }

        public scheduledtask()
        {
            this.approver_comment = String.Empty;
            this.automatic_restart = false;
            this.execute_now = false;
            this.submitter_comment = String.Empty;
        }

        public void SetScheduledTime(DateTime scheduled_time)
        {
            this._scheduled_time = scheduled_time;
        }

    }
}

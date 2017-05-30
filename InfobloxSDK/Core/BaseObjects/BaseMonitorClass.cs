namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseMonitorClass : BaseNameCommentObject
    {
        public uint interval { get; set; }
        public uint retry_down { get; set; }
        public uint retry_up { get; set; }
        public uint timeout { get; set; }
    }
}

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseSharedRecord : BaseNameCommentObject
    {
        private string _dns_name;

        public bool disable { get; set; }
        [ReadOnlyAttribute]
        public string dns_name
        {
            get
            {
                return this._dns_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_name", out this._dns_name, false, true);
            }
        }
        public uint ttl { get; set; }
        public bool use_ttl { get; set; }
        [Required]
        public string shared_record_group { get; set; }

        public BaseSharedRecord(string shared_record_group)
        {
            this.disable = false;
            this.use_ttl = false;
            this.shared_record_group = shared_record_group;
        }
    }
}

using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.InfobloxStructs;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("zone_stub")]
    public class zone_stub : BaseZone
    {
        private string _soa_email;
        private string _soa_mname;

        public bool disable_forwarding { get; set; }
        [ReadOnlyAttribute]
        public string soa_email
        {
            get
            {
                return this._soa_email;
            }
            internal protected set
            {
                NetworkAddressTest.IsValidEmail(value, out this._soa_email, false, true);
            }
        }
        [ReadOnlyAttribute]
        public uint soa_expire { get; internal protected set; }
        [ReadOnlyAttribute]
        public string soa_mname
        {
            get
            {
                return this._soa_mname;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "soa_mname", out this._soa_mname, false, true);
            }
        }
        [ReadOnlyAttribute]
        public uint soa_negative_ttl { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint soa_refresh { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint soa_retry { get; internal protected set; }
        [ReadOnlyAttribute]
        public uint soa_serial_number { get; internal protected set; }
        [Required]
        public extserver[] stub_from { get; set; }
        public memberserver[] stub_members { get; set; }
        public msdnsserver[] stub_msservers { get; set; }

        public zone_stub(string fqdn, extserver[] stub_from) : base(fqdn)
        {
            this.disable_forwarding = false;
            this.stub_from = stub_from;
            this.stub_members = new memberserver[0];
            this.stub_msservers = new msdnsserver[0];
        }
    }
}

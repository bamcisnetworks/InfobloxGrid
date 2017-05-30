using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Linq;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class dnsseckeyparams
    {
        private int[] _allowed = new int[] { 1, 10, 3, 5, 6, 7, 8 };
        private string _ksk_algorithm;
        private string _zsk_algorithm;

        public bool enable_ksk_auto_rollover { get; set; }
        public string ksk_algorithm
        {
            get
            {
                return this._ksk_algorithm;
            }
            set
            {
                int val;
                if (Int32.TryParse(value, out val))
                {
                    if (_allowed.Contains(val))
                    {
                        this._ksk_algorithm = val.ToString();
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
        public dnsseckeyalgorithm[] ksk_algorithms { get; set; }
        public bool ksk_email_notification_enabled { get; set; }
        public uint ksk_rollover { get; set; }
        public DnsSecKskRolloverNotificationEnum ksk_rollover_notification_config { get; set; }
        [ObsoleteAttribute("This property is deprecated.", false)]
        public uint ksk_size { get; set; }
        public bool ksk_snmp_notification_enabled { get; set; }
        public NsecEnum next_secure_type { get; set; }
        public uint nsec3_iterations { get; set; }
        public uint nsec3_salt_max_length { get; set; }
        public uint nsec3_salt_min_length { get; set; }
        public uint signature_expiration { get; set; }
        [ObsoleteAttribute("This property is deprecated.", false)]
        public string zsk_algorithm
        {
            get
            {
                return this._zsk_algorithm;
            }
            set
            {
                int val;
                if (Int32.TryParse(value, out val))
                {
                    if (_allowed.Contains(val))
                    {
                        this._zsk_algorithm = val.ToString();
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
        public dnsseckeyalgorithm[] zsk_algorithms { get; set; }
        public uint zsk_rollover { get; set; }
        public DnsSecZskRolloverMechanismEnum zsk_rollover_mechanism { get; set; }
        public uint zsk_size { get; set; }

        public dnsseckeyparams()
        {
            this.ksk_algorithm = "5";
            this.ksk_algorithms = new dnsseckeyalgorithm[] { new dnsseckeyalgorithm() { algorithm = DnsSecKeyAlgorithmEnum.RSASHA1, size = 2048 } };
            this.ksk_email_notification_enabled = false;
            this.ksk_rollover = 31536000;
            this.ksk_rollover_notification_config = DnsSecKskRolloverNotificationEnum.REQUIRE_MANUAL_INTERVENTION;
            this.ksk_size = 2048;
            this.ksk_snmp_notification_enabled = true;
            this.next_secure_type = NsecEnum.NSEC;
            this.nsec3_iterations = 10;
            this.nsec3_salt_max_length = 15;
            this.nsec3_salt_min_length = 1;
            this.signature_expiration = 345600;
            this.zsk_algorithm = "5";
            this.zsk_algorithms = this.ksk_algorithms = new dnsseckeyalgorithm[] { new dnsseckeyalgorithm() { algorithm = DnsSecKeyAlgorithmEnum.RSASHA1, size = 2048 } };
            this.zsk_rollover = 2592000;
            this.zsk_rollover_mechanism = DnsSecZskRolloverMechanismEnum.PRE_PUBLISH;
            this.zsk_size = 1024;
        }
    }
}

using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.Core.Enums;
using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi;
using BAMCIS.Infoblox.Core.InfobloxStructs.Setting;
using BAMCIS.Infoblox.InfobloxObjects.DHCP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.Infoblox.InfobloxObjects
{
    [Name("zone_auth")]
    public class zone_auth : BaseZone
    {
        private List<object> _allow_query;
        private List<object> _allow_transfer;
        private List<object> _allow_update;
        private string _dns_soa_email;
        private DateTime _dnssec_ksk_rollover_date;
        private DateTime _dnssec_zsk_rollover_date;
        private string _effective_record_name_policy;
        private string _import_from;
        private DateTime _last_queried;
        private List<object> _network_associations;
        private string _network_view;
        private string _primary_type;
        private DateTime _rr_not_queried_enabled_time;
        private string _soa_email;
        private List<object> _update_forwarding;
        private DateTime _zone_not_queried_enabled_time;

        public addressac[] allow_active_dir { get; set; }

        public bool allow_gss_tsig_for_underscore_zone { get; set; }

        public bool allow_gss_tsig_zone_updates { get; set; }

        public object[] allow_query
        {
            get
            {
                return this._allow_query.ToArray();
            }
            set
            {
                this._allow_query = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(addressac), typeof(tsigac) }, value, out this._allow_query);
            }
        }

        public object[] allow_transfer
        {
            get
            {
                return this._allow_transfer.ToArray();
            }
            set
            {
                this._allow_transfer = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(addressac), typeof(tsigac) }, value, out this._allow_transfer);
            }
        }

        public object[] allow_update
        {
            get
            {
                return this._allow_update.ToArray();
            }
            set
            {
                this._allow_update = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(addressac), typeof(tsigac) }, value, out this._allow_update);
            }
        }

        public bool allow_update_forwarding { get; set; }

        [ReadOnlyAttribute]
        public info cloud_info { get; internal protected set; }

        public bool copy_xfer_to_notify { get; set; }

        [NotReadableAttribute]
        public bool create_ptr_for_bulk_hosts { get; set; }

        [NotReadableAttribute]
        public bool create_ptr_for_hosts { get; set; }

        public bool create_underscore_zones { get; set; }

        public bool disable_forwarding { get; set; }

        public bool dns_integrity_enable { get; set; }

        public uint dns_integrity_frequency { get; set; }

        public string dns_integrity_member { get; set; }

        public bool dns_integrity_verbose_logging { get; set; }

        [ReadOnlyAttribute]
        public string dns_soa_email
        {
            get
            {
                return this._dns_soa_email;
            }
            internal protected set
            {
                NetworkAddressTest.IsValidEmail(value, out this._dns_soa_email, false, true);
            }
        }

        public dnsseckeyparams dnssec_key_params { get; set; }

        public dnsseckey[] dnssec_keys { get; set; }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public long dnssec_ksk_rollover_date
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._dnssec_ksk_rollover_date);
            }
            internal protected set
            {
                this._dnssec_ksk_rollover_date = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true, LessThan = true, GreaterThan = true)]
        public long dnssec_zsk_rollover_date
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._dnssec_zsk_rollover_date);
            }
            internal protected set
            {
                this._dnssec_zsk_rollover_date = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [NotReadableAttribute]
        public bool do_host_abstraction { get; set; }

        public FailWarnEnum effective_check_names_policy { get; set; }

        [ReadOnlyAttribute]
        public string effective_record_name_policy
        {
            get
            {
                return this._effective_record_name_policy;
            }
            internal protected set
            {
                this._effective_record_name_policy = value.TrimValueWithException("effective_record_name_policy");
            }
        }

        public extserver[] external_primaries { get; set; }

        public extserver[] external_secondaries { get; set; }

        [SearchableAttribute(Equality = true, Regex = true)]
        public memberserver[] grid_primary { get; set; }

        [ReadOnlyAttribute]
        public bool grid_primary_shared_with_ms_parent_delegation { get; internal protected set; }

        public memberserver[] grid_secondaries { get; set; }

        [NotReadableAttribute]
        public string import_from
        {
            get
            {
                return this._import_from;
            }
            set
            {
                NetworkAddressTest.IsIPAddress(value, out this._import_from, false, true);
            }
        }

        [ReadOnlyAttribute]
        public bool is_dnssec_enabled { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool is_dnssec_signed { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool is_multimaster { get; internal protected set; }

        [ReadOnlyAttribute]
        public long last_queried
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._last_queried);
            }
            internal protected set
            {
                this._last_queried = UnixTimeHelper.FromUnixTime(value);
            }
        }
        public gridmember_soamname[] member_soa_mnames { get; set; }

        [ReadOnlyAttribute]
        public gridmember_soaserial[] member_soa_serials { get; internal protected set; }

        public addressac[] ms_allow_transfer { get; set; }

        public MsAllowTransferModeEnum ms_allow_transfer_mode { get; set; }

        public msdnsserver[] ms_primaries { get; set; }

        public msdnsserver[] ms_secondaries { get; set; }

        public bool ms_sync_disabled { get; set; }

        [ReadOnlyAttribute]
        public object[] network_associations
        {
            get
            {
                return this._network_associations.ToArray();
            }
            internal protected set
            {
                this._network_associations = new List<object>();
                ValidateUnknownArray.ValidateHetergenousArray(new List<Type>() { typeof(network), typeof(networkcontainer), typeof(ipv6network), typeof(ipv6networkcontainer) }, value, out this._network_associations);
            }
        }

        [ReadOnlyAttribute]
        public string network_view
        {
            get
            {
                return this._network_view;
            }
            internal protected set
            {
                this._network_view = value.TrimValueWithException("network_view");
            }
        }

        public uint notify_delay { get; set; }

        public string ns_group { get; set; }

        [ReadOnlyAttribute]
        public string primary_type
        {
            get
            {
                return this._primary_type;
            }
            internal protected set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    string[] matches = new string[] { "External", "Grid", "Microsoft", "None" };
                    if (matches.Select(x => x.ToUpper()).Contains(value.ToUpper()))
                    {
                        this._primary_type = (char.ToUpper(value[0]) + value.Substring(1));
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("The primary_type property must be one of the following {0}, {1} was provided.", String.Join(", ", matches), value));
                    }
                }
                else
                {
                    throw new ArgumentException("The primary_type property cannot be null or empty.");
                }
            }
        }

        public string record_name_policy { get; set; }

        [ReadOnlyAttribute]
        public bool records_monitored { get; internal protected set; }

        [NotReadableAttribute]
        public bool restart_if_needed { get; set; }

        [ReadOnlyAttribute]
        public long rr_not_queried_enabled_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._rr_not_queried_enabled_time);
            }
            internal protected set
            {
                this._rr_not_queried_enabled_time = UnixTimeHelper.FromUnixTime(value);
            }
        }

        [NotReadableAttribute]
        public bool set_soa_serial_numbe { get; set; }

        public uint soa_default_ttl { get; set; }

        public string soa_email
        {
            get
            {
                return this._soa_email;
            }
            set
            {
                NetworkAddressTest.IsValidEmail(value, out this._soa_email, true, true);
            }
        }

        public uint soa_expire { get; set; }

        public uint soa_negative_ttl { get; set; }

        public uint soa_refresh { get; set; }

        public uint soa_retry { get; set; }

        public uint soa_serial_number { get; set; }

        public string[] srgs { get; set; }

        public object[] update_forwarding
        {
            get
            {
                return this._update_forwarding.ToArray();
            }
            set
            {
                this._update_forwarding = new List<object>();
                ValidateUnknownArray.ValidateHomogenousArray(new List<Type>() { typeof(addressac), typeof(tsigac) }, value, out this._update_forwarding);
            }
        }

        public bool use_allow_active_dir { get; set; }

        public bool use_allow_query { get; set; }

        public bool use_allow_transfer { get; set; }

        public bool use_allow_update { get; set; }

        public bool use_allow_update_forwarding { get; set; }

        public bool use_check_names_policy { get; set; }

        public bool use_copy_xfer_to_notify { get; set; }

        public bool use_dnssec_key_params { get; set; }

        public bool use_external_primary { get; set; }

        public bool use_grid_zone_timer { get; set; }

        public bool use_import_from { get; set; }

        public bool use_notify_delay { get; set; }

        public bool use_record_name_policy { get; set; }

        public bool use_soa_email { get; set; }

        [ReadOnlyAttribute]
        public long zone_not_queried_enabled_time
        {
            get
            {
                return UnixTimeHelper.ToUnixTime(this._zone_not_queried_enabled_time);
            }
            internal protected set
            {
                this._zone_not_queried_enabled_time = UnixTimeHelper.FromUnixTime(value);
            }
        }

        public zone_auth(string fqdn) : base(fqdn)
        {
            this.allow_active_dir = new addressac[0];
            this.allow_gss_tsig_for_underscore_zone = false;
            this.allow_gss_tsig_zone_updates = false;
            this.allow_query = new object[0];
            this.allow_transfer = new object[0];
            this.allow_update = new object[0];
            this.allow_update_forwarding = false;
            this.copy_xfer_to_notify = false;
            this.create_ptr_for_bulk_hosts = false;
            this.create_ptr_for_hosts = false;
            this.create_underscore_zones = false;
            this.disable_forwarding = false;
            this.dns_integrity_enable = false;
            this.dns_integrity_frequency = 3600;
            this.dns_integrity_member = String.Empty;
            this.dns_integrity_verbose_logging = false;
            this.dnssec_key_params = new dnsseckeyparams()
            {
                enable_ksk_auto_rollover = false,
                ksk_algorithm = "5",
                ksk_algorithms = new dnsseckeyalgorithm[] { new dnsseckeyalgorithm() { algorithm = DnsSecKeyAlgorithmEnum.RSASHA1, size = 2048 } },
                ksk_email_notification_enabled = false,
                ksk_rollover = 31536000,
                ksk_rollover_notification_config = DnsSecKskRolloverNotificationEnum.REQUIRE_MANUAL_INTERVENTION,
                ksk_size = 2048,
                ksk_snmp_notification_enabled = false,
                next_secure_type = NsecEnum.NSEC,
                nsec3_iterations = 10,
                nsec3_salt_max_length = 15,
                nsec3_salt_min_length = 1,
                signature_expiration = 345600,
                zsk_algorithm = "5",
                zsk_algorithms = new dnsseckeyalgorithm[] { new dnsseckeyalgorithm() { algorithm = DnsSecKeyAlgorithmEnum.RSASHA1, size = 1024 } },
                zsk_rollover = 2592000,
                zsk_rollover_mechanism = DnsSecZskRolloverMechanismEnum.PRE_PUBLISH,
                zsk_size = 1024
            };
            this.dnssec_keys = new dnsseckey[0];
            this.do_host_abstraction = false;
            this.effective_check_names_policy = FailWarnEnum.WARN;
            this.external_primaries = new extserver[0];
            this.external_secondaries = new extserver[0];
            this.grid_primary = new memberserver[0];
            this.grid_secondaries = new memberserver[0];
            this.locked = false;
            this.member_soa_mnames = new gridmember_soamname[0];
            this.ms_allow_transfer = new addressac[0];
            this.ms_allow_transfer_mode = MsAllowTransferModeEnum.NONE;
            this.ms_primaries = new msdnsserver[0];
            this.ms_secondaries = new msdnsserver[0];
            this.ms_sync_disabled = false;
            this.notify_delay = 5;
            this.ns_group = String.Empty;
            this.record_name_policy = String.Empty;
            this.update_forwarding = new object[0];
            this.use_allow_active_dir = false;
            this.use_allow_query = false;
            this.use_allow_transfer = false;
            this.use_allow_update = false;
            this.use_allow_update_forwarding = false;
            this.use_check_names_policy = false;
            this.use_copy_xfer_to_notify = false;
            this.use_dnssec_key_params = false;
            this.use_external_primary = false;
            this.use_grid_zone_timer = false;
            this.use_import_from = false;
            this.use_notify_delay = false;
            this.use_record_name_policy = false;
        }
    }
}

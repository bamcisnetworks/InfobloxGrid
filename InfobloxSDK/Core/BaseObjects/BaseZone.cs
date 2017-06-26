using BAMCIS.Infoblox.Core.Enums;
using System;
using System.Net;

namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseZone : BaseCommentObject
    {
        private string _address;
        private string _display_domain;
        private string _dns_fqdn;
        private string _fqdn;
        private string _locked_by;
        private string _mask_prefix;
        private bool _ms_ad_integrated;
        private string _ms_sync_master_name;
        private string _parent;
        private string _prefix;
        private string _view;

        [ReadOnlyAttribute]
        public string address
        {
            get
            {
                return this._address;
            }
            internal protected set
            {
                NetworkAddressTest.IsIPAddress(value, out this._address, false, true);
            }
        }

        public bool disable { get; set; }

        [ReadOnlyAttribute]
        public string display_domain
        {
            get
            {
                return this._display_domain;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "display_domain", out this._display_domain, false, true);
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string dns_fqdn
        {
            get
            {
                return this._dns_fqdn;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "dns_fqdn", out this._dns_fqdn, false, true);
            }
        }

        [Required]
        [SearchableAttribute(Equality = true, Regex = true)]
        public string fqdn
        {
            get
            {
                return this._fqdn;
            }
            set
            {
                NetworkAddressTest.IsFqdn(value, "fqdn", out this._fqdn, false, true);
            }
        }

        public bool locked { get; set; }

        [ReadOnlyAttribute]
        public string locked_by
        {
            get
            {
                return this._locked_by;
            }
            internal protected set
            {
                this._locked_by = value.TrimValueWithException("locked_by");
            }
        }

        [ReadOnlyAttribute]
        public string mask_prefix
        {
            get
            {
                return this._mask_prefix;
            }
            internal protected set
            {
                IPAddress ip;
                if (NetworkAddressTest.IsIPv4Address(value, out ip) || NetworkAddressTest.IsIPv6Address(value, out ip, false, true))
                {
                    this._mask_prefix = ip.ToString();
                }
                else
                {
                    throw new ArgumentException("The mask prefix must be an IPv4 netmask or an IPv6 prefix.");
                }
            }
        }

        public bool ms_ad_integrated
        {
            get
            {
                if (ms_managed == MsManagedEnum.AUTH_BOTH || ms_managed == MsManagedEnum.AUTH_PRIMARY || ms_managed == MsManagedEnum.STUB)
                {
                    return this._ms_ad_integrated;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                this._ms_ad_integrated = value;
            }
        }

        public MsDdnsModeEnum ms_ddns_mode { get; set; }

        [ReadOnlyAttribute]
        public MsManagedEnum ms_managed { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool ms_read_only { get; internal protected set; }

        [ReadOnlyAttribute]
        public string ms_sync_master_name
        {
            get
            {
                return this._ms_sync_master_name;
            }
            internal protected set
            {
                this._ms_sync_master_name = value.TrimValueWithException("ms_sync_master_name");
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string parent
        {
            get
            {
                return this._parent;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "parent", out this._parent, true, true);
            }
        }

        public string prefix
        {
            get
            {
                return this._prefix;
            }
            set
            {
                this._prefix = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool using_srg_associations { get; internal protected set; }

        [SearchableAttribute(Equality = true)]
        [Basic]
        public string view
        {
            get
            {
                return this._view;
            }
            set
            {
                this._view = value.TrimValueWithException("view");
            }
        }

        [SearchableAttribute(Equality = true)]
        public ZoneFormatEnum zone_format { get; set; }


        public BaseZone(string fqdn)
        {
            this.disable = false;
            this.fqdn = fqdn;
            this.locked = false;
            this.ms_ad_integrated = false;
            this.ms_ddns_mode = MsDdnsModeEnum.NONE;
            this.view = "default";
            this.zone_format = ZoneFormatEnum.FORWARD;
        }
    }
}

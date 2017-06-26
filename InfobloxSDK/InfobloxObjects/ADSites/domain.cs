using BAMCIS.Infoblox.Core;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.ADSites
{
    [Name("msserver:adsites:domain")]
    public class domain : RefObject
    {
        private string _ms_sync_master_name;
        private string _name;
        private string _netbios;

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string ea_definition { get; internal protected set; }

        [ReadOnlyAttribute]
        public string ms_sync_master_name
        {
            get
            {
                return this._ms_sync_master_name;
            }
            internal protected set
            {
                NetworkAddressTest.IsFqdn(value, "ms_sync_master_name", out this._ms_sync_master_name, true, true);
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string netbios
        {
            get
            {
                return this._netbios;
            }
            internal protected set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (value.TrimValue().Length <= 15)
                    {
                        this._netbios = value.TrimValue();
                    }
                    else
                    {
                        throw new ArgumentException("The netbios name cannot exceed 15 characters.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("netbios", "The NETBIOS name cannot be null or empty.");
                }
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        [Basic]
        public string network_view { get; internal protected set; }

        [ReadOnlyAttribute]
        public bool read_only { get; internal protected set; }
    }
}

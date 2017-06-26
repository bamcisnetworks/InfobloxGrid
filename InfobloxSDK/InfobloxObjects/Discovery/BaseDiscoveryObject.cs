using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.InfobloxStructs.Discovery;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    public abstract class BaseDiscoveryObject : RefObject
    {
        private string _cap_admin_status_na_reason;
        private string _cap_description_na_reason;
        private string _cap_vlan_assignment_na_reason;
        private string _cap_voice_vlan_na_reason;
        private string _name;
        private string _type;

        [ReadOnlyAttribute]
        public bool cap_admin_status_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_admin_status_na_reason
        {
            get
            {
                return this._cap_admin_status_na_reason;
            }
            internal protected set
            {
                this._cap_admin_status_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_description_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_description_na_reason
        {
            get
            {
                return this._cap_description_na_reason;
            }
            internal protected set
            {
                this._cap_description_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_vlan_assignment_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_vlan_assignment_na_reason
        {
            get
            {
                return this._cap_vlan_assignment_na_reason;
            }
            internal protected set
            {
                this._cap_vlan_assignment_na_reason = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public bool cap_voice_vlan_ind { get; internal protected set; }

        [ReadOnlyAttribute]
        public string cap_voice_vlan_na_reason
        {
            get
            {
                return this._cap_voice_vlan_na_reason;
            }
            internal protected set
            {
                this._cap_voice_vlan_na_reason = value.TrimValue();
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
            internal protected set
            {
                this._name = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string type
        {
            get
            {
                return this._type;
            }
            internal protected set
            {
                this._type = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        public vlaninfo[] vlan_infos { get; internal protected set; }
    }
}

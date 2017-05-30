using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.BaseObjects;
using BAMCIS.Infoblox.InfobloxObjects.DHCP;

namespace BAMCIS.Infoblox.InfobloxObjects.ADSites
{
    [Name("msserver:adsites:site")]
    public class site : BaseNameObject
    {
        [Required]
        [SearchableAttribute(Equality = true)]
        public string domain { get; set; }
        public ipv4network[] networks { get; set; }

        public site(string name)
        {
            base.name = name;
            this.networks = new ipv4network[0];
        }
    }
}

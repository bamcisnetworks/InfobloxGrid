using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.Enums;
using BAMCIS.Infoblox.Common.InfobloxStructs.Dtc;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Topology
{
    [Name("dtc:topology:rule")]
    public class rule : RefObject
    {
        public DestinationTypeEnum dest_type { get; set; }
        public string destination_link { get; set; }
        public topologyrulesource[] sources { get; set; }
        [SearchableAttribute(Equality = true)]
        public string topology { get; set; }
        [ReadOnlyAttribute]
        public bool valid { get; internal protected set; }

        public rule(DestinationTypeEnum dest_type, string destination_link)
        {
            this.dest_type = dest_type;
            this.destination_link = destination_link;
        }
    }
}

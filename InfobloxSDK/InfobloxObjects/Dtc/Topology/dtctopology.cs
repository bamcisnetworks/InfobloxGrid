using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc.Topology
{
    [Name("dtc:topology")]
    public class dtctopology : BaseNameCommentObject
    {
        public rule[] rules { get; set; }

        public dtctopology(string name)
        {
            base.name = name;
            base.comment = String.Empty;
            this.rules = new rule[0];
        }
    }
}

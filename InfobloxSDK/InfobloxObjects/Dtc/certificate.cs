using BAMCIS.Infoblox.Core;

namespace BAMCIS.Infoblox.InfobloxObjects.Dtc
{
    [Name("dtc:certificate")]
    public class dtccertificate
    {
        [ReadOnlyAttribute]
        public string certificate { get; internal protected set; }
        [ReadOnlyAttribute]
        public bool in_use { get; internal protected set; }
    }
}

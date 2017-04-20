using System.ComponentModel;

namespace BAMCIS.Infoblox.Common
{
    public enum SearchType
    {
        [Description("=")]
        EQUALITY,
        [Description("~=")]
        REGEX,
        [Description("!=")]
        NEGATIVE,
        [Description("<")]
        LESS_THAN,
        [Description(">")]
        GREATER_THAN,
        [Description(":=")]
        CASE_INSENSITIVE
    }
}

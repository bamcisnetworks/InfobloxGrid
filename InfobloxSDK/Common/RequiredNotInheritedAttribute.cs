using System;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class RequiredNotInheritedAttribute : Attribute
    {
        public bool Required = true;
    }
}

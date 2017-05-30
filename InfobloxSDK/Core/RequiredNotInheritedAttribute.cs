using System;

namespace BAMCIS.Infoblox.Core
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false)]
    public class RequiredNotInheritedAttribute : Attribute
    {
        public bool Required = true;
    }
}

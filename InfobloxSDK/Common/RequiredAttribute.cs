using System;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public class RequiredAttribute : Attribute
    {
        public bool Required = true;
    }
}

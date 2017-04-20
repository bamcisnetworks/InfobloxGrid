using System;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false)]
    public class ObjectInfoAttribute : Attribute
    {
        public Type ObjectType { get; set; }
        public string Name { get; set; }
        public bool SupportsGlobalSearch { get; set; }
    }
}

using System;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class NotReadableAttribute : Attribute {}
}

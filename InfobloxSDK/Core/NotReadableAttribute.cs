using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class NotReadableAttribute : Attribute
    {

        public static IEnumerable<string> GetReadableProperties<T>()
        {
            return GetReadableProperties(typeof(T));
        }

        public static IEnumerable<string> GetReadableProperties(Type type)
        {
            return type.GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name);
        }
    }
}

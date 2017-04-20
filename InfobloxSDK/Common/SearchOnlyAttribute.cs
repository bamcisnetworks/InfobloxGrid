using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SearchOnlyAttribute : Attribute {
        public static IEnumerable<string> GetSearchOnlyProperties<T>()
        {
            List<PropertyInfo> properties = typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchOnlyAttribute>()).ToList();
            List<string> finalProperties = new List<string>();
            foreach (PropertyInfo info in properties)
            {
                SearchOnlyAttribute attr = info.GetCustomAttribute<SearchOnlyAttribute>();
                if (attr != null)
                {
                    try
                    {
                        string front = info.PropertyType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>().Description;
                        finalProperties.AddRange(info.PropertyType.GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchOnlyAttribute>()).Select(x => front + "." + x.Name).ToList());
                    }
                    catch (Exception) { }
                }
                else
                {
                    finalProperties.Add(info.Name);
                }
            }

            return finalProperties;
        }
    }
}

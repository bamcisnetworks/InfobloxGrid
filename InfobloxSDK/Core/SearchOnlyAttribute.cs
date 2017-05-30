using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SearchOnlyAttribute : Attribute
    {
        public static IEnumerable<string> GetSearchOnlyProperties<T>()
        {
            IEnumerable<PropertyInfo> Properties = typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchOnlyAttribute>());

            List<string> FinalProperties = new List<string>();

            foreach (PropertyInfo PropInfo in Properties)
            {
                SearchOnlyAttribute Attr = PropInfo.GetCustomAttribute<SearchOnlyAttribute>();

                if (Attr != null)
                {
                    try
                    {
                        string Front = String.Empty;

                        DescriptionAttribute DescAttr = PropInfo.PropertyType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>();

                        if (DescAttr != null)
                        {
                            Front = $"{DescAttr.Description}.";
                        }

                        FinalProperties.AddRange(PropInfo.PropertyType.GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchOnlyAttribute>()).Select(x => $"{Front}{x.Name}"));
                    }
                    catch (Exception) { }
                }
                else
                {
                    FinalProperties.Add(PropInfo.Name);
                }
            }

            return FinalProperties;
        }
    }
}

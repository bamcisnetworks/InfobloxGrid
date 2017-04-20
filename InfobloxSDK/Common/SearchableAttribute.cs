using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Linq;
using BAMCIS.Infoblox.Common.InfobloxStructs;
using BAMCIS.Infoblox.InfobloxMethods;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class SearchableAttribute : Attribute 
    {
        public bool Equality { get; set; }
        public bool Regex { get; set; }
        public bool CaseInsensitive { get; set; }
        public bool Negative { get; set; }
        public bool LessThan { get; set; }
        public bool GreaterThan { get; set; }
        public bool ContainsSearchable { get; set; }

        public static IEnumerable<string> GetSearchableProperties<T>()
        {
            List<PropertyInfo> properties = typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchableAttribute>()).ToList();
            List<string> finalProperties = new List<string>();
            foreach (PropertyInfo info in properties)
            {
                SearchableAttribute attr = info.GetCustomAttribute<SearchableAttribute>();
                if (attr != null && attr.ContainsSearchable)
                {
                    try
                    {
                        string front = info.PropertyType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>().Description;
                        finalProperties.AddRange(info.PropertyType.GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchableAttribute>()).Select(x => front + "." + x.Name).ToList());
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

        public static bool SearchTypeAllowed(Type type, SearchType searchType, string searchField)
        {
            PropertyInfo property;

            if (searchField.ToLower().StartsWith("discovered_data."))
            {
                property = typeof(discoverydata).GetTypeInfo().GetProperty(searchField.Substring(searchField.IndexOf(".") + 1));
            }
            else
            {
                property = type.GetTypeInfo().GetProperty(searchField);
            }

            if (property != null)
            {
                if (property.IsAttributeDefined<SearchableAttribute>())
                {
                    SearchableAttribute attribute = property.GetCustomAttribute<SearchableAttribute>();
                    switch (searchType)
                    {
                        case SearchType.EQUALITY:
                            return attribute.Equality;
                        case SearchType.NEGATIVE:
                            return attribute.Negative;
                        case SearchType.CASE_INSENSITIVE:
                            return attribute.CaseInsensitive;
                        case SearchType.REGEX:
                            return attribute.Regex;
                        case SearchType.LESS_THAN:
                            return attribute.LessThan;
                        case SearchType.GREATER_THAN:
                            return attribute.GreaterThan;
                        default:
                            throw new Exception("The search type did not match any of the defined types.");
                    }
                }
                else
                {
                    throw new Exception("The property " + searchField + " is not searchable for the record host object.");
                }
            }
            else
            {
                throw new Exception("Property " + searchField + " does not exist for the host record object.");
            }
        }
    }
}

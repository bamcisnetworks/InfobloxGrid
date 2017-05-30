using BAMCIS.Infoblox.Core.InfobloxStructs;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core
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
            IEnumerable<PropertyInfo> Properties = typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchableAttribute>());
            List<string> FinalProperties = new List<string>();

            foreach (PropertyInfo PropInfo in Properties)
            {
                SearchableAttribute Atrr = PropInfo.GetCustomAttribute<SearchableAttribute>();

                if (Atrr != null && Atrr.ContainsSearchable)
                {
                    try
                    {
                        DescriptionAttribute DescAttr = PropInfo.PropertyType.GetTypeInfo().GetCustomAttribute<DescriptionAttribute>();

                        string Front = String.Empty;

                        if (DescAttr != null)
                        {
                            Front = $"{DescAttr.Description}.";
                        }

                        FinalProperties.AddRange(PropInfo.PropertyType.GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<SearchableAttribute>()).Select(x => $"{Front}{x.Name}"));
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

        public static bool IsSearchTypeAllowed(Type type, SearchType searchType, string searchField)
        {
            if (!String.IsNullOrEmpty(searchField))
            {
                PropertyInfo Property;

                if (searchField.ToLower().StartsWith("discovered_data."))
                {
                    Property = typeof(discoverydata).GetTypeInfo().GetProperty(searchField.Substring(searchField.IndexOf(".") + 1));
                }
                else
                {
                    Property = type.GetTypeInfo().GetProperty(searchField);
                }

                if (Property != null)
                {
                    if (Property.IsAttributeDefined<SearchableAttribute>())
                    {
                        SearchableAttribute Attr = Property.GetCustomAttribute<SearchableAttribute>();

                        switch (searchType)
                        {
                            case SearchType.EQUALITY:
                                {
                                    return Attr.Equality;
                                }
                            case SearchType.NEGATIVE:
                                {
                                    return Attr.Negative;
                                }
                            case SearchType.CASE_INSENSITIVE:
                                {
                                    return Attr.CaseInsensitive;
                                }
                            case SearchType.REGEX:
                                {
                                    return Attr.Regex;
                                }
                            case SearchType.LESS_THAN:
                                {
                                    return Attr.LessThan;
                                }
                            case SearchType.GREATER_THAN:
                                {
                                    return Attr.GreaterThan;
                                }
                            default:
                                {
                                    throw new ArgumentException("The search type did not match any of the defined types.", "searchType");
                                }
                        }
                    }
                    else
                    {
                        throw new Exception($"The property {searchField} is not searchable for the record host object.");
                    }
                }
                else
                {
                    throw new Exception("Property " + searchField + " does not exist for the host record object.");
                }
            }
            else
            {
                throw new ArgumentNullException("searchField", "The search field cannot be null or empty.");
            }
        }

        public static bool IsSearchTypeAllowed<T>(SearchType searchType, string searchField)
        {
            return IsSearchTypeAllowed(typeof(T), searchType, searchField);
        }
    }
}

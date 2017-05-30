using BAMCIS.Infoblox.InfobloxMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ReadOnlyAttribute : Attribute
    {
        public static IEnumerable<string> GetReadOnlyProperties<T>()
        {
            return typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<ReadOnlyAttribute>()).Select(x => x.Name);
        }

        public static string RemoveReadOnlyProperties<T>(T obj)
        {
            if (obj != null)
            {
                List<string> Properties = (List<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(typeof(T)).Invoke(typeof(ReadOnlyAttribute), null);

                JObject JObj = JObject.Parse(JsonConvert.SerializeObject(obj));

                foreach (string Property in Properties)
                {
                    JObj.Remove(Property);
                }

                return JObj.ToString();
            }
            else
            {
                throw new ArgumentNullException("obj", "The object to remove read only properties from cannot be null");
            }
        }

        public static IEnumerable<string> RemoveReadOnlyProperties<T>(IEnumerable<string> properties)
        {
            if (properties != null)
            {
                return properties.Except((IEnumerable<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(typeof(T)).Invoke(typeof(ReadOnlyAttribute), null));
            }
            else
            {
                throw new ArgumentNullException("properties", "The set of property names to remove empty properties from cannot be null.");
            }
        }

        public static string RemoveReadOnlyProperties<T>(string json)
        {
            if (!String.IsNullOrEmpty(json))
            {
                IEnumerable<string> ReadOnlyProperties = (IEnumerable<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(typeof(T)).Invoke(typeof(ReadOnlyAttribute), null);

                JObject JObj = JObject.Parse(json);

                foreach (string prop in ReadOnlyProperties)
                {
                    JObj.Remove(prop);
                }

                return JObj.ToString();
            }
            else
            {
                throw new ArgumentNullException("json", "The json to remove read only properties from cannot be null or empty.");
            }
        }
    }
}

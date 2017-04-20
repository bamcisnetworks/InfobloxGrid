using BAMCIS.Infoblox.InfobloxMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ReadOnlyAttribute : Attribute
    {
        public static IEnumerable<string> GetReadOnlyProperties<T>()
        {
            return typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<ReadOnlyAttribute>()).Select(x => x.Name).ToList();
        }

        public static string RemoveReadOnlyProperties(object obj)
        {
            Type type = obj.GetType();

            string json = JsonConvert.SerializeObject(obj);

            if (type.GetTypeInfo().GetCustomAttribute<ReadOnlyAttribute>() != null)
            {
                List<string> properties = (List<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(type).Invoke(typeof(ReadOnlyAttribute), null);

                JObject jobject = JObject.Parse(JsonConvert.SerializeObject(obj));

                foreach (string item in properties)
                {
                    jobject.Remove(item);
                }

                json = jobject.ToString();
            }

            return json;
        }

        public static string RemoveReadOnlyProperties<T>(T obj)
        {
            List<string> properties = (List<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(typeof(T)).Invoke(typeof(ReadOnlyAttribute), null);

            JObject jobject = JObject.Parse(JsonConvert.SerializeObject(obj));

            foreach (string item in properties)
            {
                jobject.Remove(item);
            }

            return jobject.ToString();
        }

        public static IEnumerable<string> RemoveReadOnlyProperties(Type type, IEnumerable<string> Properties)
        {
            List<string> properties = (List<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(type).Invoke(typeof(ReadOnlyAttribute), null);

            return Properties.Except(properties);
        }

        public static string RemoveReadOnlyProperties<T>(string json)
        {
            List<string> properties = (List<string>)typeof(ReadOnlyAttribute).GetTypeInfo().GetMethod("GetReadOnlyProperties").MakeGenericMethod(typeof(T)).Invoke(typeof(ReadOnlyAttribute), null);

            JObject jobject = JObject.Parse(json);

            foreach (string prop in properties)
            {
                jobject.Remove(prop);
            }

            return jobject.ToString();
        }
    }
}

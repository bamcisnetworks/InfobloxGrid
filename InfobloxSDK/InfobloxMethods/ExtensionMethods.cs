using BAMCIS.Infoblox.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using BAMCIS.Infoblox.Common.Enums;
using System.Net;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public static class ExtensionMethods
    {
        public static bool IsAttributeDefined<TAttribute>(this MemberInfo memberInfo)
        {
            //This calls the next method
            return memberInfo.IsAttributeDefined(typeof(TAttribute));
        }

        public static bool IsAttributeDefined(this MemberInfo memberInfo, Type attributeType)
        {
            return memberInfo.GetCustomAttribute(attributeType) != null;
        }

        public static bool IsAttributeDefined<TAttribute>(this Type memberInfo)
        {
            //This calls the next method
            return memberInfo.IsAttributeDefined(typeof(TAttribute));
        }
        
        public static bool IsAttributeDefined(this Type memberInfo, Type attributeType)
        {
            return memberInfo.GetTypeInfo().GetCustomAttribute(attributeType) != null;
        }

        public static bool IsAttributeDefined<TAttribute>(this FieldInfo fieldInfo)
        {
            //This calls the next method
            return fieldInfo.IsAttributeDefined(typeof(TAttribute));
        }

        public static bool IsAttributeDefined(this FieldInfo fieldInfo, Type attributeType)
        {
            return fieldInfo.GetCustomAttribute(attributeType) != null;
        }

        public static Type GetObjectType(this InfoBloxObjectsEnum ibxObject)
        {
            return ((MemberInfo)(typeof(InfoBloxObjectsEnum).GetTypeInfo().GetMember(ibxObject.ToString())[0])).GetCustomAttribute<ObjectInfoAttribute>().ObjectType;
        }

        public static Type GetObjectType(this DnsRecordTypeEnum ibxObject)
        {
            return ((MemberInfo)(typeof(DnsRecordTypeEnum).GetTypeInfo().GetMember(ibxObject.ToString())[0])).GetCustomAttribute<ObjectInfoAttribute>().ObjectType;
        }

        public static string GetName(this InfoBloxObjectsEnum ibxObject)
        {
            return ((MemberInfo)(typeof(InfoBloxObjectsEnum).GetTypeInfo().GetMember(ibxObject.ToString())[0])).GetCustomAttribute<ObjectInfoAttribute>().Name;
        }

        public static string GetName(this DnsRecordTypeEnum ibxObject)
        {
            return ((MemberInfo)(typeof(DnsRecordTypeEnum).GetTypeInfo().GetMember(ibxObject.ToString())[0])).GetCustomAttribute<ObjectInfoAttribute>().Name;
        }

        public static bool CanGlobalSearch(this InfoBloxObjectsEnum ibxObject)
        {
            return ((MemberInfo)(typeof(InfoBloxObjectsEnum).GetTypeInfo().GetMember(ibxObject.ToString())[0])).GetCustomAttribute<ObjectInfoAttribute>().SupportsGlobalSearch;
        }

        public static string GetNameAttribute(this Type type)
        {
            if (type.IsAttributeDefined<NameAttribute>())
            {
                return type.GetTypeInfo().GetCustomAttribute<NameAttribute>(true).Name;
            }
            else
            {
                return String.Empty;
            }
        }

        public static string GetNameAttributeUrlEncoded(dynamic obj)
        {
            return WebUtility.UrlEncode(ExtensionMethods.GetNameAttribute(obj));
        }

        public static string GetNameAttributeUrlEncoded(this Type type)
        {
            return WebUtility.UrlEncode(type.GetNameAttribute());
        }

        public static string GetNameAttribute<T>()
        {
            if (typeof(T).IsAttributeDefined<NameAttribute>())
            {
                return typeof(T).GetTypeInfo().GetCustomAttribute<NameAttribute>(true).Name;
            }
            else
            {
                return String.Empty;
            }
        }

        public static string GetNameAttribute(object obj)
        {
            if (obj.GetType() == typeof(Type))
            {
                return ((Type)obj).GetNameAttribute();
            }
            else
            {
                return Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>()
                    .Where(x => x.GetObjectType() != null).First(x => x.GetObjectType().Equals(obj.GetType()))
                    .GetObjectType().GetTypeInfo().GetCustomAttribute<NameAttribute>(true).Name;
            }
        }

        public static string RemoveEmptyProperties<T>(this T InfobloxObject)
        {
            if (IsInfobloxType<T>())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(InfobloxObject);

                    IEnumerable<string> props = InfobloxObject.GetType().GetTypeInfo().GetProperties().Where(x => String.IsNullOrEmpty(x.GetValue(InfobloxObject) as string)).Select(x => x.Name);

                    JObject jobject = JObject.Parse(json);

                    foreach (string item in props)
                    {
                        jobject.Remove(item);
                    }

                    return jobject.ToString();
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public static string RemoveEmptyProperties<T>(string json)
        {
            if (IsInfobloxType<T>())
            {
                try
                {
                    T InfobloxObject = JsonConvert.DeserializeObject<T>(json);

                    IEnumerable<string> props = typeof(T).GetTypeInfo().GetProperties().Where(x => String.IsNullOrEmpty(x.GetValue(InfobloxObject) as string)).Select(x => x.Name);

                    JObject jobject = JObject.Parse(json);

                    foreach (string item in props)
                    {
                        jobject.Remove(item);
                    }

                    return jobject.ToString();
                }
                catch (Exception)
                {
                    return String.Empty;
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public static string PrepareArgsForSend(Type type, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (type.IsInfobloxType())
            {
                string recordType = typeof(InfoBloxObjectsEnum).GetTypeInfo().GetFields().Where(x => x.IsAttributeDefined<ObjectInfoAttribute>()).Where(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType != null).First(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType.Equals(type)).GetValue(typeof(InfoBloxObjectsEnum)).ToString();

                string data = "{";

                foreach (KeyValuePair<string, string> arg in args.Where(x => !x.Value.Equals(recordType)))
                {
                    data += "\"" + arg.Key.ToLower() + "\":\"" + arg.Value + "\",";
                }

                data = data.Substring(0, data.Length - 1);
                data += "}";

                return data;
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object, {0} was provided.", type.Name));
            }
        }

        public static string PrepareArgsForSend<T>(IEnumerable<KeyValuePair<string, string>> args)
        {
            return PrepareArgsForSend(typeof(T), args);
        }

        public static string PrepareObjectForSend<T>(T obj) where T : RefObject
        {
            string json = RemoveEmptyProperties<T>(obj);
            json = RefObject.RemoveRefProperty(json);
            json = ReadOnlyAttribute.RemoveReadOnlyProperties<T>(json);
            return json;
        }

        public static string PrepareObjectForSendWithEmptyProperties<T>(T obj) where T : RefObject
        {
            string json = obj.RemoveRefProperty();
            json = ReadOnlyAttribute.RemoveReadOnlyProperties<T>(json);
            return json;
        }

        public static bool IsInfobloxType(this Type type)
        {
            return typeof(InfoBloxObjectsEnum).GetTypeInfo().GetFields().Where(x => x.IsAttributeDefined<ObjectInfoAttribute>()).Select(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType).Contains(type);
        }

        public static bool IsInfobloxType<T>()
        {
            return typeof(InfoBloxObjectsEnum).GetTypeInfo().GetFields().Where(x => x.IsAttributeDefined<ObjectInfoAttribute>()).Select(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType).Contains(typeof(T));
        }

        public static bool IsInfobloxDnsRecordType(this Type type)
        {
            return IBXCommonMethods.GetDnsRecordTypes().Select(x => x.GetObjectType()).Contains(type);
        }

        public static bool IsInfobloxDhcpRecordType(this Type type)
        {
            return IBXCommonMethods.GetDhcpRecordTypes().Select(x => x.GetObjectType()).Contains(type);
        }

        public static bool IsInfobloxZoneType(this Type type)
        {
            return IBXCommonMethods.GetZoneTypes().Select(x => x.GetObjectType()).Contains(type);
        }

    }
}

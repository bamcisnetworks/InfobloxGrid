using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Core.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public static class InfobloxSDKExtensionMethods
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
            if (type != null)
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
            else
            {
                throw new ArgumentNullException("type", "The type object cannot be null.");
            }
        }

        public static string GetNameAttributeUrlEncoded(dynamic obj)
        {
            return WebUtility.UrlEncode(InfobloxSDKExtensionMethods.GetNameAttribute(obj));
        }

        public static string GetNameAttributeUrlEncoded(this Type type)
        {
            if (type != null)
            {
                return WebUtility.UrlEncode(type.GetNameAttribute());
            }
            else
            {
                throw new ArgumentNullException("type", "The type object cannot be null.");
            }
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
            if (obj != null)
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
            else
            {
                throw new ArgumentNullException("obj", "The object parameter cannot be null");
            }
        }

        public static string RemoveEmptyProperties<T>(this T infobloxObject)
        {
            if (infobloxObject != null)
            {
                if (IsInfobloxType<T>())
                {
                    try
                    {
                        string Json = JsonConvert.SerializeObject(infobloxObject);

                        IEnumerable<string> Properties = infobloxObject.GetType().GetTypeInfo().GetProperties().Where(x => String.IsNullOrEmpty(x.GetValue(infobloxObject) as string)).Select(x => x.Name);

                        JObject JObj = JObject.Parse(Json);

                        foreach (string Prop in Properties)
                        {
                            JObj.Remove(Prop);
                        }

                        return JObj.ToString();
                    }
                    catch (Exception)
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    throw new ArgumentException($"The type must be a valid infoblox object type, {typeof(T).FullName} was provided.");
                }
            }
            else
            {
                throw new ArgumentNullException("infobloxObject", "The Infoblox Object cannot be null.");
            }
        }

        public static string RemoveEmptyProperties<T>(this string json)
        {
            if (!String.IsNullOrEmpty(json))
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
            else
            {
                throw new ArgumentNullException("json", "The object json to remove empty properties from cannot be null or empty.");
            }
        }

        public static string PrepareArgsForSend(Type type, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (type != null)
            {
                if (args != null)
                {
                    if (type.IsInfobloxType())
                    {
                        string recordType = typeof(InfoBloxObjectsEnum).GetTypeInfo().GetFields().Where(x => x.IsAttributeDefined<ObjectInfoAttribute>()).Where(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType != null).First(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType.Equals(type)).GetValue(typeof(InfoBloxObjectsEnum)).ToString();

                        StringBuilder SB = new StringBuilder();

                        SB.Append("{");

                        foreach (KeyValuePair<string, string> arg in args.Where(x => !x.Value.Equals(recordType)))
                        {
                            SB.Append($"\"{arg.Key.ToLower()}\":\"{arg.Value}\",");
                        }

                        SB.Remove(SB.Length - 1, 1);
                        SB.Append("}");

                        return SB.ToString();
                    }
                    else
                    {
                        throw new ArgumentException($"The type must be a valid infoblox object, {type.FullName} was provided.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("args", "The args to prepare cannot be null.");
                }
            }
            else
            {
                throw new ArgumentNullException("type", "The type for the object to prepare to send cannot be null.");
            }
        }

        public static string PrepareArgsForSend<T>(IEnumerable<KeyValuePair<string, string>> args)
        {
            return PrepareArgsForSend(typeof(T), args);
        }

        public static string PrepareObjectForSend<T>(T obj, bool removeEmptyProperties = true) where T : RefObject
        {
            if (obj != null)
            {
                string Json =  obj.RemoveRefProperty();

                if (removeEmptyProperties)
                {
                    Json = Json.RemoveEmptyProperties<T>();
                }

                Json = Json.RemoveReadOnlyProperties<T>();
                return Json;
            }
            else
            {
                throw new ArgumentNullException("obj", "The object to prepare for sending cannot be null.");
            }
        }

        public static bool IsInfobloxType(this Type type)
        {
            if (type != null)
            {
                return typeof(InfoBloxObjectsEnum).GetTypeInfo().GetFields().Where(x => x.IsAttributeDefined<ObjectInfoAttribute>()).Select(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType).Contains(type);
            }
            else
            {
                throw new ArgumentNullException("type", "The type to check cannot be null.");
            }
        }

        public static bool IsInfobloxType<T>()
        {
            return typeof(InfoBloxObjectsEnum).GetTypeInfo().GetFields().Where(x => x.IsAttributeDefined<ObjectInfoAttribute>()).Select(x => x.GetCustomAttribute<ObjectInfoAttribute>().ObjectType).Contains(typeof(T));
        }

        public static bool IsInfobloxDnsRecordType(this Type type)
        {
            if (type != null)
            {
                return IBXCommonMethods.GetDnsRecordTypes().Select(x => x.GetObjectType()).Contains(type);
            }
            else
            {
                throw new ArgumentNullException("type", "The type to check cannot be null.");
            }
        }

        public static bool IsInfobloxDhcpRecordType(this Type type)
        {
            if (type != null)
            {
                return IBXCommonMethods.GetDhcpRecordTypes().Select(x => x.GetObjectType()).Contains(type);
            }
            else
            {
                throw new ArgumentNullException("type", "The type to check cannot be null.");
            }
        }

        public static bool IsInfobloxZoneType(this Type type)
        {
            if (type != null)
            {
                return IBXCommonMethods.GetZoneTypes().Select(x => x.GetObjectType()).Contains(type);
            }
            else
            {
                throw new ArgumentNullException("type", "The type to check cannot be null.");
            }
        }

        #region ReadOnlyAttribute Methods

        public static string RemoveReadOnlyProperties<T>(this T obj)
        {
            return ReadOnlyAttribute.RemoveReadOnlyProperties<T>(obj);
        }

        public static IEnumerable<string> RemoveReadOnlyProperties<T>(this IEnumerable<string> properties)
        {
            return ReadOnlyAttribute.RemoveReadOnlyProperties<T>(properties);
        }

        public static string RemoveReadOnlyProperties<T>(this string json)
        {
            return ReadOnlyAttribute.RemoveReadOnlyProperties<T>(json);
        }

        #endregion

        #region SearchOnlyAttribute Methods

        public static IEnumerable<string> GetSearchOnlyProperties<T>(this T obj)
        {
            return SearchOnlyAttribute.GetSearchOnlyProperties<T>();
        }

        #endregion

        #region SearchableAttribute Methods

        public static IEnumerable<string> GetSearchableProperties<T>(this T obj)
        {
            return SearchableAttribute.GetSearchableProperties<T>();
        }

        public static bool IsSearchTypeAllowed<T>(this SearchType searchType, string searchField)
        {
            return SearchableAttribute.IsSearchTypeAllowed<T>(searchType, searchField);
        }

        public static bool IsSearchTypeAllowed(this SearchType searchType, Type type, string searchField)
        {
            return SearchableAttribute.IsSearchTypeAllowed(type, searchType, searchField);
        }

        #endregion

        #region NotReadableAttribute Methods

        public static IEnumerable<string> GetReadableProperties<T>(this T obj)
        {
            return NotReadableAttribute.GetReadableProperties<T>();
        }

        public static IEnumerable<string> GetReadableProperties(this Type type)
        {
            return NotReadableAttribute.GetReadableProperties(type);
        }

        #endregion
    }
}

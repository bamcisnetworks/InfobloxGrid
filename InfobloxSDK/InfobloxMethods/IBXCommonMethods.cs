#if (NETSTANDARD2_0 || NETSTANDARD1_6 || NETSTANDARD1_5 || NETSTANDARD1_4 || NETSTANDARD1_3 || NETSTANDARD1_2 || NETSTANDARD1_1 || NETSTANDARD1_0)
#define NETSTANDARD 
#else
#define NET
#endif

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.Common.BaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public class IBXCommonMethods
    {
        private HttpClient _client;

        public HttpClient Client
        {
            get
            {
                return this._client;
            }
        }

        public IBXCommonMethods(string gridMaster, string apiVersion, string username, SecureString password)
        {
            if (String.IsNullOrEmpty(username) || password == null)
            {
                this._client = CommandHelpers.BuildHttpClient(gridMaster, apiVersion).Result;
            }
            else
            {
                this._client = CommandHelpers.BuildHttpClient(gridMaster, apiVersion, username, password).Result;
            }
        }

        public IBXCommonMethods(string gridMaster, string apiVersion)
        {
            this._client = CommandHelpers.BuildHttpClient(gridMaster, apiVersion).Result;
        }

        public T SearchIbxObject<T>(SearchType searchType, string searchField, string value)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                return this.GetAsync<T>(CommandHelpers.BuildGetSearchRequest<T>(searchType, searchField, value)).Result;
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public T GetIbxObject<T>(string reference)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                if (!String.IsNullOrEmpty(reference))
                {
                    return this.GetAsync<T>(CommandHelpers.BuildGetRequest<T>(reference)).Result;
                }
                else
                {
                    throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public string NewIbxObject(Type type, string json)
        {
            if (type.IsInfobloxType())
            {
                if (!String.IsNullOrEmpty(json))
                {
                    return this.PostAsync(type.GetNameAttribute(), json).Result;
                }
                else
                {
                    throw new ArgumentNullException("json", "The json string cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", type.Name));
            }
        }

        public string NewIbxObject(dynamic ibxObject)
        {
            if (ibxObject != null)
            {
                if (ExtensionMethods.IsInfobloxType(ibxObject.GetType()))
                {
                    return this.PostAsync(ExtensionMethods.GetNameAttribute(ibxObject), ExtensionMethods.PrepareObjectForSend(ibxObject)).Result;
                }
                else
                {
                    throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", ibxObject.GetType().Name));
                }
            }
            else
            {
                throw new ArgumentNullException("ibxObject", "The ibxObject parameter cannot be null.");
            }
        }

        public string NewIbxObject(Type type, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (type.IsInfobloxType())
            {
                if (args.Count() > 0)
                {
                    return this.PostAsync(type.GetNameAttribute(), ExtensionMethods.PrepareArgsForSend(type, args)).Result;
                }
                else
                {
                    throw new ArgumentNullException("args", "The length of arguments must be greater than 0.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", type.Name));
            }
        }

        public string UpdateIbxObject(dynamic ibxObject, bool RemoveEmpty = true)
        {
            if (ExtensionMethods.IsInfobloxType(ibxObject.GetType()))
            {
                if (ibxObject != null)
                {
                    PropertyInfo prop = ibxObject.GetType().GetProperty("_ref");

                    if (prop != null)
                    {
                        string reference = prop.GetValue(ibxObject) as string;

                        if (!String.IsNullOrEmpty(reference))
                        {
                            string json = String.Empty;

                            if (RemoveEmpty)
                            {
                                json = ExtensionMethods.PrepareObjectForSend(ibxObject);
                            }
                            else
                            {
                                json = ExtensionMethods.PrepareObjectForSendWithEmptyProperties(ibxObject);
                            }

                            return this.PutAsync(reference, json).Result;
                        }
                        else
                        {
                            throw new ArgumentNullException("ibxObject._ref", "The _ref property cannot be null or empty.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The updated object must contain a valid _ref property data cannot be null.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("ibxObject", "The updated object cannot be null.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", ibxObject.GetType().Name));
            }
        }

        public string UpdateIbxObject(Type type, string reference, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (type.IsInfobloxType())
            {
                if (args != null)
                {
                    if (!String.IsNullOrEmpty(reference))
                    {
                        return this.PutAsync(reference, ExtensionMethods.PrepareArgsForSend(type, args)).Result;
                    }
                    else
                    {
                        throw new ArgumentNullException("reference", "The reference data cannot be null or empty.");
                    }

                }
                else
                {
                    throw new ArgumentNullException("ibxObject", "The updated object cannot be null.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", type.Name));
            }
        }

        public string UpdateIbxObject<T>(T ibxObject, bool RemoveEmpty = true) where T : RefObject
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                if (ibxObject != null)
                {
                    PropertyInfo prop = ibxObject.GetType().GetTypeInfo().GetProperty("_ref");

                    if (prop != null)
                    {
                        string reference = prop.GetValue(ibxObject) as string;

                        if (!String.IsNullOrEmpty(reference))
                        {
                            string json = String.Empty;

                            if (RemoveEmpty)
                            {
                                json = ExtensionMethods.PrepareObjectForSend(ibxObject);
                            }
                            else
                            {
                                json = ExtensionMethods.PrepareObjectForSendWithEmptyProperties(ibxObject);
                            }

                            return this.PutAsync(reference, json).Result;
                        }
                        else
                        {
                            throw new ArgumentNullException("ibxObject._ref", "The _ref property cannot be null or empty.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The updated object must contain a valid _ref property data cannot be null.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("ibxObject", "The updated object cannot be null.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public string UpdateIbxObject<T>(string url, string data)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                if (!String.IsNullOrEmpty(data))
                {
                    if (!String.IsNullOrEmpty(url))
                    {
                        return this.PutAsync(url, data).Result;
                    }
                    else
                    {
                        throw new ArgumentNullException("url", "The url parameter cannot be null or empty.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("data", "The updated data cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public string DeleteIbxObject(string reference)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                return this.DeleteAsync(reference).Result;
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        private async Task<T> GetAsync<T>(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                return CommandHelpers.ParseGetResponse<T>(await this._client.GetAsync(url.Replace(":", "%3a")));
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        private async Task<string> PostAsync(string url, string data)
        {
            if (!String.IsNullOrEmpty(url))
            {
                if (!String.IsNullOrEmpty(data))
                {
                    return CommandHelpers.ParsePostPutDeleteResponse(await this.Client.PostAsync(url.Replace(":", "%3a"), new StringContent(data, Encoding.UTF8, "application/json")));
                }
                else
                {
                    throw new ArgumentNullException("data", "A null data set cannot be posted.");
                }
            }
            else
            {
                throw new ArgumentNullException("url", "The url string cannot be null or empty.");
            }
        }

        private async Task<string> PutAsync(string url, string data)
        {
            if (!String.IsNullOrEmpty(url))
            {
                return CommandHelpers.ParsePostPutDeleteResponse(await this._client.PutAsync(url.Replace(":", "%3a"), new StringContent(data, Encoding.UTF8, "application/json")));
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }

        private async Task<string> DeleteAsync(string reference)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                return CommandHelpers.ParsePostPutDeleteResponse(await this._client.DeleteAsync(reference.Replace(":", "%3a")));
            }
            else
            {
                throw new ArgumentNullException("reference", "Reference data cannot be null or empty.");
            }
        }
       
        public static IEnumerable<InfoBloxObjectsEnum> GetDnsRecordTypes()
        {
            return Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().Where(x => !String.IsNullOrEmpty(x.GetName()) && x.GetName().StartsWith("record:"));
        }

        public static IEnumerable<InfoBloxObjectsEnum> GetDhcpRecordTypes()
        {
            return Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().Where(x => x.GetObjectType() != null && (typeof(ExtensionMethods).GetTypeInfo().Assembly.GetTypes().Where(y => y.Namespace != null && y.Namespace.Equals("BAMCIS.Infoblox.InfobloxObjects.DHCP"))).Contains(x.GetObjectType()));
        }

        public static IEnumerable<InfoBloxObjectsEnum> GetZoneTypes()
        {
            return Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().Where(x => x.GetObjectType() != null && x.GetObjectType().GetTypeInfo().BaseType == typeof(BaseZone));
        }

        public static Type GetTypeFromName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                IEnumerable<InfoBloxObjectsEnum> temp = Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().Where(x => !String.IsNullOrEmpty(x.GetName()));

                if (temp.Select(x => x.GetName().ToLower()).Contains(name.ToLower()))
                {
                    return temp.FirstOrDefault(x => x.Equals(name.ToLower())).GetObjectType();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static InfoBloxObjectsEnum GetInfobloxObjectEnumFromName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                IEnumerable<InfoBloxObjectsEnum> temp = Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().Where(x => !String.IsNullOrEmpty(x.GetName()));

                if (temp.Select(x => x.GetName().ToLower()).Contains(name.ToLower()))
                {
                    return temp.FirstOrDefault(x => x.GetName().ToLower().Equals(name.ToLower()));
                }
                else
                {
                    throw new ArgumentException(String.Format("The name value did not match any object type names, {0} was provided.", name));
                }
            }
            else
            {
                throw new ArgumentNullException("The name value cannot be null or empty.");
            }
        }

    }
}

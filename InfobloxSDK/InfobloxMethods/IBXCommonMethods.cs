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
using BAMCIS.Infoblox.Errors;

namespace BAMCIS.Infoblox.InfobloxMethods
{
    public class IBXCommonMethods
    {
        private HttpClient _Client;

        public HttpClient Client
        {
            get
            {
                return this._Client;
            }
        }

        public IBXCommonMethods(TimeSpan? timeout = null)
        {
            if (InfobloxSessionData.UseSessionData)
            {
                if (InfobloxSessionData.Cookie != null && !InfobloxSessionData.Cookie.Expired)
                {
                    this._Client = CommandHelpers.BuildHttpClient(InfobloxSessionData.GridMaster, InfobloxSessionData.Version, InfobloxSessionData.Cookie, timeout).Result;
                }
                else
                {
                    if (InfobloxSessionData.Credential != null)
                    {
                        this._Client = CommandHelpers.BuildHttpClient(InfobloxSessionData.GridMaster, InfobloxSessionData.Version, InfobloxSessionData.Credential.UserName, InfobloxSessionData.Credential.Password, timeout).Result;
                    }
                    else
                    {
                        throw new ArgumentException("The session parameter did not contain a valid cookie and a null credential.", "session");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Cannot use the default constructor because the session data for Infoblox has not been set.");
            }
        }

        public IBXCommonMethods(InfobloxSession session, TimeSpan? timeout = null)
        {
            if (session != null)
            {
                if (session.Cookie != null && !session.Cookie.Expired)
                {
                    this._Client = CommandHelpers.BuildHttpClient(session.GridMaster, session.Version, session.Cookie, timeout).Result;
                }
                else
                {
                    if (session.Credential != null)
                    {
                        this._Client = CommandHelpers.BuildHttpClient(session.GridMaster, session.Version, session.Credential.UserName, session.Credential.Password, timeout).Result;
                    }
                    else
                    {
                        throw new ArgumentException("The session parameter did not contain a valid cookie and a null credential.", "session");
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("session", "The session object cannot be null.");
            }
        }

        public IBXCommonMethods(string gridMaster, string apiVersion, string username, SecureString password, TimeSpan? timeout = null)
        {
            if (String.IsNullOrEmpty(username) || password == null)
            {
                this._Client = CommandHelpers.BuildHttpClient(gridMaster, apiVersion, timeout).Result;
            }
            else
            {
                this._Client = CommandHelpers.BuildHttpClient(gridMaster, apiVersion, username, password, timeout).Result;
            }
        }

        public async Task<T> SearchIbxObject<T>(SearchType searchType, string searchField, string value)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                return await this.GetAsync<T>(CommandHelpers.BuildGetSearchRequest<T>(searchType, searchField, value));
            }
            else
            {
                throw new ArgumentException(String.Format("The type must be a valid infoblox object type, {0} was provided.", typeof(T).Name));
            }
        }

        public async Task<T> GetIbxObject<T>(string reference)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                if (!String.IsNullOrEmpty(reference))
                {
                    return await this.GetAsync<T>(CommandHelpers.BuildGetRequest<T>(reference));
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

        public async Task<string> NewIbxObject(Type type, string json)
        {
            if (type.IsInfobloxType())
            {
                if (!String.IsNullOrEmpty(json))
                {
                    return await this.PostAsync(type.GetNameAttribute(), json);
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

        public async Task<string> NewIbxObject(dynamic ibxObject)
        {
            if (ibxObject != null)
            {
                if (ExtensionMethods.IsInfobloxType(ibxObject.GetType()))
                {
                    return await this.PostAsync(ExtensionMethods.GetNameAttribute(ibxObject), ExtensionMethods.PrepareObjectForSend(ibxObject));
                }
                else
                {
                    throw new ArgumentException($"The type must be a valid infoblox object type, {ibxObject.GetType().FullName} was provided.");
                }
            }
            else
            {
                throw new ArgumentNullException("ibxObject", "The ibxObject parameter cannot be null.");
            }
        }

        public async Task<string> NewIbxObject(Type type, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (type.IsInfobloxType())
            {
                if (args.Any())
                {
                    return await this.PostAsync(type.GetNameAttribute(), ExtensionMethods.PrepareArgsForSend(type, args));
                }
                else
                {
                    throw new ArgumentNullException("args", "The length of arguments must be greater than 0.");
                }
            }
            else
            {
                throw new ArgumentException($"The type must be a valid infoblox object type, {type.FullName} was provided.");
            }
        }

        public async Task<string> UpdateIbxObject(dynamic ibxObject, bool RemoveEmpty = true)
        {
            if (ExtensionMethods.IsInfobloxType(ibxObject.GetType()))
            {
                if (ibxObject != null)
                {
                    PropertyInfo prop = ibxObject.GetType().GetProperty("_ref");

                    if (prop != null)
                    {
                        string Reference = prop.GetValue(ibxObject) as string;

                        if (!String.IsNullOrEmpty(Reference))
                        {
                            string Json = String.Empty;

                            if (RemoveEmpty)
                            {
                                Json = ExtensionMethods.PrepareObjectForSend(ibxObject);
                            }
                            else
                            {
                                Json = ExtensionMethods.PrepareObjectForSendWithEmptyProperties(ibxObject);
                            }

                            return await this.PutAsync(Reference, Json);
                        }
                        else
                        {
                            throw new ArgumentNullException("ibxObject._ref", "The _ref property cannot be null or empty.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("The updated object must contain a valid _ref property.");
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

        public async Task<string> UpdateIbxObject(Type type, string reference, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (type.IsInfobloxType())
            {
                if (args != null)
                {
                    if (!String.IsNullOrEmpty(reference))
                    {
                        return await this.PutAsync(reference, ExtensionMethods.PrepareArgsForSend(type, args));
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

        public async Task<string> UpdateIbxObject<T>(T ibxObject, bool RemoveEmpty = true) where T : RefObject
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

                            return await this.PutAsync(reference, json);
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

        public async Task<string> UpdateIbxObject<T>(string url, string data)
        {
            if (ExtensionMethods.IsInfobloxType<T>())
            {
                if (!String.IsNullOrEmpty(data))
                {
                    if (!String.IsNullOrEmpty(url))
                    {
                        return await this.PutAsync(url, data);
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

        public async Task<string> DeleteIbxObject(string reference)
        {
            if (!String.IsNullOrEmpty(reference))
            {
                return await this.DeleteAsync(reference);
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
                string Url = url.Replace(":", "%3a");
                Console.WriteLine($"{this._Client.BaseAddress.ToString()}{Url}");

                try
                {
                    HttpResponseMessage Response = await this._Client.GetAsync(Url);
                    return CommandHelpers.ParseGetResponse<T>(Response);
                }
                catch (InfobloxCustomException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw new InfobloxCustomException(e);
                }
            }
            else
            {
                throw new ArgumentNullException("url", "The url string cannot be null or empty.");
            }
        }

        private async Task<string> PostAsync(string url, string data)
        {
            if (!String.IsNullOrEmpty(url))
            {
                if (!String.IsNullOrEmpty(data))
                {
                    string Url = url.Replace(":", "%3a");
                    Console.WriteLine($"{this._Client.BaseAddress.ToString()}{Url}");
                    Console.WriteLine(data);

                    try
                    {
                        return CommandHelpers.ParsePostPutDeleteResponse(await this.Client.PostAsync(Url, new StringContent(data, Encoding.UTF8, "application/json")));
                    }
                    catch (InfobloxCustomException e)
                    {
                        throw e;
                    }
                    catch (Exception e)
                    {
                        throw new InfobloxCustomException(e);
                    }
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
                string Url = url.Replace(":", "%3a");
                Console.WriteLine($"{this._Client.BaseAddress.ToString()}{Url}");
                Console.WriteLine(data);

                try
                {
                    return CommandHelpers.ParsePostPutDeleteResponse(await this._Client.PutAsync(Url, new StringContent(data, Encoding.UTF8, "application/json")));
                }
                catch (InfobloxCustomException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw new InfobloxCustomException(e);
                }
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
                string Url = reference.Replace(":", "%3a");
                Console.WriteLine($"{this._Client.BaseAddress.ToString()}{Url}");

                try
                {
                    return CommandHelpers.ParsePostPutDeleteResponse(await this._Client.DeleteAsync(Url));
                }
                catch (InfobloxCustomException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw new InfobloxCustomException(e);
                }
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

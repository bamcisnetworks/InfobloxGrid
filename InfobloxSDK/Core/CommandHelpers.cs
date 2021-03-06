﻿using BAMCIS.Infoblox.Errors;
using BAMCIS.Infoblox.InfobloxMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BAMCIS.Infoblox.Core
{
    public class CommandHelpers
    {
        //This is used to remove the array brackets from a JSON response from the grid master when we 
        //know that only a single object will be returned for a get request using a reference (as opposed to a search)
        private static readonly char[] _charsToTrim = { '[', ']' };

        public static async Task<HttpClient> BuildHttpClient(string gridMaster, string apiVersion, string username, SecureString password, TimeSpan? timeout = null)
        {
            if (!String.IsNullOrEmpty(gridMaster))
            {
                if (!String.IsNullOrEmpty(apiVersion))
                {
                    if (!String.IsNullOrEmpty(username))
                    {
                        if (password != null)
                        {
                            if (await CommandHelpers.CheckConnection(gridMaster))
                            {
                                HttpClientHandler Handler = new HttpClientHandler();
                                Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                                if (!apiVersion.StartsWith("v"))
                                {
                                    apiVersion = $"v{apiVersion}";
                                }

                                HttpClient Client = new HttpClient(Handler)
                                {
                                    BaseAddress = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/")
                                };

                                //The actual default is 100 seconds
                                if (timeout != null)
                                {
                                    Client.Timeout = (TimeSpan)timeout;
                                }

                                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                                    Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + SecureStringHelper.ToReadableString(password))));

                                return Client;
                            }
                            else
                            {
                                throw new WebException("The grid master could not be contacted.");
                            }
                        }
                        else
                        {
                            throw new ArgumentNullException("password", "The provided password cannot be null.");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException("username", "The username used to connect to the grid master cannot be null or empty.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("apiVersion", "The api version parameter cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("gridMaster", "The value for the grid master cannot be null or empty.");
            }
        }

        public static async Task<HttpClient> BuildHttpClient(string gridMaster, string apiVersion, TimeSpan? timeout = null)
        {
            if (!String.IsNullOrEmpty(gridMaster))
            {
                if (!String.IsNullOrEmpty(apiVersion))
                {
                    if (await CommandHelpers.CheckConnection(gridMaster))
                    {
                        if (InfobloxSessionData.UseSessionData)
                        {
                            if (InfobloxSessionData.Cookie != null && !InfobloxSessionData.Cookie.Expired)
                            {
                                HttpClientHandler Handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };

                                if (!apiVersion.StartsWith("v"))
                                {
                                    apiVersion = $"v{apiVersion}";
                                }

                                Uri Address = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/");

                                Handler.CookieContainer.Add(Address, InfobloxSessionData.Cookie);

                                Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                                HttpClient Client = new HttpClient(Handler)
                                {
                                    BaseAddress = Address
                                };

                                //The actual default is 100 seconds
                                if (timeout != null)
                                {
                                    Client.Timeout = (TimeSpan)timeout;
                                }

                                return Client;
                            }
                            else
                            {
                                throw new Exception("There is not a valid cookie to utilize, you must specify a credential.");
                            }
                        }
                        else
                        {
                            throw new Exception("There is not a valid cookie to utilize, you must specify a credential.");
                        }
                    }
                    else
                    {
                        throw new WebException("The grid master could not be contacted.");
                    }
                }
                else
                {
                    throw new ArgumentNullException("apiVersion", "The api version parameter cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("gridMaster", "The value for the grid master cannot be null or empty.");
            }
        }

        public static async Task<HttpClient> BuildHttpClient(string gridMaster, string apiVersion, Cookie cookie, TimeSpan? timeout = null)
        {
            if (!String.IsNullOrEmpty(gridMaster))
            {
                if (!String.IsNullOrEmpty(apiVersion))
                {
                    if (cookie != null)
                    {
                        if (!cookie.Expired)
                        {
                            if (await CommandHelpers.CheckConnection(gridMaster))
                            {
                                HttpClientHandler Handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };

                                if (!apiVersion.StartsWith("v"))
                                {
                                    apiVersion = $"v{apiVersion}";
                                }

                                Uri Address = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/");

                                Handler.CookieContainer.Add(Address, cookie);

                                Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                                HttpClient Client = new HttpClient(Handler)
                                {
                                    BaseAddress = Address
                                };

                                //The actual default timeout is 100 seconds
                                if (timeout != null)
                                {
                                    Client.Timeout = (TimeSpan)timeout;
                                }

                                return Client;
                            }
                            else
                            {
                                throw new WebException("The grid master could not be contacted.");
                            }
                        }
                        else
                        {
                            throw new ArgumentException($"The provided cookie is expired, it expired on {cookie.Expires.ToUniversalTime().ToString()} UTC");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException("cookie", "Cookie cannot be null");
                    }
                }
                else
                {
                    throw new ArgumentNullException("apiVersion", "The api version parameter cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("gridMaster", "The value for the grid master cannot be null or empty.");
            }
        }

        public static async Task<bool> CheckConnection(string host)
        {
            if (!String.IsNullOrEmpty(host))
            {
                bool pingable = false;
                Ping pinger = new Ping();

                try
                {
                    PingReply reply = await pinger.SendPingAsync(host);
                    pingable = reply.Status == IPStatus.Success;
                    return pingable;
                }
                catch (PingException)
                {
                    return false;
                }
            }
            else
            {
                throw new ArgumentNullException("host", "The host value to ping cannot be null or empty.");
            }
        }

        /// <summary>
        /// Builds a full URL search request including the object type, the query, and the fields to return.
        /// </summary>
        /// <param name="resourceType">An actual infoblox class type.</param> 
        /// <param name="searchType">The type of search to be performed.</param>
        /// <param name="searchField">The field to search on.</param> 
        /// <param name="recordValue">The value to search for.</param> 
        /// <returns></returns>
        internal static string BuildGetSearchRequest(Type resourceType, SearchType searchType, string searchField, string recordValue, IEnumerable<string> fieldsToReturn)
        {
            if (resourceType == null)
            {
                throw new ArgumentNullException("resourceType", "The resource type to get cannot be null.");
            }

            if (String.IsNullOrEmpty(searchField))
            {
                throw new ArgumentNullException("searchField", "The field to search on cannot be null or empty.");
            }

            if (InfobloxSDKExtensionMethods.IsInfobloxType(resourceType))
            {
                if (fieldsToReturn != null && fieldsToReturn.Any())
                {
                    if (fieldsToReturn.Contains("ALL", StringComparer.OrdinalIgnoreCase))
                    {
                        fieldsToReturn = resourceType.GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name);
                    }
                    else if (fieldsToReturn.Contains("BASIC", StringComparer.OrdinalIgnoreCase))
                    {
                        fieldsToReturn = resourceType.GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Where(x => x.IsAttributeDefined<BasicAttribute>()).Select(x => x.Name);
                    }
                }
                else
                {
                    fieldsToReturn = resourceType.GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Where(x => x.IsAttributeDefined<BasicAttribute>()).Select(x => x.Name);
                }

                string Fields = String.Join(",", RefObject.RemoveRefProperty(fieldsToReturn));

                MemberInfo[] Info = typeof(SearchType).GetTypeInfo().GetMember(searchType.ToString());

                if (Info != null && Info.Length > 0)
                {
                    string searchOperator = Info[0].GetCustomAttribute<DescriptionAttribute>().Description;

                    if (searchType.IsSearchTypeAllowed(resourceType, searchField))
                    {
                        return $"{resourceType.GetNameAttribute()}?{searchField.ToLower()}{searchOperator}{recordValue}&_return_fields%2B={Fields}";
                    }
                    else
                    {
                        throw new Exception($"The search type {searchType.ToString()} is not allowed on field {searchField}");
                    }
                }
                else
                {
                    throw new ArgumentException("Reflection evaluation on the search type object failed and no member info could be read.");
                }
            }
            else
            {
                throw new ArgumentException("The resource type must be an infoblox class.");
            }
        }

        /// <summary>
        /// Builds a full URL search request including the object type, the query, and the fields to return
        /// </summary>
        /// <typeparam name="T">An actual infoblox object class type.</typeparam>
        /// <param name="search">The type of search to be performed.</param>
        /// <param name="searchField">The field to search on.</param>
        /// <param name="recordValue">The value to search for.</param>
        /// <returns></returns>
        internal static string BuildGetSearchRequest<T>(SearchType search, string searchField, string recordValue, IEnumerable<string> fieldsToReturn)
        {
            return CommandHelpers.BuildGetSearchRequest(typeof(T), search, searchField, recordValue, fieldsToReturn);
        }

        internal static string BuildGetRequest<T>(string reference, IEnumerable<string> fieldsToReturn)
        {
            if (String.IsNullOrEmpty(reference))
            {
                throw new ArgumentNullException("reference", "The reference value for the object to get cannot be null or empty.");
            }

            if (fieldsToReturn != null && fieldsToReturn.Any())
            {
                if (fieldsToReturn.Contains("ALL", StringComparer.OrdinalIgnoreCase))
                {
                    fieldsToReturn = typeof(T).GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name);
                }
                else if (fieldsToReturn.Contains("BASIC", StringComparer.OrdinalIgnoreCase))
                {
                    fieldsToReturn = typeof(T).GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Where(x => x.IsAttributeDefined<BasicAttribute>()).Select(x => x.Name);
                }
            }
            else
            {
                fieldsToReturn = typeof(T).GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Where(x => x.IsAttributeDefined<BasicAttribute>()).Select(x => x.Name);
            }

            string Fields = String.Join(",", RefObject.RemoveRefProperty(fieldsToReturn));

            //%2B is a plus sign, which indicates the basic fields in addition to the requested fields will be returned
            return $"{reference}?_return_fields%2B={Fields}";
        }

        internal static string ParsePostPutDeleteResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response", "The http response message to parse cannot be null.");
            }

            if (response.IsSuccessStatusCode)
            {
                CommandHelpers.UpdateSessionDataCookieFromResponse(response);

                return response.Content.ReadAsStringAsync().Result.Replace("\"", "");
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        internal static T ParseGetResponse<T>(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response", "The http response message to parse cannot be null.");
            }

            if (response.IsSuccessStatusCode)
            {
                CommandHelpers.UpdateSessionDataCookieFromResponse(response);

                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result.Trim(_charsToTrim));
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        internal static IEnumerable<T> ParseGetSearchResponse<T>(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response", "The http response message to parse cannot be null.");
            }

            if (response.IsSuccessStatusCode)
            {
                CommandHelpers.UpdateSessionDataCookieFromResponse(response);

                return JsonConvert.DeserializeObject<IEnumerable<T>>(response.Content.ReadAsStringAsync().Result);
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        internal static object ParseGetResponse(HttpResponseMessage response, Type type)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response", "The http response message to parse cannot be null.");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type", "The type to parse the response message to cannot be null");
            }

            if (response.IsSuccessStatusCode)
            {
                CommandHelpers.UpdateSessionDataCookieFromResponse(response);

                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.Trim(_charsToTrim), type);
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        public static Cookie GetResponseCookie(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response", "The http response message to get the cookie from cannot be null.");
            }

            if (response.IsSuccessStatusCode)
            {
                IEnumerable<string> Cookies;

                /*
                 * Cookie string
                 * ibapauth="group=admin-group,ctime=1495590270,ip=192.168.1.101,auth=LOCAL,client=API,su=1,timeout=600,mtime=1495591213,user=admin,2CF4756i4DZ3lQ1rBn8t9epWMHYcWZ1TogQ"; httponly; Path=/; secure
                 */
                response.Headers.TryGetValues("Set-Cookie", out Cookies);

                if (Cookies != null && Cookies.Any())
                {
                    string CookieString = Cookies.FirstOrDefault(x => x.StartsWith("ibapauth"));

                    if (!String.IsNullOrEmpty(CookieString))
                    {
                        IEnumerable<string> CookieParts = CookieString.Split(';').Select(x => { return x.Trim(); });

                        if (CookieParts.Any())
                        {
                            if (!String.IsNullOrEmpty(CookieParts.First()))
                            {
                                Cookie Cookie = new Cookie("ibapauth", CookieParts.First().Replace("ibapauth=", ""), "", response.RequestMessage.RequestUri.Host);

                                if (!String.IsNullOrEmpty(CookieParts.FirstOrDefault(x => x.Equals("secure", StringComparison.OrdinalIgnoreCase))))
                                {
                                    Cookie.Secure = true;
                                }

                                if (!String.IsNullOrEmpty(CookieParts.FirstOrDefault(x => x.Equals("httponly", StringComparison.OrdinalIgnoreCase))))
                                {
                                    Cookie.HttpOnly = true;
                                }

                                string[] ValueParts = CookieParts.First().Split(',');
                                string TimeoutString = ValueParts.FirstOrDefault(x => x.StartsWith("timeout=", StringComparison.OrdinalIgnoreCase));
                                string ModifyTimeString = ValueParts.FirstOrDefault(x => x.StartsWith("mtime=", StringComparison.OrdinalIgnoreCase));

                                if (!String.IsNullOrEmpty(TimeoutString) && !String.IsNullOrEmpty(ModifyTimeString))
                                {
                                    try
                                    {
                                        string[] TimeoutParts = TimeoutString.Split('=');

                                        if (TimeoutParts.Length == 2)
                                        {
                                            int Timeout;
                                            if (Int32.TryParse(TimeoutParts[1], out Timeout))
                                            {
                                                string[] ModifyTimeParts = ModifyTimeString.Split('=');

                                                if (ModifyTimeParts.Length == 2)
                                                {
                                                    int Modify;

                                                    if (Int32.TryParse(ModifyTimeParts[1], out Modify))
                                                    {
                                                        Cookie.Expires = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(Modify).AddSeconds(Timeout);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }

                                return Cookie;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static void UpdateSessionDataCookieFromResponse(HttpResponseMessage response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response", "The http response message to use to update the session cookie cannot be null.");
            }

            if (response.IsSuccessStatusCode)
            {
                Cookie Cookie = GetResponseCookie(response);

                if (Cookie != null && !Cookie.Expired)
                {
                    if (InfobloxSessionData.Cookie != null)
                    {
                        if (Cookie.Expires.ToUniversalTime() > InfobloxSessionData.Cookie.Expires.ToUniversalTime())
                        {
                            InfobloxSessionData.Cookie = Cookie;
                        }
                    }
                    else
                    {
                        InfobloxSessionData.Cookie = Cookie;
                    }
                }
            }
        }
    }
}

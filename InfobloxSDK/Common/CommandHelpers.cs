#if (NETSTANDARD2_0 || NETSTANDARD1_6 || NETSTANDARD1_5 || NETSTANDARD1_4 || NETSTANDARD1_3 || NETSTANDARD1_2 || NETSTANDARD1_1 || NETSTANDARD1_0)
#define NETSTANDARD 
#else
#define NET
#endif

using BAMCIS.Infoblox.Errors;
using BAMCIS.Infoblox.InfobloxMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BAMCIS.Infoblox.Common
{
    public class CommandHelpers
    {
        private static readonly char[] _charsToTrim = { '[', ']' };

        public static async Task<HttpClient> BuildHttpClient(string gridMaster, string apiVersion, string username, SecureString password)
        {
            if (await CommandHelpers.CheckConnection(gridMaster))
            {
                HttpClient Client;
                HttpClientHandler Handler = new HttpClientHandler();
                Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                if (!apiVersion.StartsWith("v"))
                {
                    apiVersion = $"v{apiVersion}";
                }

                Client = new HttpClient(Handler)
                {
                    BaseAddress = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/"),
                    Timeout = TimeSpan.FromSeconds(30),

                };

                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + SecureStringHelper.ToReadableString(password))));

                return Client;
            }
            else
            {
                throw new WebException("The grid master could not be contacted.");
            }
        }

        public static async Task<HttpClient> BuildHttpClient(string gridMaster, string apiVersion)
        {
            if (await CommandHelpers.CheckConnection(gridMaster))
            {
                HttpClient Client;

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

                        Client = new HttpClient(Handler)
                        {
                            BaseAddress = Address,
                            Timeout = TimeSpan.FromSeconds(30)
                        };

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

        public static async Task<HttpClient> BuildHttpClient(string gridMaster, string apiVersion, Cookie cookie)
        {
            if (await CommandHelpers.CheckConnection(gridMaster))
            {
                HttpClient Client;

                if (cookie != null)
                {
                    if (!cookie.Expired)
                    {
                        HttpClientHandler Handler = new HttpClientHandler() { CookieContainer = new CookieContainer() };

                        if (!apiVersion.StartsWith("v"))
                        {
                            apiVersion = $"v{apiVersion}";
                        }

                        Uri Address = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/");

                        Handler.CookieContainer.Add(Address, cookie);

                        Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                        Client = new HttpClient(Handler)
                        {
                            BaseAddress = Address,
                            Timeout = TimeSpan.FromSeconds(30)
                        };

                        return Client;
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
                throw new WebException("The grid master could not be contacted.");
            }
        }

        public static async Task<bool> CheckConnection(string host)
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

        /// <summary>
        /// Builds a full URL search request including the object type, the query, and the fields to return.
        /// </summary>
        /// <param name="resourceType">An actual infoblox class type.</param> 
        /// <param name="searchType">The type of search to be performed.</param>
        /// <param name="searchField">The field to search on.</param> 
        /// <param name="recordValue">The value to search for.</param> 
        /// <returns></returns>
        internal static string BuildGetSearchRequest(Type resourceType, SearchType searchType, string searchField, string recordValue)
        {
            if (ExtensionMethods.IsInfobloxType(resourceType))
            {
                IEnumerable<string> FieldsToReturn = resourceType.GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name).ToList();

                FieldsToReturn = RefObject.RemoveRefProperty(FieldsToReturn);

                string Fields = String.Join(",", FieldsToReturn);

                MemberInfo[] Info = typeof(SearchType).GetTypeInfo().GetMember(searchType.ToString());

                if (Info != null && Info.Length > 0)
                {
                    string searchOperator = Info[0].GetCustomAttribute<DescriptionAttribute>().Description;

                    if (SearchableAttribute.SearchTypeAllowed(resourceType, searchType, searchField))
                    {
                        return resourceType.GetNameAttribute() + "?" + searchField.ToLower() + searchOperator + recordValue + "&" + "_return_fields%2B=" + Fields;
                    }
                    else
                    {
                        throw new Exception("The search type " + searchType.ToString() + " is not allowed on field " + searchField);
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
        /// <param name="Search">The type of search to be performed.</param>
        /// <param name="SearchField">The field to search on.</param>
        /// <param name="RecordValue">The value to search for.</param>
        /// <returns></returns>
        internal static string BuildGetSearchRequest<T>(SearchType Search, string SearchField, string RecordValue)
        {
            return CommandHelpers.BuildGetSearchRequest(typeof(T), Search, SearchField, RecordValue);
        }

        internal static string BuildGetRequest<T>(string reference)
        {
            //%2B is a plus sign, which indicates the basic fields in addition to the requested fields will be returned
            return reference + "?_return_fields%2B=" + String.Join(",", RefObject.RemoveRefProperty(
                        typeof(T).GetTypeInfo().GetProperties().Where(x => !x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name).ToList()));
        }

        internal static string ParsePostPutDeleteResponse(HttpResponseMessage response)
        {
            CommandHelpers.ProcessResponseCookies(response);

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsStringAsync().Result.Replace("\"", "");
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        internal static T ParseGetResponse<T>(HttpResponseMessage response)
        {
            CommandHelpers.ProcessResponseCookies(response);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result.Trim(_charsToTrim));
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        internal static object ParseGetResponse(HttpResponseMessage response, Type type)
        {
            CommandHelpers.ProcessResponseCookies(response);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result.Trim(_charsToTrim), type);
            }
            else
            {
                throw new InfobloxCustomException(response);
            }
        }

        private static void ProcessResponseCookies(HttpResponseMessage response)
        {
            if (!CommandHelpers.IsRequestCookieStillValid(response.RequestMessage))
            {
                CommandHelpers.SetCookieFromResponse(response);
            }
        }

        private static string GetCookiePath()
        {
#if NETSTANDARD
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "Microsoft", "Windows", "INetCookies", "infoblox.txt");
            }
            else
            {
                return "/tmp/cookies/infoblox.txt";
            }
#else
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "INetCookies", "infoblox.txt");
#endif
        }

        public static bool DoesValidCookieExist()
        {
            string Path = GetCookiePath();

            if (File.Exists(Path))
            {
                try
                {
                    Cookie cookie = JsonConvert.DeserializeObject<Cookie>(File.ReadAllText(Path));
                    return !cookie.Expired;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }


        private static bool IsRequestCookieStillValid(HttpRequestMessage request)
        {
            IEnumerable<string> RequestCookies;

            if (request.Headers.TryGetValues("Cookie", out RequestCookies))
            {
                if (RequestCookies != null && RequestCookies.Any())
                {
                    string Temp = RequestCookies.FirstOrDefault(x => x.StartsWith("ibapauth"));

                    if (!String.IsNullOrEmpty(Temp))
                    {
                        string[] RequestCookieParts = Temp.Split(';');

                        if (RequestCookieParts.Any())
                        {
                            string Expires = RequestCookieParts.FirstOrDefault(x => x.StartsWith("Expires"));

                            DateTime Date;

                            if (DateTime.TryParse(Expires, out Date))
                            {
                                return DateTime.Now < Date;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static void SetCookieFromResponse(HttpResponseMessage response)
        {
            IEnumerable<string> Cookies;

            response.Headers.TryGetValues("Set-Cookie", out Cookies);

            if (Cookies != null && Cookies.Count() > 0)
            {
                string[] CookieParts = Cookies.FirstOrDefault(x => x.StartsWith("ibapauth")).Split(';');

                if (CookieParts.Any())
                {
                    if (!String.IsNullOrEmpty(CookieParts[0]))
                    {
                        Cookie Cookie = new Cookie("ibapauth", CookieParts[0].Replace("ibapauth=", ""), "", response.RequestMessage.RequestUri.Host)
                        {
                            Secure = true
                        };

                        InfobloxSessionData.Cookie = Cookie;
                    }
                }
            }
        }
    }
}

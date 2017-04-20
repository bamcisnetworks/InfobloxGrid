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
                Uri Address = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/");

                if (CommandHelpers.DoesValidCookieExist())
                {
                    Cookie cookie = CommandHelpers.GetValidCookie();
                    if (cookie != null && !cookie.Expired)
                    {
                        CookieContainer Container = new CookieContainer();
                        Handler.CookieContainer = Container;
                        Client = new HttpClient(Handler);
                        Container.Add(Address, cookie);
                    }
                    else
                    {
                        Client = new HttpClient();
                        Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                            Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + SecureStringHelper.ToReadableString(password))));
                    }
                }
                else
                {
                    Client = new HttpClient();
                    Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + SecureStringHelper.ToReadableString(password))));
                }

                Client.BaseAddress = Address;
                Client.Timeout = TimeSpan.FromSeconds(30);
                Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

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

                if (CommandHelpers.DoesValidCookieExist())
                {
                    Cookie cookie = CommandHelpers.GetValidCookie();
                    if (cookie != null && !cookie.Expired)
                    {
                        CookieContainer Container = new CookieContainer();
                        HttpClientHandler Handler = new HttpClientHandler() { CookieContainer = Container };

                        Client = new HttpClient(Handler);                
                        Client.BaseAddress = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/");
                        Container.Add(Client.BaseAddress, cookie);

                        if (!apiVersion.StartsWith("v"))
                        {
                            apiVersion = $"v{apiVersion}";
                        }

                        Client.BaseAddress = new Uri("https://" + gridMaster + "/wapi/" + apiVersion + "/");
                        Client.Timeout = TimeSpan.FromSeconds(30);

                        Handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

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
        /// <param name="ResourceType">An actual infoblox class type.</param> 
        /// <param name="Search">The type of search to be performed.</param>
        /// <param name="SearchField">The field to search on.</param> 
        /// <param name="RecordValue">The value to search for.</param> 
        /// <returns></returns>
        public static string BuildGetSearchRequest(Type ResourceType, SearchType Search, string SearchField, string RecordValue)
        {
            if (ExtensionMethods.IsInfobloxType(ResourceType))
            {
                IEnumerable<string> fieldsToReturn = ResourceType.GetTypeInfo().GetProperties().Where(x => ! x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name).ToList();

                fieldsToReturn = RefObject.RemoveRefProperty(fieldsToReturn);

                string fields = String.Join(",", fieldsToReturn);

                MemberInfo[] info = typeof(SearchType).GetTypeInfo().GetMember(Search.ToString());
                if (info != null && info.Length > 0)
                {
                    string searchOperator = info[0].GetCustomAttribute<DescriptionAttribute>().Description;

                    if (SearchableAttribute.SearchTypeAllowed(ResourceType, Search, SearchField))
                    {
                        return ResourceType.GetNameAttribute() + "?" + SearchField.ToLower() + searchOperator + RecordValue + "&" + "_return_fields%2B=" + fields;
                    }
                    else
                    {
                        throw new Exception("The search type " + Search.ToString() + " is not allowed on field " + SearchField);
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
        public static string BuildGetSearchRequest<T>(SearchType Search, string SearchField, string RecordValue)
        {
            return CommandHelpers.BuildGetSearchRequest(typeof(T), Search, SearchField, RecordValue);
        }

        public static string BuildGetRequest<T>(string reference)
        {
            //%2B is a plus sign
            return reference + "?_return_fields%2B=" + String.Join(",", RefObject.RemoveRefProperty(
                        typeof(T).GetTypeInfo().GetProperties().Where(x => x.IsAttributeDefined<NotReadableAttribute>()).Select(x => x.Name).ToList()));
        }

        public static string ParsePostPutDeleteResponse(HttpResponseMessage response)
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

        public static T ParseGetResponse<T>(HttpResponseMessage response)
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

        public static object ParseGetResponse(HttpResponseMessage response, Type type)
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

        public static void ProcessResponseCookies(HttpResponseMessage response)
        {
            if (!CommandHelpers.IsRequestCookieStillValid(response.RequestMessage))
            {
                CommandHelpers.SetCookieFromResponse(response);
            }
        }

        private static string GetCookiePath()
        {
#if NETSTANDARD
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
            string path = GetCookiePath();
            if (File.Exists(path))
            {
                try
                {
                    Cookie cookie = JsonConvert.DeserializeObject<Cookie>(File.ReadAllText(path));
                    return !cookie.Expired;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public static Cookie GetValidCookie()
        {
            if (CommandHelpers.DoesValidCookieExist())
            {
                string path = GetCookiePath();
                return JsonConvert.DeserializeObject<Cookie>(File.ReadAllText(path));
            }
            else
            {
                return null;
            }     
        }

        private static bool IsRequestCookieStillValid(HttpRequestMessage request)
        {
            IEnumerable<string> requestCookies;
            if (request.Headers.TryGetValues("Cookie", out requestCookies))
            {
                if (requestCookies != null && requestCookies.Count() > 0)
                {
                    string temp = requestCookies.FirstOrDefault(x => x.StartsWith("ibapauth"));
                    if (!String.IsNullOrEmpty(temp))
                    {
                        string[] requestCookieParts = temp.Split(';');
                        if (requestCookieParts.Count() > 0)
                        {
                            string expires = requestCookieParts.FirstOrDefault(x => x.StartsWith("Expires"));
                            DateTime date;
                            if (DateTime.TryParse(expires, out date))
                            {
                                return DateTime.Now < date;
                            }
                        }
                    }
                }
            }

            return false;
        }

        private static void SetCookieFromResponse(HttpResponseMessage response)
        {
            IEnumerable<string> cookies;
            response.Headers.TryGetValues("Set-Cookie", out cookies);

            if (cookies != null && cookies.Count() > 0)
            {
                string[] cookieParts = cookies.FirstOrDefault(x => x.StartsWith("ibapauth")).Split(';');
                if (cookieParts.Count() > 0)
                {
                    if (!String.IsNullOrEmpty(cookieParts[0]))
                    {
                        Cookie cookie = new Cookie("ibapauth", cookieParts[0].Replace("ibapauth=", ""), "", response.RequestMessage.RequestUri.Host);
                        cookie.Expires = DateTime.Now.AddMinutes(10);
                        cookie.Secure = true;
                        File.WriteAllText(GetCookiePath(), JsonConvert.SerializeObject(cookie));
                    }
                }
            }
        }
    }
}

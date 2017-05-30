using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.Infoblox.Core
{
    public class RefObject
    {
        public string _ref { get; set; }

        public string RemoveRefProperty()
        {
            try
            {
                if (this != null)
                {
                    JObject jobject = JObject.Parse(JsonConvert.SerializeObject(this));
                    jobject.Remove("_ref");
                    return jobject.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string RemoveRefProperty(string input)
        {
            try
            {
                if (!String.IsNullOrEmpty(input))
                {
                    JObject JObj = JObject.Parse(input);
                    JObj.Remove("_ref");
                    return JObj.ToString();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<string> RemoveRefProperty(IEnumerable<string> input)
        {
            if (input != null)
            {
                if (input.Contains("_ref"))
                {
                    List<string> temp = input.ToList();
                    temp.Remove("_ref");
                    input = temp;
                }
            }
               
            return input;
        }
    }
}

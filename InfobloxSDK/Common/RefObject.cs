using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAMCIS.Infoblox.Common
{
    public class RefObject
    {
        public string _ref { get; set; }

        public string RemoveRefProperty()
        {
            try
            {
                JObject jobject = JObject.Parse(JsonConvert.SerializeObject(this));
                jobject.Remove("_ref");
                return jobject.ToString();
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
                JObject jobject = JObject.Parse(input);
                jobject.Remove("_ref");
                return jobject.ToString();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<string> RemoveRefProperty(IEnumerable<string> input)
        {
            if (input.Contains("_ref"))
            {
                List<string> temp = input.ToList();
                temp.Remove("_ref");
                input = temp;
            }
               
            return input;
        }
    }
}

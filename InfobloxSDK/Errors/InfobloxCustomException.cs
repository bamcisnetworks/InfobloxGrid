#if (NETSTANDARD2_0 || NETSTANDARD1_6 || NETSTANDARD1_5 || NETSTANDARD1_4 || NETSTANDARD1_3 || NETSTANDARD1_2 || NETSTANDARD1_1 || NETSTANDARD1_0)
#define NETSTANDARD 
#else
#define NET
#endif

using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace BAMCIS.Infoblox.Errors
{
    public class InfobloxCustomException : Exception
    {
        private string _message;

        public string Text { get; set; }
        public int HttpCode { get; set; }
        public string HttpStatusCode { get; set; }
        public string HttpMessage { get; set; }
        public string Error { get; set; }

        public InfobloxCustomException(HttpResponseMessage response)
        {
            InfobloxError error = JsonConvert.DeserializeObject<InfobloxError>(response.Content.ReadAsStringAsync().Result);

            this._message = error.text;
            this.Error = error.Error.Trim('\r').Trim('\n').Trim('\r');
            this.Text = error.text;
            this.HttpCode = (int)response.StatusCode;
            this.HttpStatusCode = response.StatusCode.ToString();
            this.HttpMessage = response.ReasonPhrase;
        }

        public InfobloxCustomException(string Message)
        {
            this._message = Message;
        }

        public override string Message
        {
            get
            {
                return this._message;
            }
        }
    }
}

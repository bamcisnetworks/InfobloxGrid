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
            string Temp = response.Content.ReadAsStringAsync().Result;

            try
            {
                InfobloxError error = JsonConvert.DeserializeObject<InfobloxError>(Temp);

                this._message = error.text;
                this.Error = error.Error.Trim('\r').Trim('\n').Trim('\r');
                this.Text = error.text;
                this.HttpCode = (int)response.StatusCode;
                this.HttpStatusCode = response.StatusCode.ToString();
                this.HttpMessage = response.ReasonPhrase;
            }
            catch (Exception)
            {
                this._message = Temp;
                this.Text = Temp;
                this.HttpCode = (int)response.StatusCode;
                this.HttpStatusCode = response.StatusCode.ToString();
                this.HttpMessage = response.ReasonPhrase;
            }
        }

        public InfobloxCustomException(Exception e)
        {
            this._message = e.Message;
            this.Text = e.Message;
            this.HttpMessage = e.GetType().FullName;
            this.HttpCode = e.HResult;
            this.HttpStatusCode = e.Source;
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

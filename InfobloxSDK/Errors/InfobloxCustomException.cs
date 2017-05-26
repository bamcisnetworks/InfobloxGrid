using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace BAMCIS.Infoblox.Errors
{
    public class InfobloxCustomException : Exception
    {
        private string _message;

        public string Text { get; private set; }
        public int HttpResponseCode { get; private set; }
        public string HttpStatus { get; private set; }
        public string HttpErrorReason { get; private set; }
        public string Error { get; private set; }

        public InfobloxCustomException(HttpResponseMessage response)
        {
            string Temp = response.Content.ReadAsStringAsync().Result;

            try
            {
                InfobloxError error = JsonConvert.DeserializeObject<InfobloxError>(Temp);

                this._message = error.text;
                this.Error = error.Error.Trim('\r').Trim('\n').Trim('\r');
                this.Text = error.text;
                this.Source = error.code;
            }
            catch (Exception)
            {
                this._message = Temp;
                this.Text = Temp;
                this.Error = Temp;
            }

            this.HttpResponseCode = (int)response.StatusCode;
            this.HttpStatus = response.StatusCode.ToString();
            this.HttpErrorReason = response.ReasonPhrase;
        }

        public InfobloxCustomException(WebException e)
        {
            if (e != null)
            {
                if (e.Response != null)
                {
                    using (StreamReader Reader = new StreamReader(e.Response.GetResponseStream()))
                    {
                        this._message = Reader.ReadToEnd();
                    }

                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        HttpWebResponse Response = (HttpWebResponse)e.Response;

                        if (Response != null)
                        {
                            this.HttpStatus = Response.StatusCode.ToString();
                            this.HttpResponseCode = (int)Response.StatusCode;
                            this.HttpErrorReason = Response.StatusDescription;
                        }
                    }
                    else
                    {
                        this.HttpStatus = e.Status.ToString();
                        this.HttpErrorReason = e.Status.ToString();
                    }
                }
                else
                {
                    this._message = e.Message;
                }

                this.Text = this._message;
                this.Error = e.Status.ToString();
            }
            else
            {
                throw new ArgumentNullException("e", "The WebException parameter cannot be null.");
            }
        }

        public InfobloxCustomException(Exception e)
        {
            this._message = e.Message;
            this.Text = e.Message;
            this.Error = e.HResult.ToString();
            this.HttpErrorReason = e.Message;
            this.HttpResponseCode = 0;
            this.HttpStatus = e.GetType().FullName;
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

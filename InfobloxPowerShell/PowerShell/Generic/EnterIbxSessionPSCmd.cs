using BAMCIS.Infoblox.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
       VerbsCommon.Enter,
       "IBXSession"
   )]
    public class EnterIbxSessionPSCmd : PSCmdlet
    {
        /// <summary>
        /// The grid master to communicate with. This will be used to build the URL string.
        /// </summary>
        [Parameter(
            Position = 0,
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface."
        )]
        [ValidateNotNullOrEmpty()]
        [Alias("ComputerName")]
        public string GridMaster
        {
            get
            {
                return InfobloxSessionData.GridMaster;
            }
            set
            {
                InfobloxSessionData.GridMaster = value;
            }
        }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master."
        )]
        [ValidateNotNull()]
        [System.Management.Automation.Credential()]
        public PSCredential Credential
        {
            get
            {
                return new PSCredential(InfobloxSessionData.Credential.UserName, InfobloxSessionData.Credential.Password);
            }
            set
            {
                InfobloxSessionData.Credential = new InfobloxCredential(value.UserName, value.Password);
            }
        }

        /// <summary>
        /// The API version to specify in the URL path. The function to build the URL string for the
        /// query will add in the leading "v"
        /// </summary>
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST."),
        ]
        [Alias("ApiVersion")]
        [ValidateSet("LATEST", "1.0", "1.1", "1.2", "1.2.1", "1.3", "1.4", "1.4.1", "1.4.2",
            "1.5", "1.6", "1.6.1", "1.7", "1.7.1", "1.7.2", "1.7.3", "1.7.4", "2.0",
            "2.1", "2.1.1", "2.2", "2.2.1", "2.2.2", "2.3")]
        [ValidateNotNullOrEmpty()]
        public string Version
        {
            get
            {
                return InfobloxSessionData.Version;
            }
            set
            {
                InfobloxSessionData.Version = value;
            }
        }

        #region Override Methods

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            try
            {
                if (InfobloxSessionData.Version.Equals("LATEST"))
                {
                    using (HttpClient Client = CommandHelpers.BuildHttpClient(this.GridMaster, "1.0", this.Credential.UserName, this.Credential.Password).Result)
                    {
                        WriteVerbose("Getting supported versions.");

                        HttpResponseMessage Response = Client.GetAsync("?_schema").Result;

                        WriteVerbose(Response.RequestMessage.RequestUri.ToString());

                        if (Response.IsSuccessStatusCode)
                        {
                            string Content = Response.Content.ReadAsStringAsync().Result;

                            WriteVerbose($"Response {Content}");

                            dynamic Obj = JsonConvert.DeserializeObject(Content);
                            IEnumerable<string> Versions = Obj.supported_versions;

                            WriteVerbose("Got versions");

                            Versions = Versions.Select(x => { return new Version(x); }).OrderByDescending(x => x).Select(x => { return x.ToString(); });

                            WriteVerbose("Sorted versions");
                            InfobloxSessionData.Version = Versions.First();
                            WriteVerbose($"Latest supported version is {InfobloxSessionData.Version}");
                        }
                        else
                        {
                            WriteVerbose("Failed to get schema, reverting to using version 2.0");
                            InfobloxSessionData.Version = "2.0";
                        }
                    }
                }

                using (HttpClient Client = CommandHelpers.BuildHttpClient(this.GridMaster, this.Version, this.Credential.UserName, this.Credential.Password).Result)
                {
                    IEnumerable<string> Cookies;

                    HttpResponseMessage Response = Client.GetAsync("grid").Result;
                    Response.Headers.TryGetValues("Set-Cookie", out Cookies);

                    if (Cookies != null && Cookies.Count() > 0)
                    {
                        string[] CookieParts = Cookies.FirstOrDefault(x => x.StartsWith("ibapauth")).Split(';');

                        if (CookieParts.Any())
                        {
                            WriteVerbose("Found an ibapauth cookie.");
                            if (!String.IsNullOrEmpty(CookieParts[0]))
                            {
                                InfobloxSessionData.Cookie = new Cookie("ibapauth", CookieParts[0].Replace("ibapauth=", ""), "", Response.RequestMessage.RequestUri.Host)
                                {
                                    Secure = true
                                };

                                WriteVerbose("Infoblox session data will be used now.");
                                InfobloxSessionData.UseSessionData = true;
                            }
                        }
                    }
                }
            }
            catch (AggregateException ae)
            {
                PSCommon.WriteExceptions(ae, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(ae.Flatten(), ae.GetType().FullName, ErrorCategory.NotSpecified, this));
            }
            catch (Exception e)
            {
                PSCommon.WriteExceptions(e, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(e, e.GetType().FullName, ErrorCategory.NotSpecified, this));
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        #endregion
    }
}

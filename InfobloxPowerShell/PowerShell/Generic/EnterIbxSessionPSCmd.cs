using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Errors;
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
        public string GridMaster { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master."
        )]
        [ValidateNotNull()]
        [System.Management.Automation.Credential()]
        public PSCredential Credential { get; set; }

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
        public string Version { get; set; }

        #region Override Methods

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            InfobloxSessionData.Reset();

            if (this.Version.Equals("LATEST"))
            {
                using (HttpClient Client = CommandHelpers.BuildHttpClient(this.GridMaster, "1.0", this.Credential.UserName, this.Credential.Password).Result)
                {
                    WriteVerbose("Getting supported versions.");

                    try
                    {
                        //Because I'm using .Result instead of await, the exception thrown is an AggregrateException
                        //instead of a normal exception if we'd used await like IBXCommonMethods.GetAsnyc() does
                        //So the resulting InfobloxCustomException isn't as informative, may need to update logic
                        //to use the InvokeGenericAsync() logic being used in PSExtensionMethods to keep ProcessRecord
                        //as a sync function
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
                            this.Version = Versions.First();
                            WriteVerbose($"Latest supported version is {this.Version}");
                        }
                        else
                        {
                            WriteVerbose("Failed to get schema, reverting to using version 2.0");
                            this.Version = "2.0";
                        }
                    }
                    catch (WebException e)
                    {
                        InfobloxCustomException Ex = new InfobloxCustomException(e);
                        PSCommon.WriteExceptions(Ex, this.Host);
                    }
                    catch (HttpRequestException e)
                    {
                        InfobloxCustomException Ex;

                        if (e.InnerException is WebException)
                        {
                            Ex = new InfobloxCustomException((WebException)e.InnerException);
                        }
                        else
                        {
                            Ex = new InfobloxCustomException(e);
                        }

                        PSCommon.WriteExceptions(Ex, this.Host);
                    }
                    catch (Exception e)
                    {
                        InfobloxCustomException Ex = new InfobloxCustomException(e);
                        PSCommon.WriteExceptions(Ex, this.Host);
                    }
                }
            }


            WriteVerbose("Infoblox session data will be used now.");
            InfobloxSessionData.GridMaster = this.GridMaster;
            InfobloxSessionData.Credential = new InfobloxCredential(this.Credential.UserName, this.Credential.Password);
            InfobloxSessionData.Version = this.Version;
            InfobloxSessionData.UseSessionData = true;

            using (HttpClient Client = CommandHelpers.BuildHttpClient(this.GridMaster, this.Version, this.Credential.UserName, this.Credential.Password).Result)
            {
                try
                {
                    HttpResponseMessage Response = Client.GetAsync("grid").Result;
                    Cookie Cookie = CommandHelpers.GetResponseCookie(Response);

                    if (Cookie != null)
                    {
                        WriteVerbose("Infoblox cookie will be used for authentication.");
                        InfobloxSessionData.Cookie = Cookie;
                    }
                    else
                    {
                        WriteWarning("Could not retrieve a valid cookie from the grid master.");
                    }
                }
                catch (WebException e)
                {
                    InfobloxCustomException Ex = new InfobloxCustomException(e);
                    PSCommon.WriteExceptions(Ex, this.Host);
                    this.ThrowTerminatingError(new ErrorRecord(Ex, Ex.HttpStatus, ErrorCategory.NotSpecified, this));
                }
                catch (HttpRequestException e)
                {
                    InfobloxCustomException Ex;

                    if (e.InnerException is WebException)
                    {
                        Ex = new InfobloxCustomException((WebException)e.InnerException);
                    }
                    else
                    {
                        Ex = new InfobloxCustomException(e);
                    }

                    PSCommon.WriteExceptions(Ex, this.Host);

                    this.ThrowTerminatingError(new ErrorRecord(Ex, Ex.HttpStatus, ErrorCategory.NotSpecified, this));
                }
                catch (Exception e)
                {
                    InfobloxCustomException Ex = new InfobloxCustomException(e);
                    PSCommon.WriteExceptions(Ex, this.Host);
                    this.ThrowTerminatingError(new ErrorRecord(Ex, Ex.HttpStatus, ErrorCategory.NotSpecified, this));
                }
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

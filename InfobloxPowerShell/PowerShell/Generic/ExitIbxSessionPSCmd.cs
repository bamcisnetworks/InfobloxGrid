using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.Errors;
using System;
using System.Management.Automation;
using System.Net;
using System.Net.Http;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
       VerbsCommon.Exit,
       "IBXSession"
   )]
    public class ExitIbxSessionPSCmd : PSCmdlet
    {
        #region Override Methods

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (InfobloxSessionData.UseSessionData && InfobloxSessionData.Cookie != null)
            {
                if (!InfobloxSessionData.Cookie.Expired)
                {
                    using (HttpClient Client = CommandHelpers.BuildHttpClient(InfobloxSessionData.GridMaster, InfobloxSessionData.Version).Result)
                    {
                        try
                        {
                            HttpResponseMessage Response = Client.PostAsync("logout", null).Result;

                            if (Response.IsSuccessStatusCode)
                            {
                                WriteVerbose("Successfully ended session.");
                            }
                            else
                            {
                                ThrowTerminatingError(new ErrorRecord(new WebException(Response.StatusCode.ToString()), Response.StatusCode.ToString(), ErrorCategory.InvalidOperation, Client.BaseAddress));
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
                else if (InfobloxSessionData.Credential != null)
                {
                    using (HttpClient Client = CommandHelpers.BuildHttpClient(InfobloxSessionData.GridMaster, InfobloxSessionData.Version, InfobloxSessionData.Credential.UserName, InfobloxSessionData.Credential.Password).Result)
                    {
                        try
                        {
                            HttpResponseMessage Response = Client.PostAsync("logout", null).Result;

                            if (Response.IsSuccessStatusCode)
                            {
                                WriteVerbose("Successfully ended session.");
                            }
                            else
                            {
                                ThrowTerminatingError(new ErrorRecord(new WebException(Response.StatusCode.ToString()), Response.StatusCode.ToString(), ErrorCategory.InvalidOperation, Client.BaseAddress));
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
                else
                {
                    WriteWarning($"No valid authentication methods present to use to logout of Infoblox session to {InfobloxSessionData.GridMaster}");
                }
            }

            InfobloxSessionData.Reset();
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

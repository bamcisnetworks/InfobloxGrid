using BAMCIS.Infoblox.Common;
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
            try
            {
                if (InfobloxSessionData.UseSessionData && InfobloxSessionData.Cookie != null)
                {
                    using (HttpClient Client = CommandHelpers.BuildHttpClient(InfobloxSessionData.GridMaster, InfobloxSessionData.Version).Result)
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
                }

                InfobloxSessionData.Reset();
            }
            catch (AggregateException ae)
            {
                PSCommon.WriteExceptions(ae, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(ae.InnerException, ae.InnerException.GetType().FullName, ErrorCategory.NotSpecified, this));
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

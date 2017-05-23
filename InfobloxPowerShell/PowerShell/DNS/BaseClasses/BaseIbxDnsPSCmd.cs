using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public abstract class BaseIbxDnsPSCmd : BaseIbxObjectPSCmd
    {
        private DnsMethods _ibx;

        public new DnsMethods IBX
        {
            get
            {
                return this._ibx;
            }
            set

            {
                this._ibx = value;
            }
        }

        protected override void BeginProcessing()
        {
            try
            {
                if (this.ParameterSetName.StartsWith(_GRID))
                {
                    this._ibx = new DnsMethods(this.GridMaster, this.Version, this.Credential.UserName, this.Credential.Password);
                }
                else if (this.ParameterSetName.StartsWith(_SESSION))
                {
                    this._ibx = new DnsMethods(this.Session);
                }
                else if (this.ParameterSetName.StartsWith(_ENTERED_SESSION))
                {
                    this._ibx = new DnsMethods();
                }
                else
                {
                    this.ThrowTerminatingError(new ErrorRecord(new PSArgumentException($"Could not identify parameter set from {this.ParameterSetName}"), "PSArgumentException", ErrorCategory.InvalidArgument, this));
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
    }
}

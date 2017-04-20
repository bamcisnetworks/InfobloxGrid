using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Management.Automation;
using System.Security;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public class BaseIbxDnsPSCmd : BaseIbxObjectPSCmd
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
                if (base.Credential == null)
                {
                    this._ibx = new DnsMethods(base.GridMaster, base.Version);
                }
                else
                {
                    this._ibx = new DnsMethods(base.GridMaster, base.Version, base.Credential.UserName, base.Credential.Password);
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

using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.Remove,
        "IBXObject",
        SupportsShouldProcess = true
        )]
    public class RemoveIbxObjectPSCmd : BaseIbxObjectPSCmd
    {
        private bool _force;

        #region Parameters

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "The object reference string to delete.")]
        public string Reference
        {
            get
            {
                return base.Ref;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    base.Ref = value;
                }
                else
                {
                    throw new PSArgumentNullException("Reference", "The reference cannot be null or empty.");
                }
            }
        }

        [Parameter(
            Mandatory = false,
            HelpMessage = "Bypasses the confirmation prompty."
            )
        ]
        public SwitchParameter Force
        {
            get
            {
                return this._force;
            }
            set
            {
                this._force = value;
            }
        }

        #endregion

        #region Override Methods
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            int choice = 0;
            if (!this._force)
            {
                ChoiceDescription yes = new ChoiceDescription("&Yes", "Will delete the object.");
                ChoiceDescription no = new ChoiceDescription("&No", "Will cancel the action.");
                Collection<ChoiceDescription> choices = new Collection<ChoiceDescription>() { yes, no };
                choice = Host.UI.PromptForChoice("Delete Infoblox Object", ("Are you sure you want to delete " + base.Ref + " ?"), choices, 0);
            }

            switch (choice)
            {
                case 0:
                    try
                    {
                        base.Response = base.IBX.DeleteIbxObject(base.Ref);
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
                    break;
                default:
                case 1:
                    WriteWarning("You cancelled the command.");
                    break;
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

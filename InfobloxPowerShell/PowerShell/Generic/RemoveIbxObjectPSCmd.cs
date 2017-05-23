using BAMCIS.Infoblox.Common;
using System;
using System.IO;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.Remove,
        "IBXObject",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High,
        DefaultParameterSetName = _ENTERED_SESSION_REFERENCE
    )]
    public class RemoveIbxObjectPSCmd : ForcePSCmd
    {
        /* Parameters:
         * Base: -GridMaster -Credential -Version -Session [Dynamic Params]
         * InputObject: -InputObject
         * PassThru: -PassThru
         * Force: -Force
         */

        #region Parameters

        /// <summary>
        /// The existing InfobloxSession to use
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_BY_OBJECT
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_REFERENCE
        )]
        [ValidateNotNull()]
        public override InfobloxSession Session
        {
            get
            {
                return base.Session;
            }
            set
            {
                base.Session = value;
            }
        }

        /// <summary>
        /// The grid master to communicate with. This will be used to build the URL string.
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_BY_OBJECT
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [ValidateNotNullOrEmpty()]
        public override string GridMaster
        {
            get
            {
                return base.GridMaster;
            }
            set
            {
                base.GridMaster = value;
            }
        }

        /// <summary>
        /// The API version to specify in the URL path. The function to build the URL string for the
        /// query will add in the leading "v"
        /// </summary>
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_BY_OBJECT
        )]
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [Alias("ApiVersion")]
        [ValidateSet("LATEST", "1.0", "1.1", "1.2", "1.2.1", "1.3", "1.4", "1.4.1", "1.4.2",
            "1.5", "1.6", "1.6.1", "1.7", "1.7.1", "1.7.2", "1.7.3", "1.7.4", "2.0",
            "2.1", "2.1.1", "2.2", "2.2.1", "2.2.2", "2.3")]
        [ValidateNotNullOrEmpty()]
        public override string Version
        {
            get
            {
                return base.Version;
            }
            set
            {
                base.Version = value;
            }
        }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_BY_OBJECT
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [ValidateNotNull()]
        [System.Management.Automation.Credential()]
        public override PSCredential Credential
        {
            get
            {
                return base.Credential;
            }
            set
            {
                base.Credential = value;
            }
        }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The object reference string to delete.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The object reference string to delete.",
            ParameterSetName = _SESSION_REFERENCE
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The object reference string to delete.",
            ParameterSetName = _ENTERED_SESSION_REFERENCE
        )]
        [ValidateNotNullOrEmpty()]
        public string Reference
        {
            get
            {
                return base._Ref;
            }
            set
            {
                base._Ref = value;
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
            //Use base._Ref since the user can provide an input object that sets it
            if (base._Force == true || ShouldProcess($"{base._Ref}", $"Delete"))
            {
                if (base._Force == true || ShouldContinue($"Do you want to delete the IBX Object {base._Ref}?", "Deleting IBX Object"))
                {
                    StringWriter SWriter = new StringWriter();
                    TextWriter OriginalOut = Console.Out;
                    Console.SetOut(SWriter);

                    try
                    {
                        base.Response = base.IBX.DeleteIbxObject(base._Ref).Result;
                        base.FinishResponse(this.PassThru);
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
                    finally
                    {
                        WriteVerbose(SWriter.ToString());
                        Console.SetOut(OriginalOut);
                    }
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

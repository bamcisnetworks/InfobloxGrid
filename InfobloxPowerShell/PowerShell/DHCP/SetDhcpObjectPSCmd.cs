using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.DHCP
{
    [Cmdlet(
        VerbsCommon.Set,
        "IBXDhcpObject",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium,
        DefaultParameterSetName = _ENTERED_SESSION_BY_ATTRIBUTE
    )]
    public class SetDhcpObjectPSCmd : UpdateObjectPSCmd, IDynamicParameters
    {
        /* Parameters:
         * Base: -GridMaster -Version [Dynamic Params]
         * InputObject: -InputObject
         * PassThru: -PassThru
         * Force: -Force
         * UpdateObject: -Reference -ByAttribute -RemoveEmptyProperties
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
            ParameterSetName = _SESSION_BY_ATTRIBUTE
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
            ParameterSetName = _GRID_BY_ATTRIBUTE
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_BY_OBJECT
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
            ParameterSetName = _GRID_BY_ATTRIBUTE
        )]
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_BY_OBJECT
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
            ParameterSetName = _GRID_BY_ATTRIBUTE
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_BY_OBJECT
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

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (this.InputObject == null)
            {
                RuntimeDefinedParameter param = IBXDynamicParameters.DhcpType(true);
                base.ParameterDictionary.Add(param.Name, param);

                string RecordType = this.GetUnboundValue<string>("DhcpType");

                if (!String.IsNullOrEmpty(RecordType))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(RecordType, out base.ObjectType))
                    {
                        foreach (RuntimeDefinedParameter Param in IBXDynamicParameters.ObjectTypeProperties(base.ObjectType, new string[] { _GRID_BY_ATTRIBUTE, _SESSION_BY_ATTRIBUTE, _ENTERED_SESSION_BY_ATTRIBUTE}))
                        {
                            base.ParameterDictionary.Add(Param.Name, Param);
                        }
                    }
                }
            }
            return base.ParameterDictionary;
        }

        #region Override Methods

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (base.Force == true || ShouldProcess("DHCP Object", "Update", "Updating IBX DHCP Object"))
            {
                if (base.Force == true || ShouldContinue("Do you want to update the DHCP object?", "Updating IBX DHCP Object"))
                {
                    switch (this.ParameterSetName)
                    {
                        case _GRID_BY_OBJECT:
                        case _SESSION_BY_OBJECT:
                        case _ENTERED_SESSION_BY_OBJECT:
                            {
                                base.ProcessByUpdatedObject(base.InputObject, this.RemoveEmptyProperties);
                                break;
                            }
                        case _GRID_BY_ATTRIBUTE:
                        case _SESSION_BY_ATTRIBUTE:
                        case _ENTERED_SESSION_BY_ATTRIBUTE:
                            {
                                base.ProcessByAttributeForUpdatedObject("DhcpType");
                                break;
                            }
                        default:
                            {
                                throw new PSArgumentException("Bad parameter set name.");
                            }
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

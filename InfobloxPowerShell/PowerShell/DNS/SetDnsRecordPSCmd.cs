using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(VerbsCommon.Set,
        "IBXDnsRecord",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium,
        DefaultParameterSetName = _ENTERED_SESSION_BY_ATTRIBUTE
    )]
    public class SetDnsRecordPSCmd : BaseIbxDnsForcePSCmd, IDynamicParameters
    {
        /* Parameters:
        * Base: -GridMaster -Version [Dynamic Params]
        * PassThru: -PassThru
        * Force: -Force
        */

        private bool _NextAvailable;
        private string _Network;
        private object _InputObject;
        private bool _RemoveEmpty;

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
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP
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
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP
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
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP
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
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP
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
           HelpMessage = "The dns record reference string."
        )]
        [Alias("Ref")]
        public string Reference
        {
            get
            {
                return base._Ref;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    base._Ref = value;
                }
                else
                {
                    throw new PSArgumentNullException("Reference", "The host record reference cannot be null or empty.");
                }
            }
        }

        [Parameter(
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            Mandatory = true,
            HelpMessage = "Switch to change the IP address of the host to the next available in DHCP."
        )]
        [Parameter(
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            Mandatory = true,
            HelpMessage = "Switch to change the IP address of the host to the next available in DHCP."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            Mandatory = true,
            HelpMessage = "Switch to change the IP address of the host to the next available in DHCP."
        )]
        [Alias("NextAvailable")]
        public SwitchParameter NextAvailableIp
        {
            get
            {
                return this._NextAvailable;
            }
            set
            {
                this._NextAvailable = value;
            }
        }

        [Parameter(
            Mandatory = true,
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            HelpMessage = "The network to select the next available IP from."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "The network to select the next available IP from."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "The network to select the next available IP from."
        )]
        public string Network
        {
            get
            {
                return this._Network;
            }
            set
            {
                if (NetworkAddressTest.IsIPv4Cidr(value) || NetworkAddressTest.IsIPv6Cidr(value))
                {
                    this._Network = value;
                }
                else
                {
                    throw new PSArgumentException("The network must be a valid IPv4 or IPv6 CIDR notation.");
                }
            }
        }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._GRID_BY_OBJECT,
            HelpMessage = "The object to be updated."
        )]
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._SESSION_BY_OBJECT,
            HelpMessage = "The object to be updated."
        )]
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._ENTERED_SESSION_BY_OBJECT,
            HelpMessage = "The object to be updated."
        )]
        public object InputObject
        {
            get
            {
                return this._InputObject;
            }
            set
            {
                if (value != null)
                {
                    Type ObjectType;
                    if (value.GetType() == typeof(PSObject))
                    {
                        PSObject PSObj = (PSObject)value;
                        ObjectType = PSObj.BaseObject.GetType();
                        value = typeof(PSExtensionMethods).GetMethod("ConvertPSObject").MakeGenericMethod(ObjectType).Invoke(typeof(PSExtensionMethods), new object[] { PSObj });
                    }
                    else
                    {
                        ObjectType = value.GetType();
                    }

                    if (ObjectType.IsInfobloxDnsRecordType())
                    {
                        this._InputObject = value;
                    }
                    else
                    {
                        throw new PSArgumentException($"The input object must be a dns record type, {value.GetType().FullName} was provided.");
                    }
                }
                else
                {
                    throw new PSArgumentNullException("InputObject", "The input oject cannot be null or empty.");
                }
            }
        }

        [Parameter(
            Mandatory = false,
            ParameterSetName = BaseIbxObjectPSCmd._GRID_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        [Parameter(
            Mandatory = false,
            ParameterSetName = BaseIbxObjectPSCmd._SESSION_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        [Parameter(
            Mandatory = false,
            ParameterSetName = BaseIbxObjectPSCmd._ENTERED_SESSION_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        public bool RemoveEmptyProperties
        {
            get
            {
                return this._RemoveEmpty;
            }
            set
            {
                this._RemoveEmpty = value;
            }
        }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (this.InputObject == null)
            {
                RuntimeDefinedParameter param = IBXDynamicParameters.RecordType(true);
                base.ParameterDictionary.Add(param.Name, param);

                string RecordType = this.GetUnboundValue<string>("RecordType");

                if (!String.IsNullOrEmpty(RecordType))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(RecordType, out base.ObjectType))
                    {
                        if (this.NextAvailableIp)
                        {
                            IEnumerable<RuntimeDefinedParameter> Temp = IBXDynamicParameters.ObjectTypeProperties(base.ObjectType);
                            string[] fieldsToRemove = new string[] { "ipv4addr", "ipv6addr" };

                            foreach (RuntimeDefinedParameter Param in Temp.Except(Temp.Where(x => fieldsToRemove.Contains(x.Name))))
                            {
                                base.ParameterDictionary.Add(Param.Name, Param);
                            }
                        }
                        else
                        {
                            foreach (RuntimeDefinedParameter Param in IBXDynamicParameters.ObjectTypeProperties(base.ObjectType, new string[] { _GRID_BY_ATTRIBUTE, _SESSION_BY_ATTRIBUTE, _ENTERED_SESSION_BY_ATTRIBUTE }))
                            {
                                base.ParameterDictionary.Add(Param.Name, Param);
                            }
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
            if (base.Force == true || ShouldProcess("DNS Record", "Update", "Updating IBX DNS Object"))
            {
                if (base.Force == true || ShouldContinue("Do you want to update the DNS Record?", "Updating IBX DNS Object"))
                {
                    switch (this.ParameterSetName)
                    {
                        case _GRID_BY_OBJECT:
                        case _SESSION_BY_OBJECT:
                        case _ENTERED_SESSION_BY_OBJECT:
                            {
                                base.ProcessByUpdatedObject(this._InputObject, this.RemoveEmptyProperties);
                                break;
                            }
                        case _GRID_BY_ATTRIBUTE:
                        case _SESSION_BY_ATTRIBUTE:
                        case _ENTERED_SESSION_BY_ATTRIBUTE:
                        case _GRID_NEXT_AVAILABLE_IP:
                        case _SESSION_NEXT_AVAILABLE_IP:
                        case _ENTERED_SESSION_NEXT_AVAILABLE_IP:
                            {
                                base.ProcessByAttributeForUpdatedObject("RecordType");
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

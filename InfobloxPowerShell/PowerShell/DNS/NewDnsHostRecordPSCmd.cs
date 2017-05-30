using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.InfobloxObjects.DNS;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.IO;
using System.Management.Automation;
using System.Net;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.New,
        "IBXDnsHostRecord",
        DefaultParameterSetName = _ENTERED_SESSION_SPECIFY_IP
    )]
    public class NewDnsHostRecordPSCmd : BaseIbxDnsPassThruPSCmd, IDynamicParameters
    {
        /* Parameters:
         * Base: -GridMaster -Version [Dynamic Params]
         * PassThru: -PassThru
         */

        private string _HostName;
        private string _IPAddress = String.Empty;
        private bool _AddToDns = true;
        private string _MAC = String.Empty;
        private bool _DHCP = false;
        private host _InputObject;
        private bool _NextAvailable = false;
        private bool _SetHostName = false;
        private string _Network = String.Empty;

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
            ParameterSetName = _SESSION_SPECIFY_IP
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
            ParameterSetName = _GRID_BY_OBJECT
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_SPECIFY_IP
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
            ParameterSetName = _GRID_BY_OBJECT
        )]
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_SPECIFY_IP
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
            ParameterSetName = _GRID_BY_OBJECT
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_SPECIFY_IP
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
            ParameterSetName = _GRID_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The new dns host record name, must be a valid FQDN."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The new dns host record name, must be a valid FQDN."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The new dns host record name, must be a valid FQDN."
        )]
        [Parameter(
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            Mandatory = true,
            HelpMessage = "The new dns host record name, must be a valid FQDN."
        )]
        [Parameter(
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            Mandatory = true,
            HelpMessage = "The new dns host record name, must be a valid FQDN."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            Mandatory = true,
            HelpMessage = "The new dns host record name, must be a valid FQDN."
        )]
        [Alias("FQDN")]
        public string HostName
        {
            get
            {
                return this._HostName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    NetworkAddressTest.IsFqdn(value, "HostName", out this._HostName, false, true);
                }
                else
                {
                    throw new PSArgumentNullException("HostName", "The host record cannot be null or empty.");
                }
            }
        }

        [Parameter(
            ParameterSetName = _GRID_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The IP address of the new host record."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The IP address of the new host record."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The IP address of the new host record."
        )]
        [Alias("IPAddress")]
        public string IP
        {
            get
            {
                return this._IPAddress;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (IPAddress.TryParse(value, out IPAddress ip))
                    {
                        this._IPAddress = value;
                    }
                    else
                    {
                        throw new FormatException("IP could not be parsed into a valid IP address.");
                    }
                }
                else
                {
                    throw new PSArgumentNullException("IP", "IP cannot be null or empty.");
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
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._GRID_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "A host record object."
        )]
        [Parameter(
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._SESSION_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "A host record object."
        )]
        [Parameter(
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._ENTERED_SESSION_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "A host record object."
        )]
        [Alias("HostObject")]
        public host InputObject
        {
            get
            {
                return this._InputObject;
            }
            set
            {
                if (value != null)
                {
                    this._InputObject = value;
                }
                else
                {
                    throw new PSArgumentNullException("InputObject", "Host object cannot be null.");
                }
            }
        }

        [Parameter(
            ParameterSetName = _GRID_SPECIFY_IP,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SPECIFY_IP,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SPECIFY_IP,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
        )]
        [Parameter(
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
        )]
        [Parameter(
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
        )]
        [Alias("DoNotAddToDns")]
        public SwitchParameter NoDns
        {
            get
            {
                return this._AddToDns;
            }
            set
            {
                this._AddToDns = !value;
            }
        }

        [Parameter(
            ParameterSetName = _GRID_SPECIFY_IP,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SPECIFY_IP,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SPECIFY_IP,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Alias("DHCP")]
        public SwitchParameter EnableDHCP
        {
            get
            {
                return this._DHCP;
            }
            set
            {
                this._DHCP = value;
            }
        }

        [Parameter(
            Mandatory = true,
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            HelpMessage = "Specify the network the next available IP should come from, in FQDN/CIDR notation, like 192.168.0.1/24."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "Specify the network the next available IP should come from, in FQDN/CIDR notation, like 192.168.0.1/24."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            HelpMessage = "Specify the network the next available IP should come from, in FQDN/CIDR notation, like 192.168.0.1/24."
        )]
        public string Network
        {
            get
            {
                return this._Network;
            }
            set
            {
                this._Network = value;
            }
        }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (this._DHCP)
            {
                RuntimeDefinedParameter dhcp = IBXDynamicParameters.SetHostNameInDhcp();
                RuntimeDefinedParameter mac = IBXDynamicParameters.MAC(true);

                base.ParameterDictionary.Add(dhcp.Name, dhcp);
                base.ParameterDictionary.Add(mac.Name, mac);
            }
            else
            {
                
                if (this.InputObject == null )
                {
                    RuntimeDefinedParameter mac = IBXDynamicParameters.MAC();
                    base.ParameterDictionary.Add(mac.Name, mac);
                }
            }

            return base.ParameterDictionary;
        }

        #region Override Parameters
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (this._DHCP)
            {
                if (base.ParameterDictionary.ContainsKey("SetHostNameInDhcp") && base.ParameterDictionary["SetHostNameInDhcp"].IsSet)
                {
                    SwitchParameter temp = (SwitchParameter)base.ParameterDictionary["SetHostNameInDhcp"].Value;
                    this._SetHostName = temp.ToBool();
                }
            }

            if (base.ParameterDictionary.ContainsKey("MAC") && base.ParameterDictionary["MAC"].IsSet)
            {
                this._MAC = base.ParameterDictionary["MAC"].Value as string;
            }

            switch (this.ParameterSetName)
            {
                case _GRID_SPECIFY_IP:
                case _SESSION_SPECIFY_IP:
                case _ENTERED_SESSION_SPECIFY_IP:
                    {
                        ProcessByIp();
                        break;
                    }
                case _GRID_NEXT_AVAILABLE_IP:
                case _SESSION_NEXT_AVAILABLE_IP:
                case _ENTERED_SESSION_NEXT_AVAILABLE_IP:
                    {
                        ProcessByNextAvailableIp();
                        break;
                    }
                case _GRID_BY_OBJECT:
                case _SESSION_BY_OBJECT:
                case _ENTERED_SESSION_BY_OBJECT:
                    {
                        base.ProcessByNewObject(this.InputObject);
                        break;
                    }
                default:
                    {
                        throw new PSArgumentException("Bad ParameterSet Name");
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

        #region Helper Methods

        private void ProcessByIp()
        {
            StringWriter SWriter = new StringWriter();
            TextWriter OriginalOut = Console.Out;
            Console.SetOut(SWriter);

            try
            {
                base.Response = base.IBX.NewDnsHostRecord(this._HostName, this._IPAddress, this._AddToDns, this._DHCP, this._SetHostName, this._MAC).Result;
                base.FinishResponse(this.PassThru);
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
            finally
            {
                WriteVerbose(SWriter.ToString());
                Console.SetOut(OriginalOut);
            }
        }

        private void ProcessByNextAvailableIp()
        {
            StringWriter SWriter = new StringWriter();
            TextWriter OriginalOut = Console.Out;
            Console.SetOut(SWriter);

            try
            {
                base.Response = base.IBX.NewDnsHostRecordWithNextAvailableIP(this._HostName, this._Network, this._AddToDns, this._DHCP, this._SetHostName, this._MAC).Result;
                base.FinishResponse(this.PassThru);
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
            finally
            {
                WriteVerbose(SWriter.ToString());
                Console.SetOut(OriginalOut);
            }
        }

        #endregion  
    }
}

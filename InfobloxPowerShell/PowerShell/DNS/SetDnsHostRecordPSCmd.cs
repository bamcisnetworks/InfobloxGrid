using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxObjects.DNS;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Management.Automation;
using System.IO;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.Set,
        "IBXDnsHostRecord",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium,
        DefaultParameterSetName = _ENTERED_SESSION_HOSTNAME
    )]
    public class SetDnsHostRecordPSCmd : BaseIbxDnsForcePSCmd, IDynamicParameters
    {
        /* Parameters:
         * Base: -GridMaster -Version [Dynamic Params]
         * PassThru: -PassThru
         * Force: -Force
         */

        private string _HostRecord;
        private string _IPAddress = String.Empty;
        private string _Network = String.Empty;
        private bool _AddToDns = true;
        private string _MAC = String.Empty;
        private bool _NextAvailable;
        private bool _DHCP = false;
        private host _InputObject;
        private bool _SetHostName = false;
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
            ParameterSetName = _SESSION_SPECIFY_IP
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_HOSTNAME
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
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_HOSTNAME
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
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_HOSTNAME
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
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_HOSTNAME
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
            HelpMessage = "The dns host record reference string."
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
            ParameterSetName = _GRID_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The new IP address of the host record."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The new IP address of the host record."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SPECIFY_IP,
            Mandatory = true,
            HelpMessage = "The new IP address of the host record."
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
                    NetworkAddressTest.isIPWithException(value, out this._IPAddress);
                }
                else
                {
                    throw new PSArgumentNullException("IP", "IP cannot be null or empty.");
                }
            }
        }

        [Parameter(
            ParameterSetName = _GRID_HOSTNAME,
            Mandatory = true,
            HelpMessage = "The new name of the host."
        )]
        [Parameter(
            ParameterSetName = _SESSION_HOSTNAME,
            Mandatory = true,
            HelpMessage = "The new name of the host."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_HOSTNAME,
            Mandatory = true,
            HelpMessage = "The new name of the host."
        )]
        [Alias("Host")]
        public string HostName
        {
            get
            {
                return this._HostRecord;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this._HostRecord = value;
                }
                else
                {
                    throw new PSArgumentNullException("HostName", "Host record name cannot be null or empty.");
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
            ParameterSetName = _GRID_SPECIFY_IP,
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SPECIFY_IP,
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SPECIFY_IP,
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _GRID_NEXT_AVAILABLE_IP,
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _SESSION_NEXT_AVAILABLE_IP,
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_NEXT_AVAILABLE_IP,
            Mandatory = false,
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
            ParameterSetName = _GRID_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "The host object to with updates to be applied to the host record."
        )]
        [Parameter(
            ParameterSetName = _SESSION_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "The host object to with updates to be applied to the host record."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "The host object to with updates to be applied to the host record."
        )]
        [Alias("HostRecordObject")]
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
                    throw new ArgumentNullException("InputObject", "The host record object cannot be null.");
                }
            }
        }

        [Parameter(
            ParameterSetName = BaseIbxObjectPSCmd._GRID_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        [Parameter(
            ParameterSetName = BaseIbxObjectPSCmd._SESSION_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        [Parameter(
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

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (this._DHCP)
            {
                RuntimeDefinedParameter SetHostName = IBXDynamicParameters.SetHostNameInDhcp();
                RuntimeDefinedParameter MAC = IBXDynamicParameters.MAC(true);

                base.ParameterDictionary.Add(SetHostName.Name, SetHostName);
                base.ParameterDictionary.Add(MAC.Name, MAC);
            }
            else
            {
                if (this.InputObject == null  && String.IsNullOrEmpty(this.HostName))
                {
                    RuntimeDefinedParameter MAC = IBXDynamicParameters.MAC();
                    base.ParameterDictionary.Add(MAC.Name, MAC);
                }
            }

            if (this._NextAvailable)
            {
                RuntimeDefinedParameter Network = IBXDynamicParameters.Network();
                base.ParameterDictionary.Add(Network.Name, Network);
            }

            return base.ParameterDictionary;
        }

        #region Override Methods
        protected override void ProcessRecord()
        {
            if (base.ParameterDictionary.ContainsKey("SetHostName"))
            {
                this._SetHostName = ((SwitchParameter)base.ParameterDictionary["SetHostName"].Value).ToBool();
            }

            if (base.ParameterDictionary.ContainsKey("Network"))
            {
                this._Network = base.ParameterDictionary["Network"].Value as string;
            }

            if (base.ParameterDictionary.ContainsKey("MAC"))
            {
                this._MAC = base.ParameterDictionary["MAC"].Value as string;
            }

            if (base._Force == true || ShouldProcess("DNS Host Record", "Update", "Updating IBX DNS Object"))
            {
                if (base._Force == true || ShouldContinue("Do you want to update the DNS Host Record?", "Updating IBX DNS Object"))
                {
                    if (this.ParameterSetName.EndsWith(_SPECIFY_IP))
                    {
                        StringWriter SWriter = new StringWriter();
                        TextWriter OriginalOut = Console.Out;
                        Console.SetOut(SWriter);

                        try
                        {
                            base.Response = base.IBX.SetDnsHostRecordIP(base._Ref, this._IPAddress, this._DHCP, this._SetHostName, this._MAC).Result;
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
                    else if (this.ParameterSetName.EndsWith(_HOST_NAME))
                    {
                        StringWriter SWriter = new StringWriter();
                        TextWriter OriginalOut = Console.Out;
                        Console.SetOut(SWriter);

                        try
                        {
                            base.Response = base.IBX.SetDnsHostRecordName(base._Ref, this._HostRecord).Result;
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
                    else if (this.ParameterSetName.EndsWith(_NEXT_AVAILABLE_IP))
                    {
                        StringWriter SWriter = new StringWriter();
                        TextWriter OriginalOut = Console.Out;
                        Console.SetOut(SWriter);

                        try
                        {
                            base.Response = base.IBX.SetDnsHostRecordNextAvailableIP(base._Ref, this._Network, this._DHCP, this._SetHostName, this._MAC).Result;
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
                    else if (this.ParameterSetName.EndsWith(_BY_OBJECT))
                    {

                        base.ProcessByUpdatedObject(this._InputObject, this.RemoveEmptyProperties);
                    }
                    else
                    {
                        throw new PSArgumentException("Bad ParameterSet Name");
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

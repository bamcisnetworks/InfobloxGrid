using BAMCIS.Infoblox.Common;
using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.Add,
        "IBXIPAddressToHostRecord",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.Medium,
        DefaultParameterSetName = _ENTERED_SESSION_SINGLE_IP
    )]
    public class AddIPAddressToDnsHostRecordPSCmd : BaseIbxDnsForcePSCmd, IDynamicParameters
    {
        /* Parameters:
         * Base: -GridMaster -Version -Credential -Session [Dynamic Params]
         * PassThru: -PassThru
         * Force: -Force
         */

        private string _IP = String.Empty;
        private string[] _IPs = new string[0];
        private bool _NextAvailable = false;
        private string _Network = String.Empty;
        private bool _DHCP = false;
        private string _MAC = String.Empty;
        private bool _SetHostName = false;

        #region Parameters

        /// <summary>
        /// The existing InfobloxSession to use
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_SINGLE_IP
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_MULTIPLE_IP
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
            ParameterSetName = _GRID_SINGLE_IP
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_MULTIPLE_IP
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
            ParameterSetName = _GRID_SINGLE_IP
        )]
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_MULTIPLE_IP
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
            ParameterSetName = _GRID_SINGLE_IP
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_MULTIPLE_IP
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
            HelpMessage = "The dns host record reference string."
        )]
        [ValidateNotNullOrEmpty()]
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
            Mandatory = true,
            ParameterSetName = _GRID_SINGLE_IP,
            HelpMessage = "Add a single IP to a dns host record."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _SESSION_SINGLE_IP,
            HelpMessage = "Add a single IP to a dns host record."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _ENTERED_SESSION_SINGLE_IP,
            HelpMessage = "Add a single IP to a dns host record."
        )]
        [ValidateNotNullOrEmpty()]
        [Alias("IPAddress")]
        public string IP
        {
            get
            {
                return this._IP;
            }
            set
            {
                this._IP = IPAddress.Parse(value).ToString();
            }
        }

        [Parameter(
            Mandatory = true,
            ParameterSetName = _GRID_MULTIPLE_IP,
            HelpMessage = "An array of IP Addresses to add to a dns host record."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _SESSION_MULTIPLE_IP,
            HelpMessage = "An array of IP Addresses to add to a dns host record."
        )]
        [Parameter(
            Mandatory = true,
            ParameterSetName = _ENTERED_SESSION_MULTIPLE_IP,
            HelpMessage = "An array of IP Addresses to add to a dns host record."
        )]
        [Alias("IPAddresses")]
        public string[] IPs
        {
            get
            {
                return this._IPs;
            }
            set
            {
                if (value.Length > 0)
                {
                    this._IPs = value;
                }
                else
                {
                    throw new PSArgumentException("Array of IP Addresses must contain members.");
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
                if (!String.IsNullOrEmpty(value))
                {
                    this._Network = value;
                }
                else
                {
                    throw new PSArgumentNullException("Network", "Network cannot be null or empty.");
                }
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
                if (this.IPs != null && this.IPs.Any())
                {
                    RuntimeDefinedParameter mac = IBXDynamicParameters.MAC();
                    base.ParameterDictionary.Add(mac.Name, mac);
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

            StringWriter SWriter = new StringWriter();
            TextWriter OriginalOut = Console.Out;
            Console.SetOut(SWriter);

            if (base.Force == true || ShouldProcess("DNS Host Record", "Add IP Address", "Updating IBX DNS Object"))
            {
                if (base.Force == true || ShouldContinue("Do you want to add the IP Address(es) to the DNS Host Record?", "Updating IBX DNS Object"))
                {
                    if (this.ParameterSetName.EndsWith(_SINGLE_IP))
                    {
                        try
                        {
                            base.Response = base.IBX.AddDnsHostRecordIPAddress(base._Ref, this._IP, this._DHCP, this._SetHostName, this._MAC).Result;
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
                    else if (this.ParameterSetName.EndsWith(_GRID_MULTIPLE_IP))
                    {
                        try
                        {
                            base.Response = base.IBX.AddDnsHostRecordIPAddresses(base._Ref, this._IPs).Result;
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
                        try
                        {
                            base.Response = base.IBX.AddDnsHostRecordNextAvailableIPAddress(base._Ref, this._Network, this._DHCP, this._SetHostName, this._MAC).Result;
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

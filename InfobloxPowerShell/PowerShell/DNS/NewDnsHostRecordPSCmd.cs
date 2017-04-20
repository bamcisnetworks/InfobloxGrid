using System;
using System.Management.Automation;
using System.Net;
using BAMCIS.Infoblox.InfobloxObjects.DNS;
using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.Common;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.New,
        "IBXDnsHostRecord",
        SupportsShouldProcess = true
        )
    ]
    public class NewDnsHostRecordPSCmd : BaseIbxDnsPSCmd, IDynamicParameters
    {
        private string _hostName;
        private string _ipAddress = String.Empty;
        private bool _addToDns = true;
        private string _mac = String.Empty;
        private bool _dhcp = false;
        private host _host;
        private bool _nextAvailable = false;
        private bool _setHostName = false;
        private string _network = String.Empty;

        #region Parameters

        [Parameter(
           Position = 1,
           ParameterSetName = "SpecifyIp",
           ValueFromPipelineByPropertyName = true,
           Mandatory = true,
           HelpMessage = "The new dns host record name, must be a valid FQDN.")
        ]
        [Parameter(
          Position = 1,
          ParameterSetName = "NextAvailableIp",
          ValueFromPipelineByPropertyName = true,
          Mandatory = true,
          HelpMessage = "The new dns host record name, must be a valid FQDN.")
        ]
        [Alias("FQDN")]
        public string HostName
        {
            get
            {
                return this._hostName;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    NetworkAddressTest.IsFqdnWithException(value, "HostName", out this._hostName);
                }
                else
                {
                    throw new PSArgumentNullException("HostRecord", "The host record cannot be null or empty.");
                }
            }
        }

        [Parameter(
            Position = 2,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "SpecifyIp",
            Mandatory = true,
            HelpMessage = "The IP address of the new host record."
            )
        ]
        [Alias("IPAddress")]
        public string IP
        {
            get
            {
                return this._ipAddress;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    if (IPAddress.TryParse(value, out IPAddress ip))
                    {
                        this._ipAddress = value;
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
            Position = 2,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "NextAvailableIp",
            Mandatory = true,
            HelpMessage = "Switch to change the IP address of the host to the next available in DHCP."
            )
        ]
        [Alias("NextAvailable")]
        public SwitchParameter NextAvailableIp
        {
            get
            {
                return this._nextAvailable;
            }
            set
            {
                this._nextAvailable = value;
            }
        }

        [Parameter(
            ValueFromPipeline = true,
            Position = 2,
            ParameterSetName = "ByObject",
            Mandatory = true,
            HelpMessage = "A host record object."
            )
        ]
        [Alias("HostObject")]
        public host InputObject
        {
            get
            {
                return this._host;
            }
            set
            {
                if (value != null)
                {
                    this._host = value;
                }
                else
                {
                    throw new PSArgumentNullException("InputObject", "Host object cannot be null.");
                }
            }
        }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "SpecifyIp",
            Mandatory = false,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
            )
        ]
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "NextAvailableIp",
            Mandatory = false,
            HelpMessage = "Switch to not add the host to dns, meaning that infoblox is not authoritative for the zone."
            )
        ]
        [Alias("DoNotAddToDns")]
        public SwitchParameter NoDns
        {
            get
            {
                return this._addToDns;
            }
            set
            {
                this._addToDns = !value;
            }
        }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "SpecifyIp",
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
            )
        ]
        [Parameter(
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "NextAvailableIp",
            Mandatory = false,
            HelpMessage = "Switch to enable the IP address in the DHCP scope. This works both for the next available IP as well as a defined IP. Must also use MAC address if this is set."
            )
        ]
        [Alias("DHCP")]
        public SwitchParameter EnableDHCP
        {
            get
            {
                return this._dhcp;
            }
            set
            {
                this._dhcp = value;
            }
        }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            ParameterSetName = "NextAvailableIp",
            HelpMessage = "Specify the network the next available IP should come from, in FQDN/CIDR notation, like 192.168.0.1/24."
            )
        ]
        public string Network
        {
            get
            {
                return this._network;
            }
            set
            {
                this._network = value;
            }
        }

        #endregion

        #region Override Parameters
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            if (this._dhcp)
            {
                if (base.ParameterDictionary.ContainsKey("SetHostNameInDhcp") && base.ParameterDictionary["SetHostNameInDhcp"].IsSet)
                {
                    SwitchParameter temp = (SwitchParameter)base.ParameterDictionary["SetHostNameInDhcp"].Value;
                    this._setHostName = temp.ToBool();
                }
            }

            if (base.ParameterDictionary.ContainsKey("MAC") && base.ParameterDictionary["MAC"].IsSet)
            {
                this._mac = base.ParameterDictionary["MAC"].Value as string;
            }

            switch (this.ParameterSetName)
            {
                case "SpecifyIp":
                    ProcessByIp();
                    break;
                case "NextAvailableIp":
                    ProcessByNextAvailableIp();
                    break;
                case "ByObject":
                    ProcessByObject();
                    break;
                default:
                    throw new PSArgumentException("Bad ParameterSet Name");
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
            try
            {
                base.Response = base.IBX.NewDnsHostRecord(this._hostName, this._ipAddress, this._addToDns, this._dhcp, this._setHostName, this._mac);
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

        private void ProcessByNextAvailableIp()
        {
            try
            {
                base.Response = base.IBX.NewDnsHostRecordWithNextAvailableIP(this._hostName, this._network, this._addToDns, this._dhcp, this._setHostName, this._mac);
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

        private void ProcessByObject()
        {
            try
            {
                base.Response = base.IBX.NewIbxObject(this._host);
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

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (this._dhcp)
            {
                RuntimeDefinedParameter dhcp = IBXDynamicParameters.SetHostNameInDhcp();
                RuntimeDefinedParameter mac = IBXDynamicParameters.MAC(true);

                base.ParameterDictionary.Add(dhcp.Name, dhcp);
                base.ParameterDictionary.Add(mac.Name, mac);
            }
            else
            {
                if (!this.ParameterSetName.Equals("ByObject"))
                {
                    RuntimeDefinedParameter mac = IBXDynamicParameters.MAC();
                    base.ParameterDictionary.Add(mac.Name, mac);
                }
            }

            return base.ParameterDictionary;
        }
    }
}

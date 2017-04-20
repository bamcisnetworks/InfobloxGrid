using System;
using System.Management.Automation;
using BAMCIS.Infoblox.InfobloxObjects.DNS;
using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System.Management.Automation.Host;
using System.Collections.ObjectModel;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
       VerbsCommon.Set,
       "IBXDnsHostRecord",
       SupportsShouldProcess = true
       )
    ]
    public class SetDnsHostRecordPSCmd : BaseIbxDnsPSCmd, IDynamicParameters
    {
        private string _hostRecord;
        private string _ipAddress = String.Empty;
        private string _network = String.Empty;
        private bool _addToDns = true;
        private string _mac = String.Empty;
        private bool _nextAvailable;
        private bool _dhcp = false;
        private host _hostObject;
        private bool _setHostName = false;
        private bool _removeEmpty;

        #region Parameters

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            HelpMessage = "The dns host record reference string.")
        ]
        [Alias("Ref")]
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
                    throw new PSArgumentNullException("Reference", "The host record reference cannot be null or empty.");
                }
            }
        }

        [Parameter(
           Position = 2,
           ValueFromPipelineByPropertyName = true,
           ParameterSetName = "SpecifyIp",
           Mandatory = true,
           HelpMessage = "The new IP address of the host record."
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
                    NetworkAddressTest.isIPWithException(value, out this._ipAddress);
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
            ParameterSetName = "HostName",
            Mandatory = true,
            HelpMessage = "The new name of the host."
            )
        ]
        [Alias("Host")]
        public string HostName
        {
            get
            {
                return this._hostRecord;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this._hostRecord = value;
                }
                else
                {
                    throw new PSArgumentNullException("HostName", "Host record name cannot be null or empty.");
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
            ParameterSetName = "ByObject",
            Mandatory = true,
            HelpMessage = "The host object to with updates to be applied to the host record."
            )
        ]
        [Alias("HostRecordObject")]
        public host InputObject
        {
            get
            {
                return this._hostObject;
            }
            set
            {
                if (value != null)
                {
                    this._hostObject = value;
                }
                else
                {
                    throw new ArgumentNullException("InputObject", "The host record object cannot be null.");
                }
            }
        }
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "ByObject",
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
            )]
        public bool RemoveEmptyProperties
        {
            get
            {
                return this._removeEmpty;
            }
            set
            {
                this._removeEmpty = value;
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

        #endregion

        #region Override Methods
        protected override void ProcessRecord()
        {
            if (base.ParameterDictionary.ContainsKey("SetHostName"))
            {
                SwitchParameter temp = (SwitchParameter)base.ParameterDictionary["SetHostName"].Value;
                this._setHostName = temp.ToBool();
            }

            if (base.ParameterDictionary.ContainsKey("Network"))
            {
                this._network = base.ParameterDictionary["Network"].Value as string;
            }

            if (base.ParameterDictionary.ContainsKey("MAC"))
            {
                this._mac = base.ParameterDictionary["MAC"].Value as string;
            }

            switch (this.ParameterSetName)
            {
                case "SpecifyIp":
                    ProcessByIp();
                    break;
                case "HostName":
                    ProcessByHostName();
                    break;
                case "NextAvailableIp":
                    ProcessByNextAvailableIp();
                    break;
                case "ByObject":
                    ProcessByObject();
                    break;
                default:
                    throw new ArgumentException("Bad ParameterSet Name");
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
                base.Response = base.IBX.SetDnsHostRecordIP(base.Ref, this._ipAddress, this._dhcp, this._setHostName, this._mac);
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

        private void ProcessByHostName()
        {
            try
            {
                base.Response = base.IBX.SetDnsHostRecordName(base.Ref, this._hostRecord);
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
                base.Response = base.IBX.SetDnsHostRecordNextAvailableIP(base.Ref, this._network, this._dhcp, this._setHostName, this._mac);
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
            bool RemoveEmpty = this._removeEmpty;

            if (!this.MyInvocation.BoundParameters.ContainsKey("RemoveEmptyProperties"))
            {
                int choice = 0;

                ChoiceDescription yes = new ChoiceDescription("&Remove", "Will remove empty and null values from the object during serialization.");
                ChoiceDescription no = new ChoiceDescription("&Keep", "Will keep empty and null values and overwrite any existing values.");
                Collection<ChoiceDescription> choices = new Collection<ChoiceDescription>() { yes, no };
                choice = Host.UI.PromptForChoice("Update Infoblox Object", ("Do you want to remove the empty properties when sending the object?"), choices, 0);

                switch (choice)
                {
                    default:
                    case 0:
                        RemoveEmpty = true;
                        break;
                    case 1:
                        RemoveEmpty = false;
                        break;
                }
            }

            try
            {
                base.Response = base.IBX.UpdateIbxObject(this._hostObject, RemoveEmpty);
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
                if (!this.ParameterSetName.Equals("ByObject") && !this.ParameterSetName.Equals("HostName"))
                {
                    RuntimeDefinedParameter mac = IBXDynamicParameters.MAC();
                    base.ParameterDictionary.Add(mac.Name, mac);
                }
            }

            if (this._nextAvailable)
            {
                RuntimeDefinedParameter network = IBXDynamicParameters.Network();
                base.ParameterDictionary.Add(network.Name, network);
            }

            return base.ParameterDictionary;
        }
    }
}

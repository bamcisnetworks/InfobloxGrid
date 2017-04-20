using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Management.Automation;
using System.Net;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.Add,
        "IBXIPAddressToHostRecord",
        SupportsShouldProcess = true
        )
    ]
    public class AddIPAddressToDnsHostRecordPSCmd : BaseIbxDnsPSCmd, IDynamicParameters
    {
        private string _ip = String.Empty;
        private string[] _ips = new string[0];
        private bool _nextAvailable = false;
        private string _network = String.Empty;
        private bool _dhcp = false;
        private string _mac = String.Empty;
        private bool _setHostName = false;

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
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            ParameterSetName = "SingleIp",
            HelpMessage = "Add a single IP to a dns host record."
            )
        ]
        [Alias("IPAddress")]
        public string IP
        {
            get
            {
                return this._ip;
            }
            set
            {
                this._ip = IPAddress.Parse(value).ToString();
            }
        }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = true,
            ParameterSetName = "MultipleIp",
            HelpMessage = "An array of IP Addresses to add to a dns host record."
            )
        ]
        [Alias("IPAddresses")]
        public string[] IPs
        {
            get
            {
                return this._ips;
            }
            set
            {
                if (value.Length > 0)
                {
                    this._ips = value;
                }
                else
                {
                    throw new ArgumentException("Array of IP Addresses must contain members.");
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
                if (!String.IsNullOrEmpty(value))
                {
                    this._network = value;
                }
                else
                {
                    throw new PSArgumentNullException("Network", "Network cannot be null or empty.");
                }
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
                case "SingleIp":
                    try
                    {
                        base.Response =  base.IBX.AddDnsHostRecordIPAddress(base.Ref, this._ip, this._dhcp, this._setHostName, this._mac);
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
                case "MultipleIp":
                    try
                    {
                        base.Response = base.IBX.AddDnsHostRecordIPAddresses(base.Ref, this._ips);
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
                case "NextAvailableIp":
                    try
                    {
                        base.Response = base.IBX.AddDnsHostRecordNextAvailableIPAddress(base.Ref, this._network, this._dhcp, this._setHostName, this._mac);
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
                if (!this.ParameterSetName.Equals("MultipleIp"))
                {
                    RuntimeDefinedParameter mac = IBXDynamicParameters.MAC();
                    base.ParameterDictionary.Add(mac.Name, mac);
                }
            }

            return base.ParameterDictionary;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.Remove,
        "IBXIPAddressFromHostRecord",
        SupportsShouldProcess = true
        )
    ]
    public class RemoveIPAddressFromDnsHostRecordPSCmd : BaseIbxDnsPSCmd
    {
        private List<string> _ips;
        private bool _force = false;

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
                return this._ips.FirstOrDefault();
            }
            set
            {
                this._ips = new List<string>() { IPAddress.Parse(value).ToString() };
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
                return this._ips.ToArray();
            }
            set
            {
                this._ips = new List<string>();
                if (value.Length > 0)
                {
                    foreach (string ip in value)
                    {
                        string temp = String.Empty;
                        NetworkAddressTest.isIPWithException(ip, out temp);
                        this._ips.Add(temp);
                    }
                }
                else
                {
                    throw new ArgumentException("Array of IP Addresses must contain members.");
                }
            }
        }

        [Parameter(
            ValueFromPipelineByPropertyName = true,
            Mandatory = false,
            HelpMessage = "Will bypass the confirmation dialog for each IP address."
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
            List<string> ipsToDelete = new List<string>();

            if (!this._force)
            {
                ChoiceDescription yes = new ChoiceDescription("&Yes", "Will delete the IP address from the host record.");
                ChoiceDescription no = new ChoiceDescription("&No", "Will cancel the action.");
                Collection<ChoiceDescription> choices = new Collection<ChoiceDescription>() { yes, no };
                
                foreach (string ip in this._ips)
                {
                    choice = Host.UI.PromptForChoice("Delete IP Address", ("Are you sure you want to delete " + ip + " ?"), choices, 0);

                    if (choice == 0)
                    {
                        ipsToDelete.Add(ip);
                    }
                }
            }
            else
            {
                ipsToDelete = this._ips;
            }

            if (ipsToDelete.Count > 0)
            {
                try
                {
                    base.Response = base.IBX.RemoveDnsHostRecordIPAddresses(base.Ref, ipsToDelete.ToArray());
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
            else
            {
                WriteWarning("No IP addresses to delete.");
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

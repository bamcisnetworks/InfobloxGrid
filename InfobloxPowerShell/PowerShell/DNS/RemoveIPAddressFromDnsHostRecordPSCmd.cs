using BAMCIS.Infoblox.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.Remove,
        "IBXIPAddressFromHostRecord",
        SupportsShouldProcess = true,
        ConfirmImpact = ConfirmImpact.High,
        DefaultParameterSetName = _ENTERED_SESSION
    )]
    public class RemoveIPAddressFromDnsHostRecordPSCmd : BaseIbxDnsForcePSCmd
    {
        /* Parameters:
         * Base: -GridMaster -Version -Credential -Session [Dynamic Params]
         * PassThru: -PassThru
         * Force: -Force
         */

        private List<string> _IPs;

        #region Parameters

        /// <summary>
        /// The existing InfobloxSession to use
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION
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
            ParameterSetName = _GRID
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
            ParameterSetName = _GRID
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
            ParameterSetName = _GRID
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
            Mandatory = true,
            HelpMessage = "An array of IP Addresses to add to a dns host record."
        )]
        [Alias("IPAddresses")]
        [ValidateNotNull()]
        public IEnumerable<string> IPs
        {
            get
            {
                return this._IPs;
            }
            set
            {
                this._IPs = new List<string>();
                if (value.Any())
                {
                    foreach (string ip in value)
                    {
                        string temp = String.Empty;
                        NetworkAddressTest.isIPWithException(ip, out temp);
                        this._IPs.Add(temp);
                    }
                }
                else
                {
                    throw new PSArgumentException("Array of IP Addresses must contain members.");
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
            List<string> IPsToDelete = new List<string>();

            if (!this.Force)
            {
                if (ShouldProcess("DNS Host Record", "Delete IPs", "Updating IBX DNS Object"))
                {
                    foreach (string ip in this._IPs)
                    {
                        if (ShouldContinue($"Do you want to delete {ip} from the DNS Host Record?", "Updating IBX DNS Object"))
                        {
                            IPsToDelete.Add(ip);
                        }
                    }
                }
            }
            else
            {
                IPsToDelete = this._IPs;
            }

            if (IPsToDelete.Count > 0)
            {
                StringWriter SWriter = new StringWriter();
                TextWriter OriginalOut = Console.Out;
                Console.SetOut(SWriter);

                try
                {
                    base.Response = base.IBX.RemoveDnsHostRecordIPAddresses(base._Ref, IPsToDelete.ToArray()).Result;
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

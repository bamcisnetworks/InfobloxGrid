using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public abstract class BaseIbxDnsPassThruPSCmd : BaseIbxDnsPSCmd
    {
        /// <summary>
        /// Will return the requested object to the pipeline
        /// </summary>
        [Parameter(
            HelpMessage = "Specifies if the object should be passed down the pipeline."
        )]
        public SwitchParameter PassThru
        {
            get
            {
                return base._PassThru;
            }
            set
            {
                base._PassThru = value;
            }
        }
    }
}

using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public abstract class BaseIbxDnsForcePSCmd : BaseIbxDnsPassThruPSCmd
    {
        /// <summary>
        /// Will ignore the confirm prompt
        /// </summary>
        [Parameter(
            HelpMessage = "Ignores the confirm prompts."
        )]
        public SwitchParameter Force
        {
            get
            {
                return base._Force;
            }
            set
            {
                base._Force = value;
            }
        }
    }
}

using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    public abstract class ForcePSCmd : PassThruPSCmd
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
                this._Force = value;
            }
        }
    }
}

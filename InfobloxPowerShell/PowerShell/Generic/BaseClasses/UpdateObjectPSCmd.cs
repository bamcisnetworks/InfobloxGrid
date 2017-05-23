using System;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    public abstract class UpdateObjectPSCmd : ForcePSCmd
    {
        [Parameter(
            Mandatory = true,
            HelpMessage = "The dhcp object reference string."
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
                    throw new PSArgumentNullException("Reference", "The reference cannot be null or empty.");
                }
            }
        }

        [Parameter(
            Mandatory = false,
            ParameterSetName = BaseIbxObjectPSCmd._GRID_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        [Parameter(
            Mandatory = false,
            ParameterSetName = BaseIbxObjectPSCmd._SESSION_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        [Parameter(
            Mandatory = false,
            ParameterSetName = BaseIbxObjectPSCmd._ENTERED_SESSION_BY_OBJECT,
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
        )]
        public bool RemoveEmptyProperties { get; set; }
    }
}

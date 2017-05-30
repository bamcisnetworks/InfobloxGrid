using System;

namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class filterrule
    {
        private string _permission;

        public string filter { get; set; }
        public string permission
        {
            get
            {
                return this._permission;
            }
            set
            {
                if (!String.IsNullOrEmpty(value) && (value.Equals("Allow") || value.Equals("Deny")))
                {
                    this._permission = value;
                }
                else
                {
                    throw new ArgumentException("The value for permission must be Allow or Deny");
                }
            }
        }
    }
}

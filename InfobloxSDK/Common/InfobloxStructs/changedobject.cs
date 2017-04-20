using BAMCIS.Infoblox.Common.Enums;
using System;

namespace BAMCIS.Infoblox.Common.InfobloxStructs
{
    public class changedobject
    {
        private string _action;

        [SearchableAttribute(Equality = true)]
        public string action
        {
            get
            {
                return this._action;
            }
            set
            {
                if (ActionStringConst.Contains(value))
                {
                    this._action = value;
                }
                else
                {
                    throw new ArgumentException("The value provided for action is not valid.");
                }
            }
        }
        [SearchableAttribute(Equality = true)]
        public string name { get; set; }
        [SearchableAttribute(Equality = true)]
        public string object_type { get; set; }
        public string[] properties { get; set; }
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string @type { get; set; }
    }
}

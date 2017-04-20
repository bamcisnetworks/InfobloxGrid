using BAMCIS.Infoblox.Common;
using System;

namespace BAMCIS.Infoblox.InfobloxObjects.DNS
{
    [Name("record:dtclbdn")]
    public class dtclbdn : RefObject
    {
        private string _comment;
        private string _name;
        private string _pattern;
        private string _view;

        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string comment
        {
            get
            {
                return this._comment;
            }
            internal protected set
            {
                if (value.TrimValue().Length <= 256)
                {
                    this._comment = value.TrimValue();
                }
                else
                {
                    throw new ArgumentException("The comment must be 256 characters or less.");
                }
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public bool disable { get; internal protected set; }
        [ReadOnlyAttribute]
        public string lbdn { get; internal protected set; }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string name
        {
            get
            {
                return this._name;
            }
            internal protected set
            {
                this._name = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        public string pattern
        {
            get
            {
                return this._pattern;
            }
            internal protected set
            {
                this._pattern = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string view
        {
            get
            {
                return this._view;
            }
            internal protected set
            {
                this._view = value.TrimValue();
            }
        }
        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string zone { get; internal protected set; }
    }
}

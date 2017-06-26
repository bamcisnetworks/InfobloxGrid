namespace BAMCIS.Infoblox.Core.BaseObjects
{
    public abstract class BaseNameObject : RefObject
    {
        private string _name;

        [Required]
        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public virtual string name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value.TrimValue();
            }
        }
    }
}


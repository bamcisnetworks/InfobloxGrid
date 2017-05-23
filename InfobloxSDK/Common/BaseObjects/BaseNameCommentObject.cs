namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseNameCommentObject : BaseCommentObject
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

using System;

namespace BAMCIS.Infoblox.Common.BaseObjects
{
    public abstract class BaseCommentObject : RefObject
    {
        private string _comment;

        [SearchableAttribute(CaseInsensitive = true, Equality = true, Regex = true)]
        [Basic]
        public string comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                if (value.TrimValue().Length <= 256)
                {
                    this._comment = value.TrimValue();
                }
                else
                {
                    throw new ArgumentException("The comment cannot exceed 256 characters.");
                }
            }
        }

        public BaseCommentObject()
        {
            this.comment = String.Empty;
        }
    }
}

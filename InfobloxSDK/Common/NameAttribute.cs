using System;

namespace BAMCIS.Infoblox.Common
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class NameAttribute : Attribute
    {
        private string _name;
        public string Name
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

        public NameAttribute()
        {
            this.Name = String.Empty;
        }
        public NameAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}

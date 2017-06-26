using BAMCIS.Infoblox.Core;

namespace BAMCIS.Infoblox.InfobloxObjects.Discovery
{
    [Name("discovery:devicecomponent")]
    public class devicecomponent : RefObject
    {
        private string _component_name;
        private string _description;
        private string _model;
        private string _serial;
        private string _type;

        [ReadOnlyAttribute]
        [Basic]
        public string component_name 
        { 
            get
            {
                return this._component_name;
            }
            internal protected set
            {
                this._component_name = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string description 
        {
            get
            {
                return this._description;
            }
            internal protected set
            {
                this._description = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [SearchableAttribute(Equality = true)]
        public string device { get; internal protected set; }

        [ReadOnlyAttribute]
        [Basic]
        public string model 
        {
            get
            {
                return this._model;
            }
            internal protected set
            {
                this._model = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string serial 
        {
            get
            {
                return this._serial;
            }
            internal protected set
            {
                this._serial = value.TrimValue();
            }
        }

        [ReadOnlyAttribute]
        [Basic]
        public string type 
        {
            get
            {
                return this._type;
            }
            internal protected set
            {
                this._type = value.TrimValue();
            }
        }
    }
}

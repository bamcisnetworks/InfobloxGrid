
namespace BAMCIS.Infoblox.Core.InfobloxStructs
{
    public class queriesuser
    {
        private string _comment;

        public string comment
        {
            get
            {
                return this._comment;
            }
            set
            {
                this._comment = value.TrimValue();
            }
        }
        public string user { get; set; }
    }
}

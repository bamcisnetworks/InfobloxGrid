
namespace BAMCIS.Infoblox.Core.InfobloxStructs.Discovery
{
    public class snmpcredential
    {
        private string _comment;
        private string _community_string;

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
        public string community_string 
        {
            get
            {
                return this._community_string;
            }
            set
            {
                this._community_string = value.TrimValue();
            }
        }
    }
}


namespace BAMCIS.Infoblox.Core.InfobloxStructs.Grid.CloudApi
{
    public class user
    {
        private string _remote_admin { get; set; }

        public bool is_remote { get; set; }
        public string local_admin { get; set; }
        public string remote_admin
        {
            get
            {
                return this._remote_admin;
            }
            set
            {
                this._remote_admin = value.TrimValue();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core.InfobloxStructs.Setting
{
    public class security
    {
        private List<object> _admin_access_items;

        public object[] admin_access_items
        {
            get
            {
                return this._admin_access_items.ToArray();
            }
            set
            {
                if (value.Length > 0)
                {
                    Type type = value[0].GetType();

                    if (value.ToList().Where(x => x.GetType().Equals(type)).Count().Equals(value.Length))
                    {
                        if (type.Equals(typeof(tsigac)) || type.Equals(typeof(addressac)))
                        {
                            this._admin_access_items = new List<object>();
                            
                            foreach (object val in value)
                            {
                                object newAcObject = Activator.CreateInstance(type);
                                foreach (PropertyInfo info in type.GetTypeInfo().GetProperties())
                                {
                                    info.SetValue(newAcObject, val.GetType().GetTypeInfo().GetProperty(info.Name).GetValue(val));
                                }
                                this._admin_access_items.Add(newAcObject);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("The type of the array must be addressac or tsigac.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("All objects in the admin_access_items property must be of the same type.");
                    }
                }
            }
        }
        public bool audit_log_rolling_enable { get; set; }
        public bool http_redirect_enable { get; set; }
        public bool lcd_input_enable { get; set; }
        public bool login_banner_enable { get; set; }
        public string login_banner_text { get; set; }
        public bool remote_console_access_enable { get; set; }
        public bool security_access_enable { get; set; }
        public bool security_access_remote_console_enable { get; set; }
        public uint session_timeout { get; set; }
        public bool ssh_perm_enable { get; set; }
        public bool support_access_enable { get; set; }
        public string support_access_info { get; set; }
    }
}

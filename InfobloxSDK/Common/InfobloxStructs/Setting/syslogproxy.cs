using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace BAMCIS.Infoblox.Common.InfobloxStructs.Setting
{
    public class syslogproxy
    {
        private List<object> _client_acls;

        public object[] client_acls 
        {
            get
            {
                return this._client_acls.ToArray();
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
                            this._client_acls = new List<object>();

                            foreach (object val in value)
                            {
                                object newAcObject = Activator.CreateInstance(type);
                                foreach (PropertyInfo info in type.GetTypeInfo().GetProperties())
                                {
                                    info.SetValue(newAcObject, val.GetType().GetTypeInfo().GetProperty(info.Name).GetValue(val));
                                }
                                this._client_acls.Add(newAcObject);
                            }
                        }
                        else
                        {
                            throw new ArgumentException("The type of the array must be addressac or tsigac.");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("All objects in the client_acls property must be of the same type.");
                    }
                }
            }
        }
        public bool enable { get; set; }
        public bool tcp_enable { get; set; }
        public uint tcp_port { get; set; }
        public bool udp_enable { get; set; }
        public uint udp_port { get; set; }
    }
}

using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.PowerShell
{
    /// <summary>
    /// Generates the dynamic parameters used by the PowerShell cmdlets
    /// </summary>
    internal static class IBXDynamicParameters
    {
        private static readonly string _SEARCH_PARAMETER_SET = "Search";

        /// <summary>
        /// Generates a Network parameter
        /// </summary>
        /// <returns></returns>
        internal static RuntimeDefinedParameter Network()
        {
            ParameterAttribute attr = new ParameterAttribute() {
                Mandatory = true,
                HelpMessage = "Specify the network the next available IP should come from, in FQDN/CIDR notation, like 192.168.0.1/24."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "Network",
                ParameterType = typeof(String),
            };

            param.Attributes.Add(attr);

            return param;
        }

        /// <summary>
        /// Generates a SetHostNameInDhcp switch parameter
        /// </summary>
        /// <returns></returns>
        internal static RuntimeDefinedParameter SetHostNameInDhcp()
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                Mandatory = false,
                HelpMessage = "Set the host name through dhcp option 12."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "SetHostNameInDhcp",
                ParameterType = typeof(SwitchParameter),
                Value = new SwitchParameter()
            };

            param.Attributes.Add(attr);

            return param;
        }

        /// <summary>
        /// Generates a MAC address parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter MAC(bool required = false)
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The MAC address of the host, used if creating a fixed address or defining a MAC as part of the host record."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "MAC",
                ParameterType = typeof(string),
            };

            param.Attributes.Add(attr);

            return param;
        }

        /// <summary>
        /// Generates a credential parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter Credential(bool required = false)
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The credentials to use to access the Grid Master."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "Credential",
                ParameterType = typeof(PSCredential),
            };

            param.Attributes.Add(attr);

            return param;
        }

        /// <summary>
        /// Returns an ObjectType parameter that applies to __AllParameterSets
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter ObjectType(bool required = false)
        {
            return ObjectType(ParameterAttribute.AllParameterSets, required);
        }

        /// <summary>
        /// Returns an ObjectType parameter that applies to the specified parameter set.
        /// </summary>
        /// <param name="parameterSetName">The parameter set this parameter set is part of</param>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter ObjectType(string parameterSetName, bool required = false)
        {
            if (!String.IsNullOrEmpty(parameterSetName))
            {
                ParameterAttribute attr = new ParameterAttribute()
                {
                    Mandatory = required,
                    HelpMessage = "The type of object to get.",
                    ParameterSetName = parameterSetName
                };

                RuntimeDefinedParameter param = new RuntimeDefinedParameter()
                {
                    Name = "ObjectType",
                    ParameterType = typeof(String)
                };

                ValidateSetAttribute set = new ValidateSetAttribute(Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().ToList().Select(x => x.ToString()).ToArray());

                param.Attributes.Add(attr);
                param.Attributes.Add(set);
                return param;
            }
            else
            {
                throw new ArgumentNullException("parameterSetName", "The parameter set name cannot be null or empty.");
            }
        }

        /// <summary>
        /// Generates a RecordType parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter RecordType(bool required = false)
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The type of record to create."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "RecordType",
                ParameterType = typeof(String)
            };

            ValidateSetAttribute set = new ValidateSetAttribute(IBXCommonMethods.GetDnsRecordTypes().Select(x => x.ToString()).ToArray());

            param.Attributes.Add(attr);
            param.Attributes.Add(set);
            return param;
        }

        /// <summary>
        /// Generates a DhcpType parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter DhcpType(bool required = false)
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The type of dhcp object to create."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "DhcpType",
                ParameterType = typeof(String)
            };

            ValidateSetAttribute set = new ValidateSetAttribute(IBXCommonMethods.GetDhcpRecordTypes().Select(x => x.ToString()).ToArray());

            param.Attributes.Add(attr);
            param.Attributes.Add(set);
            return param;
        }

        /// <summary>
        /// Generates a ZoneType parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter ZoneType(bool required = false)
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The type of zone object to create."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "ZoneType",
                ParameterType = typeof(String)
            };

            ValidateSetAttribute set = new ValidateSetAttribute(IBXCommonMethods.GetZoneTypes().Select(x => x.ToString()).ToArray());

            param.Attributes.Add(attr);
            param.Attributes.Add(set);
            return param;
        }

        /// <summary>
        /// Generates dynamic parameters for the properties of an object
        /// </summary>
        /// <param name="objectType">The type of object to get the properties of</param>
        /// <param name="parameterSetName">The name of the parameter set these parameters will be part of</param>
        /// <param name="required">Specifies if these parameters are mandatory</param>
        /// <returns></returns>
        public static IEnumerable<RuntimeDefinedParameter> ObjectTypeProperties(InfoBloxObjectsEnum objectType, string parameterSetName, bool required = false)
        {
            PropertyInfo[] info = objectType.GetObjectType().GetTypeInfo().GetProperties().Where(x => x.GetCustomAttribute<ReadOnlyAttribute>() == null && x.Name != "_ref").ToArray();

            List<RuntimeDefinedParameter> list = new List<RuntimeDefinedParameter>();

            foreach (PropertyInfo prop in info)
            {
                ParameterAttribute attr = new ParameterAttribute()
                {
                    Mandatory = required,
                    HelpMessage = $"The {prop.Name} property of the object.",
                    ParameterSetName = parameterSetName
                };

                if (prop.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    attr.Mandatory = true;
                }
                else
                {
                    attr.Mandatory = false;
                }

                RuntimeDefinedParameter param = new RuntimeDefinedParameter()
                {
                    Name = Char.ToUpper(prop.Name.First()) + prop.Name.Substring(1),
                    ParameterType = prop.PropertyType
                };

                param.Attributes.Add(attr);

                list.Add(param);
            }

            return list;
        }

        /// <summary>
        /// Generates dynamic parameters for the properties of an object. This defaults
        /// to the __AllParameterSets.
        /// </summary>
        /// <param name="objectType">The type of object to get the properties of</param>
        /// <param name="required">Specifies if these parameters are mandatory</param>
        /// <returns></returns>
        public static IEnumerable<RuntimeDefinedParameter> ObjectTypeProperties(InfoBloxObjectsEnum ObjectType, bool required = false)
        {
            return ObjectTypeProperties(ObjectType, ParameterAttribute.AllParameterSets, required);
        }

        /// <summary>
        /// Generates a search field which is a list of properties that can be
        /// searched on for an object
        /// </summary>
        /// <param name="ibxObject">The object whose properties will be enumerated to determine which can be searched on</param>
        /// <param name="required">Specifies if these parameters are mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter SearchField(InfoBloxObjectsEnum ibxObject, bool required = false)
        {
            //*** Build the list of properties that can be searched for an object

            ParameterAttribute attr = new ParameterAttribute()
            {
                ParameterSetName = _SEARCH_PARAMETER_SET,
                Mandatory = required,
                HelpMessage = "Select the field to search on."
            };

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "SearchField",
                ParameterType = typeof(String)
            };

            ValidateSetAttribute set = new ValidateSetAttribute(((IEnumerable<string>)typeof(SearchableAttribute).GetMethod("GetSearchableProperties").MakeGenericMethod(ibxObject.GetObjectType()).Invoke(typeof(SearchableAttribute), null)).ToArray());

            param.Attributes.Add(attr);
            param.Attributes.Add(set);

            return param;
        }

        /// <summary>
        /// Generates a search type parameter which is determined by the search field of the object
        /// </summary>
        /// <param name="objectType">The type of the object whose field is provided</param>
        /// <param name="searchField">The field (property) to be searched</param>
        /// <param name="required">Specifies if these parameters are mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter SearchType(Type objectType, string searchField, bool required = false)
        {
            ParameterAttribute attr = new ParameterAttribute()
            {
                ParameterSetName = _SEARCH_PARAMETER_SET,
                Mandatory = required,
                HelpMessage = "Select the type of search you want to perform."
            };   

            RuntimeDefinedParameter param = new RuntimeDefinedParameter()
            {
                Name = "SearchType",
                ParameterType = typeof(String)
            };

            SearchableAttribute search = objectType.GetProperty(searchField).GetCustomAttribute<SearchableAttribute>();

            List<string> searchesAllowed = new List<string>();

            foreach (PropertyInfo prop in search.GetType().GetProperties())
            {
                if (prop.PropertyType.Equals(typeof(bool)))
                {
                    if ((bool)prop.GetValue(search) == true)
                    {
                        searchesAllowed.Add(prop.Name);
                    }
                }
            }

            ValidateSetAttribute set = new ValidateSetAttribute(searchesAllowed.ToArray());

            param.Attributes.Add(attr);
            param.Attributes.Add(set);

            return param;
        }
    }
}

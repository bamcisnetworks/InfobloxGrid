using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace BAMCIS.Infoblox.PowerShell
{
    /// <summary>
    /// Generates the dynamic parameters used by the PowerShell cmdlets
    /// </summary>
    internal static class IBXDynamicParameters
    {
        private static readonly string _GRID_SEARCH = "GridSearch";
        private static readonly string _SESSION_SEARCH = "SessionSearch";
        private static readonly string _ENTERED_SESSION_SEARCH = "EnteredSessionSearch";

        private static readonly string[] _PARAMETER_SETS = new string[] { _GRID_SEARCH, _SESSION_SEARCH, _ENTERED_SESSION_SEARCH };

        /// <summary>
        /// Generates a Network parameter
        /// </summary>
        /// <returns></returns>
        internal static RuntimeDefinedParameter Network()
        {
            ParameterAttribute Attr = new ParameterAttribute()
            {
                Mandatory = true,
                HelpMessage = "Specify the network the next available IP should come from, in FQDN/CIDR notation, like 192.168.0.1/24."
            };

            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "Network",
                ParameterType = typeof(String),
            };

            Param.Attributes.Add(Attr);

            return Param;
        }

        /// <summary>
        /// Generates a SetHostNameInDhcp switch parameter
        /// </summary>
        /// <returns></returns>
        internal static RuntimeDefinedParameter SetHostNameInDhcp()
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "SetHostNameInDhcp",
                ParameterType = typeof(SwitchParameter),
                Value = new SwitchParameter()
            };

            Param.Attributes.Add(new ParameterAttribute()
            {
                Mandatory = false,
                HelpMessage = "Set the host name through dhcp option 12."
            });

            return Param;
        }

        /// <summary>
        /// Generates a MAC address parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        internal static RuntimeDefinedParameter MAC(bool required = false)
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "MAC",
                ParameterType = typeof(string),
            };

            Param.Attributes.Add(new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The MAC address of the host, used if creating a fixed address or defining a MAC as part of the host record."
            });

            return Param;
        }

        /// <summary>
        /// Generates a credential parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        internal static RuntimeDefinedParameter Credential(bool required = false, IEnumerable<string> parameterSets = null)
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "Credential",
                ParameterType = typeof(PSCredential),
            };

            if (parameterSets != null && parameterSets.Any())
            {
                foreach (string Set in parameterSets)
                {
                    Param.Attributes.Add(new ParameterAttribute()
                    {
                        Mandatory = required,
                        HelpMessage = "The credentials to use to access the Grid Master.",
                        ParameterSetName = Set
                    });
                }
            }
            else
            {
                Param.Attributes.Add(new ParameterAttribute()
                {
                    Mandatory = required,
                    HelpMessage = "The credentials to use to access the Grid Master.",
                });
            }

            return Param;
        }

        /// <summary>
        /// Returns an ObjectType parameter that applies to __AllParameterSets
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        internal static RuntimeDefinedParameter ObjectType(bool required = false)
        {
            return ObjectType(new string[] { ParameterAttribute.AllParameterSets }, required);
        }

        /// <summary>
        /// Returns an ObjectType parameter that applies to the specified parameter sets, or .
        /// </summary>
        /// <param name="parameterSets">The parameter sets this parameter is part of</param>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        internal static RuntimeDefinedParameter ObjectType(IEnumerable<string> parameterSets, bool required = false)
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "ObjectType",
                ParameterType = typeof(String)
            };

            if (parameterSets != null && parameterSets.Any())
            {
                foreach (string Set in parameterSets)
                {
                    Param.Attributes.Add(new ParameterAttribute()
                    {
                        Mandatory = required,
                        HelpMessage = "The type of object to get.",
                        ParameterSetName = Set
                    });
                }
            }
            else
            {
                Param.Attributes.Add(new ParameterAttribute()
                {
                    Mandatory = required,
                    HelpMessage = "The type of object to get.",
                    ParameterSetName = ParameterAttribute.AllParameterSets
                });
            }

            Param.Attributes.Add(new ValidateSetAttribute(Enum.GetValues(typeof(InfoBloxObjectsEnum)).Cast<InfoBloxObjectsEnum>().ToList().Select(x => x.ToString()).ToArray()));

            return Param;
        }

        /// <summary>
        /// Generates a RecordType parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        internal static RuntimeDefinedParameter RecordType(bool required = false)
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "RecordType",
                ParameterType = typeof(String)
            };

            Param.Attributes.Add(new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The type of record to create."
            });

            Param.Attributes.Add(new ValidateSetAttribute(IBXCommonMethods.GetDnsRecordTypes().Select(x => x.ToString()).ToArray()));
            return Param;
        }

        /// <summary>
        /// Generates a DhcpType parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        internal static RuntimeDefinedParameter DhcpType(bool required = false)
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "DhcpType",
                ParameterType = typeof(String)
            };

            Param.Attributes.Add(new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The type of dhcp object to create."
            });

            Param.Attributes.Add(new ValidateSetAttribute(IBXCommonMethods.GetDhcpRecordTypes().Select(x => x.ToString()).ToArray()));

            return Param;
        }

        /// <summary>
        /// Generates a ZoneType parameter
        /// </summary>
        /// <param name="required">Specifies if this parameter is mandatory</param>
        /// <returns></returns>
        public static RuntimeDefinedParameter ZoneType(bool required = false)
        {
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "ZoneType",
                ParameterType = typeof(String)
            };

            Param.Attributes.Add(new ParameterAttribute()
            {
                Mandatory = required,
                HelpMessage = "The type of zone object to create."
            });

            Param.Attributes.Add(new ValidateSetAttribute(IBXCommonMethods.GetZoneTypes().Select(x => x.ToString()).ToArray()));

            return Param;
        }

        /// <summary>
        /// Generates dynamic parameters for the properties of an object
        /// </summary>
        /// <param name="objectType">The type of object to get the properties of</param>
        /// <param name="parameterSetName">The name of the parameter set these parameters will be part of</param>
        /// <param name="required">Specifies if these parameters are mandatory</param>
        /// <returns></returns>
        public static IEnumerable<RuntimeDefinedParameter> ObjectTypeProperties(InfoBloxObjectsEnum objectType, IEnumerable<string> parameterSets)
        {
            PropertyInfo[] PropInfo = objectType.GetObjectType().GetTypeInfo().GetProperties().Where(x => x.GetCustomAttribute<ReadOnlyAttribute>() == null && x.Name != "_ref").ToArray();

            List<RuntimeDefinedParameter> ParamList = new List<RuntimeDefinedParameter>();

            foreach (PropertyInfo Property in PropInfo)
            {
                RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
                {
                    Name = Char.ToUpper(Property.Name.First()) + Property.Name.Substring(1),
                    ParameterType = Property.PropertyType
                };

                bool Mandatory = Property.GetCustomAttribute<RequiredAttribute>() != null;

                if (parameterSets != null && parameterSets.Any())
                {
                    foreach (string Set in parameterSets)
                    {
                        ParameterAttribute attr = new ParameterAttribute()
                        {
                            Mandatory = Mandatory,
                            HelpMessage = $"The {Property.Name} property of the object.",
                            ParameterSetName = Set
                        };

                        Param.Attributes.Add(attr);
                    }
                }
                else
                {
                    Param.Attributes.Add(new ParameterAttribute()
                    {
                        Mandatory = Mandatory,
                        HelpMessage = $"The {Property.Name} property of the object."
                    });
                }

                ParamList.Add(Param);
            }

            return ParamList;
        }

        /// <summary>
        /// Generates dynamic parameters for the properties of an object. This defaults
        /// to the __AllParameterSets.
        /// </summary>
        /// <param name="objectType">The type of object to get the properties of</param>
        /// <param name="required">Specifies if these parameters are mandatory</param>
        /// <returns></returns>
        public static IEnumerable<RuntimeDefinedParameter> ObjectTypeProperties(InfoBloxObjectsEnum ObjectType)
        {
            return ObjectTypeProperties(ObjectType, new string[] { ParameterAttribute.AllParameterSets });
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

            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "SearchField",
                ParameterType = typeof(String)
            };

            foreach (string Set in _PARAMETER_SETS)
            {
                Param.Attributes.Add(new ParameterAttribute()
                {
                    ParameterSetName = Set,
                    Mandatory = required,
                    HelpMessage = "Select the field to search on."
                });
            }

            Param.Attributes.Add(new ValidateSetAttribute(((IEnumerable<string>)typeof(SearchableAttribute).GetMethod("GetSearchableProperties").MakeGenericMethod(ibxObject.GetObjectType()).Invoke(typeof(SearchableAttribute), null)).ToArray()));

            return Param;
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
            RuntimeDefinedParameter Param = new RuntimeDefinedParameter()
            {
                Name = "SearchType",
                ParameterType = typeof(String)
            };

            foreach (string Set in _PARAMETER_SETS)
            {
                Param.Attributes.Add(new ParameterAttribute()
                {
                    ParameterSetName = Set,
                    Mandatory = required,
                    HelpMessage = "Select the type of search you want to perform."
                });
            }

            SearchableAttribute Search = objectType.GetProperty(searchField).GetCustomAttribute<SearchableAttribute>();

            List<string> SearchesAllowed = new List<string>();

            foreach (PropertyInfo Prop in Search.GetType().GetProperties())
            {
                if (Prop.PropertyType.Equals(typeof(bool)))
                {
                    if ((bool)Prop.GetValue(Search) == true)
                    {
                        SearchesAllowed.Add(Prop.Name);
                    }
                }
            }

            Param.Attributes.Add(new ValidateSetAttribute(SearchesAllowed.ToArray()));

            return Param;
        }
    }
}

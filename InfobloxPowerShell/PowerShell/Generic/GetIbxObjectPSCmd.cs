using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.Get,
        "IBXObject",
        SupportsShouldProcess = false
        )
    ]
    public class GetIBXObjectCommand : BaseIbxObjectPSCmd, IDynamicParameters
    {
        private string _searchField;
        private SearchType _searchType = SearchType.EQUALITY;
        private string _searchValue;
        private InfoBloxObjectsEnum _type;

        #region Parameters

        [Parameter(
            ParameterSetName = "Reference",
            Mandatory = true,
            HelpMessage = "The object reference string.")
        ]
        [Alias("Ref")]
        public string Reference
        {
            get
            {
                return base.Ref;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    base.Ref = value;
                }
                else
                {
                    throw new PSArgumentNullException("Reference", "The host record reference cannot be null or empty.");
                }
            }
        }

        [Parameter(
            ParameterSetName = "Search",
            Mandatory = true,
            HelpMessage = "The value of the object field being searched."
            )
        ]
        public string SearchValue
        {
            get
            {
                return this._searchValue;
            }
            set
            {
                this._searchValue = value;
            }
        }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (this.ParameterSetName == "Search")
            {
                string objtype = base.GetUnboundValue("ObjectType", 0) as string;

                RuntimeDefinedParameter param = IBXDynamicParameters.ObjectType(true);

                base.ParameterDictionary.Add(param.Name, param);

                if (!String.IsNullOrEmpty(objtype))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(objtype, out this._type))
                    {
                        base.ObjectType = this._type;
                        RuntimeDefinedParameter searchFields = IBXDynamicParameters.SearchField(base.ObjectType, true);
                        base.ParameterDictionary.Add(searchFields.Name, searchFields);
                    }
                }

                string search = base.GetUnboundValue("SearchField", 0) as string;

                if (!String.IsNullOrEmpty(search))
                {
                    //*** After the user has picked a search field, return the valid types of searches than can be performed

                    RuntimeDefinedParameter searchType = IBXDynamicParameters.SearchType(base.ObjectType.GetObjectType(), search, true);
                    base.ParameterDictionary.Add(searchType.Name, searchType);
                }
            }

            return base.ParameterDictionary;
        }

        #region Override Methods
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "Reference":
                    this.ProcessByReference();
                    break;
                case "Search":
                    this.Search();
                    break;
                default:
                    throw new ArgumentException("Bad ParameterSet Name");
            }

            if (base.ObjectResponse != null)
            {
                WriteObject(base.ObjectResponse);
            }
            else
            {
                WriteWarning("No matching records were returned.");
            }
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        #endregion

        #region Helper Methods

        private void ProcessByReference()
        {
            string[] temp = base.Ref.Split('/');
            if (temp.Length > 0)
            {
                try
                {
                    base.ObjectType = IBXCommonMethods.GetInfobloxObjectEnumFromName(temp[0]);
                    try
                    {
                        base.ObjectResponse = typeof(IBXCommonMethods).GetMethod("GetIbxObject").MakeGenericMethod(base.ObjectType.GetObjectType()).Invoke(base.IBX, new object[] { base.Ref });
                    }
                    catch (AggregateException ae)
                    {
                        PSCommon.WriteExceptions(ae, this.Host);
                        this.ThrowTerminatingError(new ErrorRecord(ae.Flatten(), ae.GetType().FullName, ErrorCategory.NotSpecified, this));
                    }
                    catch (Exception e)
                    {
                        PSCommon.WriteExceptions(e, this.Host);
                        this.ThrowTerminatingError(new ErrorRecord(e, e.GetType().FullName, ErrorCategory.NotSpecified, this));
                    }
                }
                catch (Exception e)
                {
                    PSCommon.WriteExceptions(e, this.Host);
                    this.ThrowTerminatingError(new ErrorRecord(e, e.GetType().FullName, ErrorCategory.NotSpecified, this));
                }
            }
            else
            {
                throw new PSArgumentException("The reference value was not properly formatted.");
            }
        }

        private void Search()
        {
            if (base.ParameterDictionary.ContainsKey("ObjectType"))
            {
                string objectType = base.ParameterDictionary["ObjectType"].Value as string;
                if (!String.IsNullOrEmpty(objectType))
                {
                    if (!Enum.TryParse<InfoBloxObjectsEnum>(objectType.ToUpper(), out this._type))
                    {
                        throw new PSArgumentException("Object type parameter was not an allowed value.");
                    }
                    else
                    {
                        base.ObjectType = this._type;

                        this._searchField = base.ParameterDictionary["SearchField"].Value.ToString();

                        if (base.ParameterDictionary.ContainsKey("SearchType"))
                        {
                            string searchType = base.ParameterDictionary["SearchType"].Value as string;
                            if (!String.IsNullOrEmpty(searchType))
                            {
                                if (!Enum.TryParse<SearchType>(searchType.ToUpper(), out this._searchType))
                                {
                                    throw new PSArgumentException("Search type parameter was not an allowed value.");
                                }
                            }
                            else
                            {
                                throw new PSArgumentNullException("SearchType", "The search type cannot be null or empty.");
                            }
                        }
                        else
                        {
                            throw new PSArgumentNullException("SearchType", "The search type parameter is required for a search.");
                        }
                    }
                }
                else
                {
                    throw new PSArgumentNullException("ObjectType", "The object type parameter cannot be null or empty.");
                }
            }
            else
            {
                throw new PSArgumentNullException("ObjectType", "The object type parameter is required for a search.");
            }

            try
            {
                base.ObjectResponse = typeof(IBXCommonMethods).GetMethod("SearchIbxObject").MakeGenericMethod(base.ObjectType.GetObjectType()).Invoke(base.IBX, new object[] { this._searchType, this._searchField, this._searchValue });
            }
            catch (AggregateException ae)
            {
                PSCommon.WriteExceptions(ae, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(ae.Flatten(), ae.GetType().FullName, ErrorCategory.NotSpecified, this));
            }
            catch (Exception e)
            {
                PSCommon.WriteExceptions(e, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(e, e.GetType().FullName, ErrorCategory.NotSpecified, this));
            }
        }

        #endregion
    }
}

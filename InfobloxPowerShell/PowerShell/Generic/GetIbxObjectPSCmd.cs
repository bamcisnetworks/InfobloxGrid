using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.Get,
        "IBXObject",
        SupportsShouldProcess = false,
        DefaultParameterSetName = _ENTERED_SESSION_SEARCH
    )]
    public class GetIBXObjectCommand : BaseIbxObjectPSCmd, IDynamicParameters
    {
        private string _SearchField;
        private SearchType _searchType = SearchType.EQUALITY;

        #region Parameters

        /// <summary>
        /// The existing InfobloxSession to use
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_REFERENCE
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "An established session object to use to connect to the grid master.",
            ParameterSetName = _SESSION_SEARCH
        )]
        [ValidateNotNull()]
        public override InfobloxSession Session
        {
            get
            {
                return base.Session;
            }
            set
            {
                base.Session = value;
            }
        }

        /// <summary>
        /// The grid master to communicate with. This will be used to build the URL string.
        /// </summary>
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_SEARCH
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [ValidateNotNullOrEmpty()]
        public override string GridMaster
        {
            get
            {
                return base.GridMaster;
            }
            set
            {
                base.GridMaster = value;
            }
        }

        /// <summary>
        /// The API version to specify in the URL path. The function to build the URL string for the
        /// query will add in the leading "v"
        /// </summary>
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_SEARCH
        )]
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to LATEST.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [Alias("ApiVersion")]
        [ValidateSet("LATEST", "1.0", "1.1", "1.2", "1.2.1", "1.3", "1.4", "1.4.1", "1.4.2",
            "1.5", "1.6", "1.6.1", "1.7", "1.7.1", "1.7.2", "1.7.3", "1.7.4", "2.0",
            "2.1", "2.1.1", "2.2", "2.2.1", "2.2.2", "2.3")]
        [ValidateNotNullOrEmpty()]
        public override string Version
        {
            get
            {
                return base.Version;
            }
            set
            {
                base.Version = value;
            }
        }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_SEARCH
        )]
        [Parameter(
            Mandatory = true,
            HelpMessage = "The credentials to use to access the Grid Master.",
            ParameterSetName = _GRID_REFERENCE
        )]
        [ValidateNotNull()]
        [System.Management.Automation.Credential()]
        public override PSCredential Credential
        {
            get
            {
                return base.Credential;
            }
            set
            {
                base.Credential = value;
            }
        }

        [Parameter(
            ParameterSetName = _GRID_REFERENCE,
            Mandatory = true,
            HelpMessage = "The object reference string."
        )]
        [Parameter(
            ParameterSetName = _SESSION_REFERENCE,
            Mandatory = true,
            HelpMessage = "The object reference string."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_REFERENCE,
            Mandatory = true,
            HelpMessage = "The object reference string."
        )]
        [Alias("Ref")]
        [ValidateNotNullOrEmpty()]
        public string Reference
        {
            get
            {
                return base._Ref;
            }
            set
            {
                base._Ref = value;
            }
        }

        [Parameter(
            ParameterSetName = _GRID_SEARCH,
            Mandatory = true,
            HelpMessage = "The value of the object field being searched."
        )]
        [Parameter(
            ParameterSetName = _SESSION_SEARCH,
            Mandatory = true,
            HelpMessage = "The value of the object field being searched."
        )]
        [Parameter(
            ParameterSetName = _ENTERED_SESSION_SEARCH,
            Mandatory = true,
            HelpMessage = "The value of the object field being searched."
        )]
        public string SearchValue { get; set; }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (String.IsNullOrEmpty(this.Reference))
            {
                RuntimeDefinedParameter Parameter = IBXDynamicParameters.ObjectType(true);

                base.ParameterDictionary.Add(Parameter.Name, Parameter);

                string ObjectType = this.GetUnboundValue<string>("ObjectType");

                if (!String.IsNullOrEmpty(ObjectType))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(ObjectType, out base.ObjectType))
                    {
                        RuntimeDefinedParameter SearchFields = IBXDynamicParameters.SearchField(base.ObjectType, true);
                        base.ParameterDictionary.Add(SearchFields.Name, SearchFields);
                    }
                }

                string SearchField = this.GetUnboundValue<string>("SearchField");

                if (!String.IsNullOrEmpty(SearchField))
                {
                    //*** After the user has picked a search field, return the valid types of searches than can be performed

                    RuntimeDefinedParameter SearchType = IBXDynamicParameters.SearchType(base.ObjectType.GetObjectType(), SearchField, true);
                    base.ParameterDictionary.Add(SearchType.Name, SearchType);
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
            if (this.ParameterSetName.EndsWith(_REFERENCE))
            {
                this.ProcessByReference();
            }
            else if (this.ParameterSetName.EndsWith(_SEARCH))
            {
                this.Search();
            }
            else
            {
                throw new PSArgumentException($"Bad ParameterSet Name: {this.ParameterSetName}");
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
            string[] Temp = base._Ref.Split('/');
            if (Temp.Length > 0)
            {
                try
                {
                    base.ObjectType = IBXCommonMethods.GetInfobloxObjectEnumFromName(Temp[0]);

                    StringWriter SWriter = new StringWriter();
                    TextWriter OriginalOut = Console.Out;
                    Console.SetOut(SWriter);

                    try
                    {
                        base.ObjectResponse = typeof(IBXCommonMethods).GetMethod("GetIbxObject").MakeGenericMethod(base.ObjectType.GetObjectType()).InvokeGenericAsync(base.IBX, new object[] { base._Ref }).Result;
                    }
                    catch (AggregateException ae)
                    {
                        PSCommon.WriteExceptions(ae, this.Host);
                        this.ThrowTerminatingError(new ErrorRecord(ae.InnerException, ae.InnerException.GetType().FullName, ErrorCategory.NotSpecified, this));
                    }
                    catch (Exception e)
                    {
                        PSCommon.WriteExceptions(e, this.Host);
                        this.ThrowTerminatingError(new ErrorRecord(e, e.GetType().FullName, ErrorCategory.NotSpecified, this));
                    }
                    finally
                    {
                        WriteVerbose(SWriter.ToString());
                        Console.SetOut(OriginalOut);
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
                    if (!Enum.TryParse<InfoBloxObjectsEnum>(objectType.ToUpper(), out base.ObjectType))
                    {
                        throw new PSArgumentException("Object type parameter was not an allowed value.");
                    }
                    else
                    {
                        this._SearchField = base.ParameterDictionary["SearchField"].Value.ToString();

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

            StringWriter SWriter = new StringWriter();
            TextWriter OriginalOut = Console.Out;
            Console.SetOut(SWriter);

            try
            {
                base.ObjectResponse = (IEnumerable<object>)typeof(IBXCommonMethods).GetMethod("SearchIbxObject").MakeGenericMethod(base.ObjectType.GetObjectType()).InvokeGenericAsync(base.IBX, new object[] { this._searchType, this._SearchField, this.SearchValue }).Result;
            }
            catch (AggregateException ae)
            {
                PSCommon.WriteExceptions(ae, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(ae.InnerException, ae.InnerException.GetType().FullName, ErrorCategory.NotSpecified, this));
            }
            catch (Exception e)
            {
                PSCommon.WriteExceptions(e, this.Host);
                this.ThrowTerminatingError(new ErrorRecord(e, e.GetType().FullName, ErrorCategory.NotSpecified, this));
            }
            finally
            {
                WriteVerbose(SWriter.ToString());
                Console.SetOut(OriginalOut);
            }
        }

        #endregion
    }
}

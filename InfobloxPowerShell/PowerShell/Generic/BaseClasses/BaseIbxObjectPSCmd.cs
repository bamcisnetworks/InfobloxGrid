using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.InfobloxMethods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net.Http;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    public abstract class BaseIbxObjectPSCmd : PSCmdlet, IDynamicParameters
    {
        protected const string _BY_OBJECT = "ByObject";
        protected const string _BY_ATTRIBUTE = "ByAttribute";

        protected const string _GRID = "Grid";
        protected const string _SESSION = "Session";
        protected const string _ENTERED_SESSION = "EnteredSession";

        protected const string _NEXT_AVAILABLE_IP = "NextAvailableIp";
        protected const string _HOST_NAME = "HostName";
        protected const string _SINGLE_IP = "SingleIp";
        protected const string _MULTIPLE_IP = "MultipleIp";
        protected const string _SPECIFY_IP = "SpecifyIp";
        protected const string _SEARCH = "Search";
        protected const string _REFERENCE = "Reference";

        protected const string _GRID_BY_OBJECT = _GRID + _BY_OBJECT;
        protected const string _GRID_BY_ATTRIBUTE = _GRID + _BY_ATTRIBUTE;

        protected const string _SESSION_BY_ATTRIBUTE = _SESSION + _BY_ATTRIBUTE;
        protected const string _SESSION_BY_OBJECT = _SESSION + _BY_OBJECT;

        protected const string _ENTERED_SESSION_BY_ATTRIBUTE = _ENTERED_SESSION + _BY_ATTRIBUTE;
        protected const string _ENTERED_SESSION_BY_OBJECT = _ENTERED_SESSION + _BY_OBJECT;

        protected const string _GRID_HOSTNAME = _GRID + _HOST_NAME;
        protected const string _SESSION_HOSTNAME = _SESSION + _HOST_NAME;
        protected const string _ENTERED_SESSION_HOSTNAME = _ENTERED_SESSION + _HOST_NAME;

        protected const string _GRID_NEXT_AVAILABLE_IP = _GRID + _NEXT_AVAILABLE_IP;
        protected const string _SESSION_NEXT_AVAILABLE_IP = _SESSION + _NEXT_AVAILABLE_IP;
        protected const string _ENTERED_SESSION_NEXT_AVAILABLE_IP = _ENTERED_SESSION + _NEXT_AVAILABLE_IP;

        protected const string _GRID_SINGLE_IP = _GRID + _SINGLE_IP;
        protected const string _SESSION_SINGLE_IP = _SESSION + _SINGLE_IP;
        protected const string _ENTERED_SESSION_SINGLE_IP = _ENTERED_SESSION + _SINGLE_IP;

        protected const string _GRID_MULTIPLE_IP = _GRID + _MULTIPLE_IP;
        protected const string _SESSION_MULTIPLE_IP = _SESSION + _MULTIPLE_IP;
        protected const string _ENTERED_SESSION_MULTIPLE_IP = _ENTERED_SESSION + _MULTIPLE_IP;

        protected const string _GRID_SPECIFY_IP = _GRID + _SPECIFY_IP;
        protected const string _SESSION_SPECIFY_IP = _SESSION + _SPECIFY_IP;
        protected const string _ENTERED_SESSION_SPECIFY_IP = _ENTERED_SESSION + _SPECIFY_IP;

        protected const string _GRID_SEARCH = _GRID + _SEARCH;
        protected const string _SESSION_SEARCH = _SESSION + _SEARCH;
        protected const string _ENTERED_SESSION_SEARCH = _ENTERED_SESSION + _SEARCH;

        protected const string _GRID_REFERENCE = _GRID + _REFERENCE;
        protected const string _SESSION_REFERENCE = _SESSION + _REFERENCE;
        protected const string _ENTERED_SESSION_REFERENCE = _ENTERED_SESSION + _REFERENCE;

        private IBXCommonMethods _IBX;
        private string _response;
        protected bool _PassThru = false;
        protected bool _Force;
        protected InfoBloxObjectsEnum ObjectType;

        protected string _Ref = String.Empty;
        protected string _GridMaster = String.Empty;
        protected PSCredential _Credential = PSCredential.Empty;
        protected string _Version = "LATEST";
        protected InfobloxSession _Session = null;
        protected UInt32 _Timeout = 100;

        protected IEnumerable<string> _FieldsToReturn;

        #region Parameters

        [ValidateNotNull()]
        public virtual InfobloxSession Session
        {
            get
            {
                return this._Session;
            }
            set
            {
                this._Session = value;
            }
        }

        [ValidateNotNullOrEmpty()]
        public virtual string GridMaster
        {
            get
            {
                return this._GridMaster;
            }
            set
            {
                this._GridMaster = value;
            }
        }

        [Alias("ApiVersion")]
        [ValidateSet("LATEST", "1.0", "1.1", "1.2", "1.2.1", "1.3", "1.4", "1.4.1", "1.4.2",
            "1.5", "1.6", "1.6.1", "1.7", "1.7.1", "1.7.2", "1.7.3", "1.7.4", "2.0",
            "2.1", "2.1.1", "2.2", "2.2.1", "2.2.2", "2.3")]
        [ValidateNotNullOrEmpty()]
        public virtual string Version
        {
            get
            {
                return this._Version;
            }
            set
            {
                this._Version = value;
            }
        }

        [ValidateNotNull()]
        [System.Management.Automation.Credential()]
        public virtual PSCredential Credential
        {
            get
            {
                return this._Credential;
            }
            set
            {
                this._Credential = value;
            }
        }

        [Parameter(
            HelpMessage = "The timeout in seconds to use for the HttpClient. This defaults to 100."    
        )]
        public UInt32 Timeout
        {
            get
            {
                return this._Timeout;
            }
            set
            {
                this._Timeout = value;
            }
        }

        #endregion

        public object ObjectResponse
        {
            get; protected set;
        }

        protected IBXCommonMethods IBX
        {
            get
            {
                return this._IBX;
            }
        }

        public string Response
        {
            get
            {
                return this._response;
            }
            set
            {
                this._response = value;
            }
        }

        protected RuntimeDefinedParameterDictionary ParameterDictionary { get; set; }

        /// <summary>
        /// Default contsructor to build the runtime defined parameter dictionary.
        /// Each inherited explicit constructor should call base()
        /// </summary>
        public BaseIbxObjectPSCmd()
        {
            this.ParameterDictionary = new RuntimeDefinedParameterDictionary();
        }

        /// <summary>
        /// Adds the base dynamic parameters
        /// </summary>
        /// <returns>The runtime defined parameter dictionary</returns>
        public virtual object GetDynamicParameters()
        {
            //The param dictionary must be returned as part of the IDynamicParameter implementation
            //However, we save the params as well so inherited classes can add to them
            //The method is called by the PowerShell runtime
            return this.ParameterDictionary;
        }

        #region Override Methods

        protected override void BeginProcessing()
        {
            try
            {
                if (this.ParameterSetName.StartsWith(_GRID))
                {
                    if (this.Version.Equals("LATEST"))
                    {
                        using (HttpClient Client = CommandHelpers.BuildHttpClient(this.GridMaster, "1.0", this.Credential.UserName, this.Credential.Password, TimeSpan.FromSeconds(Timeout)).Result)
                        {
                            WriteVerbose("Getting supported versions.");

                            HttpResponseMessage Response = Client.GetAsync("?_schema").Result;

                            WriteVerbose(Response.RequestMessage.RequestUri.ToString());

                            if (Response.IsSuccessStatusCode)
                            {
                                string Content = Response.Content.ReadAsStringAsync().Result;

                                WriteVerbose($"Response {Content}");

                                dynamic Obj = JsonConvert.DeserializeObject(Content);
                                IEnumerable<string> Versions = Obj.supported_versions;

                                WriteVerbose("Got versions");

                                Versions = Versions.Select(x => { return new Version(x); }).OrderByDescending(x => x).Select(x => { return x.ToString(); });

                                WriteVerbose("Sorted versions");
                                this.Version = Versions.First();
                                WriteVerbose($"Latest supported version is {this.Version}");
                            }
                            else
                            {
                                WriteVerbose("Failed to get schema, reverting to using version 2.0");
                                this.Version = "2.0";
                            }
                        }
                    }

                    this._IBX = new IBXCommonMethods(this.GridMaster, this.Version, this.Credential.UserName, this.Credential.Password, TimeSpan.FromSeconds(Timeout));
                }
                else if (this.ParameterSetName.StartsWith(_SESSION))
                {
                    this._IBX = new IBXCommonMethods(this.Session, TimeSpan.FromSeconds(Timeout));
                }
                else if (this.ParameterSetName.StartsWith(_ENTERED_SESSION))
                {
                    this._IBX = new IBXCommonMethods(TimeSpan.FromSeconds(Timeout));
                }
                else
                {
                    this.ThrowTerminatingError(new ErrorRecord(new PSArgumentException($"Could not identify parameter set from {this.ParameterSetName}"), "PSArgumentException", ErrorCategory.InvalidArgument, this));
                }
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

        #region Helper Functions

        protected void FinishResponse(bool passThru)
        {
            if (passThru)
            {
                if (!String.IsNullOrEmpty(this._response))
                {
                    WriteObject(this._response);
                }
                else
                {
                    WriteWarning("There was a problem processing the request, the response was empty or null.");
                }
            }
        }

        protected virtual void ProcessByNewObject(object newObject)
        {
            if (newObject != null)
            {
                if (newObject.GetType().IsInfobloxType())
                {
                    StringWriter SWriter = new StringWriter();
                    TextWriter OriginalOut = Console.Out;
                    Console.SetOut(SWriter);

                    try
                    {
                        this.Response = this.IBX.NewIbxObject(newObject).Result;
                        this.FinishResponse(this._PassThru);
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
                else
                {
                    throw new PSArgumentException("newObject", $"The new object must be an Infoblox type, {newObject.GetType().FullName} was provided.");
                }
            }
            else
            {
                throw new PSArgumentNullException("newObject", "The new object cannot be null.");
            }
        }

        protected virtual void ProcessByUpdatedObject(object updatedObject, bool removeEmpty)
        {
            if (!this.MyInvocation.BoundParameters.ContainsKey("RemoveEmptyProperties"))
            {
                int UserChoice = 0;

                ChoiceDescription Yes = new ChoiceDescription("&Remove", "Will remove empty and null values from the object during serialization.");
                ChoiceDescription No = new ChoiceDescription("&Keep", "Will keep empty and null values and overwrite any existing values.");
                Collection<ChoiceDescription> Choices = new Collection<ChoiceDescription>() { Yes, No };
                UserChoice = Host.UI.PromptForChoice("Update Infoblox Object", ("Do you want to remove the empty properties when sending the object?"), Choices, 0);

                switch (UserChoice)
                {
                    default:
                    case 0:
                        {
                            removeEmpty = true;
                            break;
                        }
                    case 1:
                        {
                            removeEmpty = false;
                            break;
                        }
                }
            }

            StringWriter SWriter = new StringWriter();
            TextWriter OriginalOut = Console.Out;
            Console.SetOut(SWriter);

            try
            {
                updatedObject.GetType().GetProperty("_ref").SetValue(updatedObject, this._Ref);
                this.Response = this.IBX.UpdateIbxObject(updatedObject, removeEmpty).Result;
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

        protected virtual void ProcessByAttributeForNewObject(string attribute, List<KeyValuePair<string, string>> additionalProperties)
        {
            if (this.ParameterDictionary.ContainsKey(attribute))
            {
                string ParamValue = this.ParameterDictionary[attribute].Value as string;

                if (!String.IsNullOrEmpty(ParamValue) && Enum.TryParse<InfoBloxObjectsEnum>(ParamValue.ToUpper(), out this.ObjectType))
                {
                    List<KeyValuePair<string, string>> PropertyList = new List<KeyValuePair<string, string>>();

                    foreach (KeyValuePair<string, RuntimeDefinedParameter> obj in this.ParameterDictionary)
                    {
                        if (obj.Value.Value != null && !String.IsNullOrEmpty(obj.Value.Value as string))
                        {
                            PropertyList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.Value as string));
                        }
                    }

                    if (additionalProperties != null && additionalProperties.Count > 0)
                    {
                        PropertyList.AddRange(additionalProperties);
                    }

                    StringWriter SWriter = new StringWriter();
                    TextWriter OriginalOut = Console.Out;
                    Console.SetOut(SWriter);

                    try
                    {
                        this.Response = this.IBX.NewIbxObject(this.ObjectType.GetObjectType(), PropertyList).Result;
                        this.FinishResponse(this._PassThru);
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
                else
                {
                    throw new PSArgumentException($"The object type parameter was not an allowed value, {ParamValue} was provided.");
                }
            }
            else
            {
                throw new PSArgumentException("The object type parameter does not exist in the dynamic parameter dictionary.");
            }
        }

        protected virtual void ProcessByAttributeForNewObject(string attribute)
        {
            ProcessByAttributeForNewObject(attribute, null);
        }

        protected virtual void ProcessByAttributeForUpdatedObject(string attribute, List<KeyValuePair<string, string>> additionalProperties)
        {
            if (this.ParameterDictionary.ContainsKey(attribute))
            {
                string ParamValue = this.ParameterDictionary[attribute].Value as string;

                if (!String.IsNullOrEmpty(ParamValue) && Enum.TryParse<InfoBloxObjectsEnum>(ParamValue.ToUpper(), out this.ObjectType))
                {
                    List<KeyValuePair<string, string>> PropertyList = new List<KeyValuePair<string, string>>();

                    foreach (KeyValuePair<string, RuntimeDefinedParameter> obj in this.ParameterDictionary)
                    {
                        if (obj.Value.Value != null && !String.IsNullOrEmpty(obj.Value.Value as string))
                        {
                            PropertyList.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.Value as string));
                        }
                    }

                    if (additionalProperties != null && additionalProperties.Count > 0)
                    {
                        PropertyList.AddRange(additionalProperties);
                    }

                    StringWriter SWriter = new StringWriter();
                    TextWriter OriginalOut = Console.Out;
                    Console.SetOut(SWriter);

                    try
                    {
                        this.Response = typeof(IBXCommonMethods).GetMethod("UpdateIbxObject").MakeGenericMethod(this.ObjectType.GetObjectType()).InvokeGenericAsync(this.IBX, new object[] { this.ObjectType, PropertyList }).Result as string;
                        this.FinishResponse(this._PassThru);
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
                else
                {
                    throw new PSArgumentException($"Object type parameter was not an allowed value, {ParamValue} was provided.");
                }
            }
            else
            {
                throw new PSArgumentException("The object type parameter does not exist in the dynamic parameter dictionary.");
            }
        }

        protected virtual void ProcessByAttributeForUpdatedObject(string attribute)
        {
            ProcessByAttributeForUpdatedObject(attribute, null);
        }

        #endregion
    }
}
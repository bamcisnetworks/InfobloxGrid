using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    public abstract class BaseIbxObjectPSCmd : PSCmdlet, IDynamicParameters
    {
        private PSCredential _credential;
        private string _gridMaster;
        private string _apiVersion = "2.0";
        private IBXCommonMethods _ibx;
        private bool _passthru = false;
        private string _ref = String.Empty;
        private string _response;
        private object _objectResponse;
        private InfoBloxObjectsEnum _type;
        private RuntimeDefinedParameterDictionary _paramDictionary;

        #region Parameters

        /// <summary>
        /// The grid master to communicate with. This will be used to build the URL string.
        /// </summary>
        [Parameter(
            Position = 0,
            Mandatory = true,
            HelpMessage = "The IP address or FQDN of the grid master interface."
        )]
        [ValidateNotNullOrEmpty()]
        [Alias("ComputerName")]
        public string GridMaster
        {
            get
            {
                return this._gridMaster;
            }
            set
            {
                this._gridMaster = value;
            }
        }

        /// <summary>
        /// The API version to specify in the URL path. The function to build the URL string for the
        /// query will add in the leading "v"
        /// </summary>
        [Parameter(
            HelpMessage = "The version of the Infoblox WAPI, will default to \"2.0\"."),
        ]
        [Alias("ApiVersion")]
        [ValidateSet("1.0", "1.1", "1.2", "1.2.1", "1.3", "1.4", "1.4.1", "1.4.2", 
            "1.5", "1.6", "1.6.1", "1.7", "1.7.1", "1.7.2", "1.7.3", "1.7.4", "2.0", 
            "2.1", "2.1.1", "2.2", "2.2.1", "2.2.2", "2.3"
        )]
        [ValidateNotNullOrEmpty()]
        public string Version
        {
            get
            {
                return this._apiVersion;
            }
            set
            {
                this._apiVersion = value;
            }
        }

        /// <summary>
        /// Will return the requested object to the pipeline
        /// </summary>
        [Parameter(
            HelpMessage = "Specifies if the object should be passed down the pipeline, defaults to false."
        )
        ]
        public SwitchParameter PassThru
        {
            get { return this._passthru; }
            set { this._passthru = value; }
        }

        #endregion

        /// <summary>
        /// Default contsructor to build the runtime defined parameter dictionary.
        /// Each inherited explicit constructor should call base()
        /// </summary>
        public BaseIbxObjectPSCmd()
        {
            this._paramDictionary = new RuntimeDefinedParameterDictionary();
        }

        /// <summary>
        /// Adds the base dynamic parameters, which is either a mandatory or
        /// optional credential object depending on whether a cookie is present
        /// from a previous API call. Each 
        /// </summary>
        /// <returns>The runtime defined parameter dictionary</returns>
        public virtual object GetDynamicParameters()
        {
            if (!CommandHelpers.DoesValidCookieExist())
            {
                RuntimeDefinedParameter cred = IBXDynamicParameters.Credential(true);
                this._paramDictionary.Add(cred.Name, cred);
            }
            else
            {
                RuntimeDefinedParameter cred = IBXDynamicParameters.Credential();
                this._paramDictionary.Add(cred.Name, cred);
            }

            //The param dictionary must be returned as part of the IDynamicParameter implementation
            //However, we save the params as well so inherited classes can add to them
            //The method is called by the PowerShell runtime
            return this._paramDictionary;
        }

        #region Override Methods
        protected override void BeginProcessing()
        {
            try
            {
                if (this._paramDictionary.ContainsKey("Credential"))
                {
                    if (this._paramDictionary["Credential"].IsSet)
                    {
                        this._credential = this._paramDictionary["Credential"].Value as PSCredential;
                        this._ibx = new IBXCommonMethods(this._gridMaster, this._apiVersion, this._credential.UserName, this._credential.Password);
                    }
                    else
                    {
                        this._ibx = new IBXCommonMethods(this._gridMaster, this._apiVersion);
                    }
                }
                else
                {
                    this._ibx = new IBXCommonMethods(this._gridMaster, this._apiVersion);
                }
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

        protected override void EndProcessing()
        {
            if (!String.IsNullOrEmpty(this._response) && this._passthru)
            {
                WriteObject(this._response);
            }
            else
            {
                WriteWarning("There was a problem processing the request.");
            }

            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        #endregion

        public virtual IBXCommonMethods IBX
        {
            get
            {
                return this._ibx;
            }
        }

        public string Ref
        {
            get
            {
                return this._ref;
            }
            set
            {
                this._ref = value;
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

        public object ObjectResponse
        {
            get
            {
                return this._objectResponse;
            }
            set
            {
                this._objectResponse = value;
            }
        }

        public InfoBloxObjectsEnum ObjectType
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public PSCredential Credential
        {
            get
            {
                return this._credential;
            }
            set
            {
                this._credential = value;
            }
        }

        public RuntimeDefinedParameterDictionary ParameterDictionary
        {
            get
            {
                return this._paramDictionary;
            }
            set
            {
                this._paramDictionary = value;
            }
        }

        #region Helper Functions

        public object GetUnboundValue(string paramName)
        {
            return GetUnboundValue(paramName, -1);
        }

        public object GetUnboundValue(string paramName, int unnamedPosition)
        {
            // If paramName isn't found, value at unnamedPosition will be returned instead
            var context = TryGetProperty(this, "Context");
            var processor = TryGetProperty(context, "CurrentCommandProcessor");
            var parameterBinder = TryGetProperty(processor, "CmdletParameterBinderController");
            var args = TryGetProperty(parameterBinder, "UnboundArguments") as System.Collections.IEnumerable;

            if (args != null)
            {
                var currentParameterName = string.Empty;
                object unnamedValue = null;
                int i = 0;
                foreach (var arg in args)
                {
                    var isParameterName = TryGetProperty(arg, "ParameterNameSpecified");
                    if (isParameterName != null && true.Equals(isParameterName))
                    {
                        string parameterName = TryGetProperty(arg, "ParameterName") as string;
                        currentParameterName = parameterName;

                        continue;
                    }

                    var parameterValue = TryGetProperty(arg, "ArgumentValue");

                    if (currentParameterName != string.Empty)
                    {
                        // Found currentParameterName's value. If it matches paramName, return
                        // it
                        if (currentParameterName.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                        {
                            return parameterValue;
                        }
                    }
                    else if (i++ == unnamedPosition)
                    {
                        unnamedValue = parameterValue;  // Save this for later in case paramName isn't found
                    }

                    // Found a value, so currentParameterName needs to be cleared
                    currentParameterName = string.Empty;
                }

                if (unnamedValue != null)
                {
                    return unnamedValue;
                }
            }
            return null;


        }

        public object TryGetProperty(object instance, string fieldName)
        {
            var bindingFlags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;

            // any access of a null object returns null. 
            if (instance == null || string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            var propertyInfo = instance.GetType().GetProperty(fieldName, bindingFlags);

            if (propertyInfo != null)
            {
                try
                {
                    return propertyInfo.GetValue(instance, null);
                }
                catch { }
            }

            // maybe it's a field
            var fieldInfo = instance.GetType().GetField(fieldName, bindingFlags);

            if (fieldInfo != null)
            {
                try
                {
                    return fieldInfo.GetValue(instance);
                }
                catch { }
            }

            // no match, return null.
            return null;
        }

        #endregion
    }
}

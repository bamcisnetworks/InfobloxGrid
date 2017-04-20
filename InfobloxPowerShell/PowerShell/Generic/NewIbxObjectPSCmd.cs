using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.New,
        "IBXObject",
        SupportsShouldProcess = true
        )]
    public class NewIbxObjectPSCmd : BaseIbxObjectPSCmd, IDynamicParameters
    {
        private object _inputObject;
        private InfoBloxObjectsEnum _type;

        #region Parameters

        [Parameter(
            Position = 1,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            ParameterSetName = "ByObject",
            Mandatory = true,
            HelpMessage = "The new object to be created.")]
        public object InputObject
        {
            get
            {
                return this._inputObject;
            }
            set
            {
                if (value != null)
                {
                    Type type;
                    if (value.GetType() == typeof(PSObject))
                    {
                        PSObject obj = (PSObject)value;
                        type = obj.BaseObject.GetType();
                        value = typeof(PSExtensionMethods).GetMethod("ConvertPSObject").MakeGenericMethod(type).Invoke(typeof(PSExtensionMethods), new object[] { obj });
                    }
                    else
                    {
                        type = value.GetType();
                    }

                    if (type.IsInfobloxType())
                    {
                        this._inputObject = value;
                    }
                    else
                    {
                        throw new ArgumentException(String.Format("The new object must be an infoblox object type, {0} was provided.", value.GetType().Name));
                    }
                }
                else
                {
                    throw new PSArgumentNullException("The new object parameter cannot be null.");
                }
            }
        }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            RuntimeDefinedParameter param = IBXDynamicParameters.ObjectType("ByAttribute", true);
            base.ParameterDictionary.Add(param.Name, param);

            string objtype = base.GetUnboundValue("ObjectType", 0) as string;

            if (!String.IsNullOrEmpty(objtype))
            {
                if (Enum.TryParse<InfoBloxObjectsEnum>(objtype.ToUpper(), out this._type))
                {
                    base.ObjectType = this._type;
                    foreach (RuntimeDefinedParameter pa in IBXDynamicParameters.ObjectTypeProperties(base.ObjectType, "ByAttribute"))
                    {
                        base.ParameterDictionary.Add(pa.Name, pa);
                    }
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
                case "ByObject":
                    this.ProcessByObject();
                    break;
                case "ByAttribute":
                    this.ProcessByAttribute();
                    break;
                default:
                    throw new PSArgumentException(String.Format("Bad parameter set name: {0}.", this.ParameterSetName));
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

        private void ProcessByObject()
        {
            if (this._inputObject != null)
            {
                if (this._inputObject.GetType().IsInfobloxType())
                {
                    try
                    {
                        base.Response = base.IBX.NewIbxObject(this._inputObject);
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
                else
                {
                    throw new PSArgumentException("The new object must be a valid infoblox object.");
                }
            }
            else
            {
                throw new PSArgumentNullException("InputObject", "The new object cannot be null.");
            }
        }

        private void ProcessByAttribute()
        {
            if (base.ParameterDictionary.ContainsKey("ObjectType"))
            {
                string val = base.ParameterDictionary["ObjectType"].Value as string;
                if (!String.IsNullOrEmpty(val) && Enum.TryParse<InfoBloxObjectsEnum>(val.ToUpper(), out this._type))
                {
                    base.ObjectType = this._type;

                    List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                    foreach (KeyValuePair<string, RuntimeDefinedParameter> obj in base.ParameterDictionary)
                    {
                        if (obj.Value.Value != null && !String.IsNullOrEmpty(obj.Value.Value as string))
                        {
                            list.Add(new KeyValuePair<string, string>(obj.Key, obj.Value.Value as string));
                        }
                    }

                    try
                    {
                        base.Response = base.IBX.NewIbxObject(base.ObjectType.GetObjectType(), list);
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
                else
                {
                    throw new PSArgumentException(String.Format("The object type parameter was not an allowed value, {0} was provided.", val));
                }
            }
            else
            {
                throw new PSArgumentException("The object type parameter does not exist in the dynamic parameter dictionary.");
            }
        }

        #endregion
    }
}

using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace BAMCIS.Infoblox.PowerShell.Generic
{
    [Cmdlet(
        VerbsCommon.Set,
        "IBXObject",
        SupportsShouldProcess = true)]
    public class SetIbxObjectPSCmd : BaseIbxObjectPSCmd, IDynamicParameters
    {
        private object _inputObject;
        private InfoBloxObjectsEnum _type;
        private bool _removeEmpty;

        #region Parameters

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            ParameterSetName = "ByObject",
            HelpMessage = "The object to be updated."
            )]
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
                        throw new PSArgumentException(String.Format("The input object must be a valid infoblox type, {0} was provided.", value.GetType().Name));
                    }
                }
                else
                {
                    throw new PSArgumentNullException("The input object parameter cannot be null.");
                }
            }
        }
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "ByObject",
            HelpMessage = "Indicate that empty or null values should be removed from the object before sending."
            )]
        public bool RemoveEmptyProperties
        {
            get
            {
                return this._removeEmpty;
            }
            set
            {
                this._removeEmpty = value;
            }
        }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (!this.ParameterSetName.Equals("ByObject"))
            {
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
                case "ByAttribute":
                    this.ProcessByAttribute();
                    break;
                case "ByObject":
                    this.ProcessByObject();
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
                        base.Response = typeof(IBXCommonMethods).GetMethod("UpdateIbxObject").MakeGenericMethod(base.ObjectType.GetObjectType()).Invoke(base.IBX, new object[] { list }) as string;
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

        private void ProcessByObject()
        {
            bool RemoveEmpty = this._removeEmpty;
            if (!this.MyInvocation.BoundParameters.ContainsKey("RemoveEmptyProperties"))
            {
                int choice = 0;

                ChoiceDescription yes = new ChoiceDescription("&Remove", "Will remove empty and null values from the object during serialization.");
                ChoiceDescription no = new ChoiceDescription("&Keep", "Will keep empty and null values and overwrite any existing values.");
                Collection<ChoiceDescription> choices = new Collection<ChoiceDescription>() { yes, no };
                choice = Host.UI.PromptForChoice("Update Infoblox Object", ("Do you want to remove the empty properties when sending the object?"), choices, 0);

                switch (choice)
                {
                    default:
                    case 0:
                        RemoveEmpty = true;
                        break;
                    case 1:
                        RemoveEmpty = false;
                        break;
                }
            }

            try
            {
                this._inputObject.GetType().GetProperty("_ref").SetValue(this._inputObject, base.Ref);
                base.Response = base.IBX.UpdateIbxObject(this.InputObject, RemoveEmpty);
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

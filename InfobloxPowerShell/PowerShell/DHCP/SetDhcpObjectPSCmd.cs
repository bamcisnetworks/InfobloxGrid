using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace BAMCIS.Infoblox.PowerShell.DHCP
{
    [Cmdlet(VerbsCommon.Set,
        "IBXDhcpObject",
        SupportsShouldProcess = true
        )]
    public class SetDhcpObjectPSCmd : BaseIbxObjectPSCmd, IDynamicParameters
    {
        private object _inputObject;
        private InfoBloxObjectsEnum _type;
        private bool _removeEmpty;

        #region Parameters

        [Parameter(
          Position = 1,
          ValueFromPipelineByPropertyName = true,
          Mandatory = true,
          HelpMessage = "The dhcp object reference string."
           )]
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
                    throw new PSArgumentNullException("Reference", "The reference cannot be null or empty.");
                }
            }
        }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "ByAttribute",
            HelpMessage = "Set to define the new record by attributes.")]
        public SwitchParameter ByAttribute { get; set; }

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

                    if (type.IsInfobloxDhcpRecordType())
                    {
                        this._inputObject = value;
                    }
                    else
                    {
                        throw new PSArgumentException(String.Format("The input object must be a dhcp object type, {0} was provided.", value.GetType().Name));
                    }
                }
                else
                {
                    throw new PSArgumentNullException("InputObject", "The input oject cannot be null or empty.");
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
                RuntimeDefinedParameter param = IBXDynamicParameters.DhcpType(true);
                base.ParameterDictionary.Add(param.Name, param);

                string recordType = base.GetUnboundValue("DhcpType") as string;

                if (!String.IsNullOrEmpty(recordType))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(recordType, out this._type))
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
                case "ByObject":
                    this.ProcessByObject();
                    break;
                case "ByAttribute":
                    this.ProcessByAttribute();
                    break;
                default:
                    throw new PSArgumentException("Bad parameter set name.");
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
                base.Response = base.IBX.UpdateIbxObject(this._inputObject, RemoveEmpty);
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

        private void ProcessByAttribute()
        {
            if (base.ParameterDictionary.ContainsKey("DhcpType"))
            {
                string val = base.ParameterDictionary["DhcpType"].Value as string;
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
                        base.Response = typeof(IBXCommonMethods).GetMethod("UpdateIbxObject").MakeGenericMethod(base.ObjectType.GetObjectType()).Invoke(base.IBX, new object[] { base.ObjectType, list }) as string;
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
                    throw new PSArgumentException(String.Format("Dhcp object type parameter was not an allowed value, {0} was provided.", val));
                }
            }
            else
            {
                throw new PSArgumentException("The dhcp object type parameter does not exist in the dynamic parameter dictionary.");
            }
        }

        #endregion
    }
}

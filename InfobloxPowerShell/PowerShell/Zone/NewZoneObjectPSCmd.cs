using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.Zone
{
    [Cmdlet(
        VerbsCommon.New,
        "IBXZoneObject",
        SupportsShouldProcess = true
        )]
    public class NewZoneObjectPSCmd : BaseIbxObjectPSCmd, IDynamicParameters
    {
        private object _inputObject;
        private InfoBloxObjectsEnum _type;

        #region Parameters

        [Parameter(
           Mandatory = false,
           ValueFromPipelineByPropertyName = true,
           ParameterSetName = "ByAttribute",
           HelpMessage = "Set to define the new zone object by attributes.")]
        public SwitchParameter ByAttribute { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            ParameterSetName = "ByObject",
            HelpMessage = "The zone object to create."
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

                    if (type.IsInfobloxZoneType())
                    {
                        this._inputObject = value;
                    }
                    else
                    {
                        throw new PSArgumentException(String.Format("The input object must be a zone object type, {0} was provided.", value.GetType().Name));
                    }
                }
                else
                {
                    throw new PSArgumentNullException("InputObject", "The input oject cannot be null or empty.");
                }
            }
        }

        #endregion

        public override object GetDynamicParameters()
        {
            base.GetDynamicParameters();

            if (!this.ParameterSetName.Equals("ByObject"))
            {
                RuntimeDefinedParameter param = IBXDynamicParameters.ZoneType(true);
                base.ParameterDictionary.Add(param.Name, param);

                string zoneType = base.GetUnboundValue("ZoneType") as string;

                if (!String.IsNullOrEmpty(zoneType))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(zoneType, out this._type))
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

        private void ProcessByAttribute()
        {
            if (base.ParameterDictionary.ContainsKey("ZoneType"))
            {
                string val = base.ParameterDictionary["ZoneType"].Value as string;
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
                    throw new PSArgumentException(String.Format("The zone type parameter was not an allowed value.", val));
                }
            }
            else
            {
                throw new PSArgumentException("The zone type parameter does not exist in the dynamic parameter dictionary.");
            }
        }

        #endregion

    }
}

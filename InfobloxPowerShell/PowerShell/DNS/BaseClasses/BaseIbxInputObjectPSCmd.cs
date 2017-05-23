using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using System;
using System.Management.Automation;
using System.Reflection;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public abstract class BaseIbxInputObjectPSCmd : BaseIbxDnsPSCmd
    {
        private object _InputObject;

        [Parameter(
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._GRID_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "The new object to be created."
        )]
        [Parameter(
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._SESSION_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "The new object to be created."
        )]
        [Parameter(
            ValueFromPipeline = true,
            ParameterSetName = BaseIbxObjectPSCmd._ENTERED_SESSION_BY_OBJECT,
            Mandatory = true,
            HelpMessage = "The new object to be created."
        )]
        [ValidateNotNull()]
        public object InputObject
        {
            get
            {
                return this._InputObject;
            }
            set
            {
                Type ObjectType;

                if (value.GetType() == typeof(PSObject))
                {
                    PSObject NewPSObject = (PSObject)value;
                    ObjectType = NewPSObject.BaseObject.GetType();
                    value = typeof(PSExtensionMethods).GetMethod("ConvertPSObject").MakeGenericMethod(ObjectType).Invoke(typeof(PSExtensionMethods), new object[] { NewPSObject });
                }
                else
                {
                    ObjectType = value.GetType();
                }

                if (ObjectType.IsInfobloxType())
                {
                    this._InputObject = value;

                    PropertyInfo Info = this._InputObject.GetType().GetProperty("_ref");

                    if (Info != null)
                    {
                        base._Ref = this._InputObject.GetType().GetProperty("_ref").GetValue(this._InputObject) as string;
                    }
                }
                else
                {
                    throw new PSArgumentException($"The new object must be an Infoblox object type, {value.GetType().FullName} was provided.");
                }
            }
        }
    }
}

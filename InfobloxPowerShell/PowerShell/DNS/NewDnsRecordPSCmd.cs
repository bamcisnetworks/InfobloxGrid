using BAMCIS.Infoblox.Common;
using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    [Cmdlet(
        VerbsCommon.New,
        "IBXDnsRecord",
        SupportsShouldProcess = true
        )]
    public class NewDnsRecordPSCmd : BaseIbxDnsPSCmd, IDynamicParameters
    {
        private string _network;
        private object _inputObject;
        private InfoBloxObjectsEnum _type;

        #region Parameters

        [Parameter(
            Mandatory = true,
            ParameterSetName = "ByNextAvailableIp",
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The network to select the next available IP from.")]
        public string Network
        {
            get
            {
                return this._network;
            }
            set
            {
                NetworkAddressTest.IsIPv4CidrWithException(value, out this._network);
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
            HelpMessage = "The dns record object to create."
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

                    if (type.IsInfobloxDnsRecordType())
                    {
                        this._inputObject = value;
                    }
                    else
                    {
                        throw new PSArgumentException(String.Format("The input object must be a dns record type, {0} was provided.", value.GetType().Name));
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
                RuntimeDefinedParameter param = IBXDynamicParameters.RecordType(true);
                base.ParameterDictionary.Add(param.Name, param);

                string recordType = base.GetUnboundValue("RecordType") as string;

                if (!String.IsNullOrEmpty(recordType))
                {
                    if (Enum.TryParse<InfoBloxObjectsEnum>(recordType, out this._type))
                    {
                        base.ObjectType = this._type;

                        IEnumerable<RuntimeDefinedParameter> temp = new List<RuntimeDefinedParameter>();
                        if (this.ParameterSetName.Equals("NextAvailableIp"))
                        {
                            temp = IBXDynamicParameters.ObjectTypeProperties(base.ObjectType);
                            string[] fieldsToRemove = new string[] { "ipv4addr", "ipv6addr" };
                            temp = temp.Except(temp.Where(x => fieldsToRemove.Contains(x.Name)));
                        }
                        if (this.ParameterSetName.Equals("ByAttribute"))
                        {
                            temp = IBXDynamicParameters.ObjectTypeProperties(base.ObjectType, "ByAttribute");
                        }

                        foreach (RuntimeDefinedParameter pa in temp)
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
                case "ByNextAvailableIp":
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
            if (base.ParameterDictionary.ContainsKey("RecordType"))
            {
                string val = base.ParameterDictionary["RecordType"].Value as string;
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

                    if (this.ParameterSetName.Equals("ByNextAvailableIp"))
                    {
                        if (NetworkAddressTest.IsIPv4Cidr(this._network))
                        {
                            list.Add(new KeyValuePair<string, string>("ipv4addr", "func:nextavailableip:" + this._network));
                        }
                        else if (NetworkAddressTest.IsIPv6Cidr(this._network))
                        {
                            list.Add(new KeyValuePair<string, string>("ipv6addr", "func:nextavailableip:" + this._network));
                        }
                        else
                        {
                            throw new ArgumentException("The provided network was not a valid IPv4 or IPv6 network.");
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
                    throw new PSArgumentException(String.Format("Dns record type parameter was not an allowed value, {0} was provided.", val));
                }
            }
            else
            {
                throw new PSArgumentException("The dns record type parameter does not exist in the dynamic parameter dictionary.");
            }
        }

        #endregion
    }
}

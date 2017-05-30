using BAMCIS.Infoblox.Core;
using BAMCIS.Infoblox.InfobloxMethods;
using BAMCIS.Infoblox.PowerShell.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Net.Http;

namespace BAMCIS.Infoblox.PowerShell.DNS
{
    public abstract class BaseIbxDnsPSCmd : BaseIbxObjectPSCmd
    {
        private DnsMethods _IBX;

        protected new DnsMethods IBX
        {
            get
            {
                return this._IBX;
            }
        }

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

                    this._IBX = new DnsMethods(this.GridMaster, this.Version, this.Credential.UserName, this.Credential.Password, TimeSpan.FromSeconds(Timeout));
                }
                else if (this.ParameterSetName.StartsWith(_SESSION))
                {
                    this._IBX = new DnsMethods(this.Session, TimeSpan.FromSeconds(Timeout));
                }
                else if (this.ParameterSetName.StartsWith(_ENTERED_SESSION))
                {
                    this._IBX = new DnsMethods(TimeSpan.FromSeconds(Timeout));
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

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        protected override void ProcessByNewObject(object newObject)
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

        protected override void ProcessByUpdatedObject(object updatedObject, bool removeEmpty)
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

        protected override void ProcessByAttributeForNewObject(string attribute, List<KeyValuePair<string, string>> additionalProperties)
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

        protected override void ProcessByAttributeForNewObject(string attribute)
        {
            ProcessByAttributeForNewObject(attribute, null);
        }

        protected override void ProcessByAttributeForUpdatedObject(string attribute, List<KeyValuePair<string, string>> additionalProperties)
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

        protected override void ProcessByAttributeForUpdatedObject(string attribute)
        {
            ProcessByAttributeForUpdatedObject(attribute, null);
        }

    }
}

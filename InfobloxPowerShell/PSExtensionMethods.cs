using BAMCIS.Infoblox.InfobloxMethods;
using System;
using System.Collections;
using System.Management.Automation;
using System.Reflection;
using System.Threading.Tasks;

namespace BAMCIS.Infoblox.PowerShell
{
    public static class PSExtensionMethods
    {
        /// <summary>
        /// This is called indirectly by a number of PowerShell cmdlets in order to cast an InputObject
        /// to the underlying .NET class. This is because when the returned value from a cmdlet is sent
        /// down the pipeline, it is converted to a PSObject, so we need to cast it back to the baseobject 
        /// type.
        /// </summary>
        /// <typeparam name="T">The type of the Infoblox class underlying the PSObject</typeparam>
        /// <param name="value">The object passed in the pipeline</param>
        /// <returns>A converted object to the correct Infoblox class</returns>
        public static T ConvertPSObject<T>(this PSObject value)
        {
            if (value != null)
            {
                Type type = value.BaseObject.GetType();

                if (type == typeof(T) && InfobloxSDKExtensionMethods.IsInfobloxType(type))
                {
                    return (T)value.BaseObject;
                }
                else
                {
                    throw new PSArgumentException($"The PS Object must be a valid infoblox object type, {type.FullName} was provided.");
                }
            }
            else
            {
                throw new ArgumentNullException("value", "The PSObject cannot be null.");
            }
        }

        internal static T GetUnboundValue<T>(this PSCmdlet cmdlet, string paramName)
        {
                return cmdlet.GetUnboundValue<T>(paramName, -1);
        }

        internal static T GetUnboundValue<T>(this PSCmdlet cmdlet, int unnamedPosition)
        {
            return cmdlet.GetUnboundValue<T>(String.Empty, unnamedPosition);
        }

        private static T GetUnboundValue<T>(this PSCmdlet cmdlet, string paramName, int unnamedPosition)
        {
            if (cmdlet != null)
            {
                if (!String.IsNullOrEmpty(paramName))
                {
                    // If paramName isn't found, value at unnamedPosition will be returned instead
                    object Context = TryGetProperty(cmdlet, "Context");
                    object Processor = TryGetProperty(Context, "CurrentCommandProcessor");
                    object ParameterBinder = TryGetProperty(Processor, "CmdletParameterBinderController");
                    IEnumerable Args = TryGetProperty(ParameterBinder, "UnboundArguments") as System.Collections.IEnumerable;

                    if (Args != null)
                    {
                        bool IsSwitch = typeof(SwitchParameter) == typeof(T);

                        string CurrentParameterName = String.Empty;
                        //object UnnamedValue = null;
                        int i = 0;

                        foreach (object Arg in Args)
                        {
                            //Is the unbound argument associated with a parameter name
                            object IsParameterName = TryGetProperty(Arg, "ParameterNameSpecified");

                            //The parameter name for the argument was specified
                            if (IsParameterName != null && true.Equals(IsParameterName))
                            {
                                string ParameterName = TryGetProperty(Arg, "ParameterName") as string;
                                CurrentParameterName = ParameterName;

                                //If it's a switch parameter, there won't be a value following it, so just return a present switch
                                if (IsSwitch && String.Equals(CurrentParameterName, paramName, StringComparison.OrdinalIgnoreCase))
                                {
                                    return (T)(object)new SwitchParameter(true);
                                }

                                //Since we have a current parameter name, the next value in Args should be the value supplied
                                //to the argument, so we can head on to the next iteration
                                continue;
                            }

                            //We assume the previous iteration identified a parameter name, so this must be its
                            //value
                            object ParameterValue = TryGetProperty(Arg, "ArgumentValue");

                            //If the value we have grabbed had a parameter name specified,
                            //let's check to see if it's the desired parameter
                            if (CurrentParameterName != String.Empty)
                            {
                                //If the parameter name currently being assessed is equal to the provided param
                                //name, then return the value of the param
                                if (CurrentParameterName.Equals(paramName, StringComparison.OrdinalIgnoreCase))
                                {
                                    return ConvertParameter<T>(ParameterValue);
                                }
                                else
                                {
                                    //Since this wasn't the parameter name we were looking for, clear it out
                                    CurrentParameterName = String.Empty;
                                }
                            }
                            //Otherwise there wasn't a parameter name, so the argument must have been supplied positionally,
                            //check if the current index is the position whose value we want
                            //Since positional parameters have to be specified first, this will be evaluated and increment until
                            //we run out of parameters or find a parameter with a name/value
                            else if (i++ == unnamedPosition)
                            {
                                //UnnamedValue = ParameterValue;  // Save this for later in case paramName isn't found

                                //Just return the parameter value if the position matches what was specified
                                return ConvertParameter<T>(ParameterValue);
                            }

                            /* Trying to move this, it should only need to be cleared if it wasn't emptyu
                            // Found a value, so currentParameterName needs to be cleared for the check above
                            CurrentParameterName = String.Empty;
                            */
                        }

                        /* This shouldn't be needed, if the position was specified
                        if (UnnamedValue != null)
                        {
                            return ConvertParameter<T>(UnnamedValue);
                        }
                        */
                    }

                    return default(T);
                }
                else
                {
                    throw new ArgumentNullException("paramName", "The unbound value parameter name cannot be null or empty.");
                }
            }
            else
            {
                throw new ArgumentNullException("cmdlet", "The PSCmdlet cannot be null.");
            }
        }

        private static T ConvertParameter<T>(this object value)
        {
            if (value == null || object.Equals(value, default(T)))
            {
                return default(T);
            }

            PSObject PSObj = value as PSObject;

            if (PSObj != null)
            {
                return PSObj.BaseObject.ConvertParameter<T>();
            }

            if (value is T)
            {
                return (T)value;
            }

            var constructorInfo = typeof(T).GetConstructor(new[] { value.GetType() });

            if (constructorInfo != null)
            {
                return (T)constructorInfo.Invoke(new[] { value });
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private static object TryGetProperty(object instance, string fieldName)
        {
            // any access of a null object returns null. 
            if (instance == null || String.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            BindingFlags Flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public;

            try
            {
                PropertyInfo PropInfo = instance.GetType().GetProperty(fieldName, Flags);

                if (PropInfo != null)
                {
                    try
                    {
                        return PropInfo.GetValue(instance, null);
                    }
                    catch { }
                }

                // maybe it's a field
                FieldInfo FInfo = instance.GetType().GetField(fieldName, Flags);

                if (FInfo != null)
                {
                    try
                    {
                        return FInfo.GetValue(instance);
                    }
                    catch { }
                }
            }
            catch (Exception) { }

            // no match, return null.
            return null;
        }

        internal static async Task<object> InvokeGenericAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            if (@this != null)
            {
                if (obj != null)
                {
                    dynamic AwaitableItem = @this.Invoke(obj, parameters);
                    await AwaitableItem;
                    return AwaitableItem.GetAwaiter().GetResult();
                }
                else
                {
                    throw new ArgumentNullException("obj", "The object to invoke the method on cannot be null");
                }
            }
            else
            {
                throw new ArgumentNullException("this", "The method info parameter cannot be null.");
            }
        }
    }
}

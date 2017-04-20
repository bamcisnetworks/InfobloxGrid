using System;
using System.Management.Automation;
using BAMCIS.Infoblox.InfobloxMethods;

namespace BAMCIS.Infoblox.PowerShell
{
    internal static class PSExtensionMethods
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
        internal static T ConvertPSObject<T>(PSObject value)
        {
            Type type = value.BaseObject.GetType();

            if (type == typeof(T) && ExtensionMethods.IsInfobloxType(type))
            {
               return (T)value.BaseObject;
            }
            else
            {
                throw new ArgumentException($"The PS Object must be a valid infoblox object type, {type.FullName} was provided.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Core
{
    public static class ValidateUnknownArray
    {
        public static bool ValidateHomogenousArray(List<Type> types, object[] values, out List<object> returnValue)
        {
            if (types != null && types.Any())
            {
                if (values != null && values.Any())
                {
                    Type type = types[0].GetType();

                    if (values.ToList().Where(x => x.GetType().Equals(type)).Count().Equals(values.Length))
                    {
                        if (types.Contains(type))
                        {
                            returnValue = values.ToList();
                            return true;
                        }
                        else
                        {
                            returnValue = null;
                            throw new ArgumentException(String.Format("The type of the array must be one of the following: {0}.", String.Join(", ", types.Select(x => x.Name))));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("All objects in the members property must be of the same type.");
                    }
                }
                else
                {
                    returnValue = new List<object>();
                    return false;
                }
            }
            else
            {
                returnValue = null;
                return false;
            }
        }

        public static bool ValidateHetergenousArray(List<Type> types, object[] values, out List<object> returnValue)
        {
            if (types != null && types.Any())
            {
                if (values != null && values.Any())
                {
                    List<object> ReturnList = new List<object>();

                    foreach (object Value in values)
                    {
                        Type ValueType = Value.GetType();

                        if (types.Contains(ValueType))
                        {
                            object NewObject = Activator.CreateInstance(ValueType);

                            foreach (PropertyInfo PropInfo in ValueType.GetTypeInfo().GetProperties())
                            {
                                PropInfo.SetValue(NewObject, Value.GetType().GetTypeInfo().GetProperty(PropInfo.Name).GetValue(Value));
                            }

                            ReturnList.Add(NewObject);
                        }
                        else
                        {
                            returnValue = null;
                            throw new ArgumentException(String.Format("The type of the array must be one of the following: {0}.", String.Join(", ", types.Select(x => x.Name))));
                        }
                    }

                    returnValue = ReturnList.ToList();
                    return true;
                }
                else
                {
                    returnValue = new List<object>();
                    return false;
                }
            }
            else
            {
                returnValue = null;
                return false;
            }
        }
    }
}

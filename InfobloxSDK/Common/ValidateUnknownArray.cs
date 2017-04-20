using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BAMCIS.Infoblox.Common
{
    public static class ValidateUnknownArray
    {
        public static bool ValidateHomogenousArray(List<Type> Types, object[] value, out List<object> ReturnValue)
        {
            if (Types.Count > 0)
            {
                if (value.Length > 0)
                {
                    Type type = Types[0].GetType();

                    if (value.ToList().Where(x => x.GetType().Equals(type)).Count().Equals(value.Length))
                    {
                        if (Types.Contains(type))
                        {
                            ReturnValue = value.ToList();
                            return true;
                        }
                        else
                        {
                            ReturnValue = null;
                            throw new ArgumentException(String.Format("The type of the array must be one of the following: {0}.", String.Join(", ", Types.Select(x => x.Name))));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("All objects in the members property must be of the same type.");
                    }
                }
                else
                {
                    ReturnValue = new List<object>();
                    return false;
                }
            }
            else
            {
                ReturnValue = null;
                return false;
            }

        }

        public static bool ValidateHetergenousArray(List<Type> Types, object[] value, out List<object> ReturnValue)
        {
            if (Types.Count > 0)
            {
                if (value.Length > 0)
                {
                    List<object> returnList = new List<object>();
                    foreach (object val in value)
                    {
                        Type objType = val.GetType();
                        if (Types.Contains(objType))
                        {
                            object newObject = Activator.CreateInstance(objType);
                            foreach (PropertyInfo info in objType.GetTypeInfo().GetProperties())
                            {
                                info.SetValue(newObject, val.GetType().GetTypeInfo().GetProperty(info.Name).GetValue(val));
                            }
                            returnList.Add(newObject);
                        }
                        else
                        {
                            ReturnValue = null;
                            throw new ArgumentException(String.Format("The type of the array must be one of the following: {0}.", String.Join(", ", Types.Select(x => x.Name))));
                        }
                    }

                    ReturnValue = returnList.ToList();
                    return true;
                }
                else
                {
                    ReturnValue = new List<object>();
                    return false;
                }
            }
            else
            {
                ReturnValue = null;
                return false;
            }
        }
    }
}

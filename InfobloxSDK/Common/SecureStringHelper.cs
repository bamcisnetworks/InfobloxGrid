#if (NETSTANDARD2_0 || NETSTANDARD1_6 || NETSTANDARD1_5 || NETSTANDARD1_4 || NETSTANDARD1_3 || NETSTANDARD1_2 || NETSTANDARD1_1 || NETSTANDARD1_0)
#define NETSTANDARD 
#else
#define NET
#endif

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace BAMCIS.Infoblox.Common
{
    public static class SecureStringHelper
    {
        public static SecureString ToSecureString(this string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }
            else
            {
                SecureString result = new SecureString();
                foreach (char c in value.ToCharArray())
                {
                    result.AppendChar(c);
                }
                return result;
            }
        }

        public static String ToReadableString(this SecureString securePassword)
        {
            if (securePassword == null)
            {
                return null;
            }

            IntPtr Ptr = IntPtr.Zero;
            try
            {
#if NETSTANDARD
                Ptr = SecureStringMarshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(Ptr);
#else
                Ptr = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(Ptr);
#endif
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(Ptr);
            }
        }
    }
}

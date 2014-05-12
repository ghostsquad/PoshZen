namespace PoshZen
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;

    // http://msdn.microsoft.com/en-us/library/system.security.cryptography.protecteddata.aspx
    internal static class DataProtectionExtensions
    {
        #region Static Fields

        private static readonly byte[] AdditionalEntropy = { 9, 3, 9, 5, 0, 4, 1, 9, 5, 4, 3, 9, 7, 6, 9 };

        private static readonly Encoding Encoding = Encoding.Unicode;

        #endregion

        #region Public Methods and Operators

        public static string Protect(this string secret)
        {
            var bytesUnprotected = Encoding.GetBytes(secret);
            var bytesProtected = ProtectedData.Protect(bytesUnprotected, AdditionalEntropy, DataProtectionScope.CurrentUser);
            var protectedBase64 = Convert.ToBase64String(bytesProtected);
            return protectedBase64;
        }

        public static string Unprotect(this string encryptedSecret)
        {
            var bytesProtected = Convert.FromBase64String(encryptedSecret);
            var bytesUnprotected = ProtectedData.Unprotect(
                bytesProtected, 
                AdditionalEntropy, 
                DataProtectionScope.CurrentUser);

            var unprotectedString = Encoding.GetString(bytesUnprotected);

            return unprotectedString;
        }

        public static string Protect(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Protect(Marshal.PtrToStringUni(valuePtr));
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        #endregion        
    }
}
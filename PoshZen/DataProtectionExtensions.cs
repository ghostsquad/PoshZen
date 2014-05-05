namespace PoshZen
{
    using System.Security.Cryptography;

    // http://msdn.microsoft.com/en-us/library/system.security.cryptography.protecteddata.aspx
    internal static class DataProtectionExtensions
    {
        #region Static Fields

        private static readonly byte[] AdditionalEntropy = { 9, 3, 9, 5, 0, 4, 1, 9, 5, 4, 3, 9, 7, 6, 9 };

        #endregion

        #region Public Methods and Operators

        public static string Protect(this string secret)
        {
            var bytes = ProtectedData.Protect(secret.ToByteArray(), AdditionalEntropy, DataProtectionScope.CurrentUser);
            return bytes.GetString();
        }

        public static string Unprotect(this string encryptedSecret)
        {
            var bytes = ProtectedData.Unprotect(
                encryptedSecret.ToByteArray(), 
                AdditionalEntropy, 
                DataProtectionScope.CurrentUser);
            return bytes.GetString();
        }

        #endregion

        #region Methods

        private static string GetString(this byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        private static byte[] ToByteArray(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        #endregion
    }
}
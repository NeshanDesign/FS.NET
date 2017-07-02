using System;
using System.Security.Cryptography;
using System.Text;

namespace FsNet.Common.Extentions
{
    public static class HashingExtentions
    {
        public static string CreateHash(this string plain, HashAlgorithm hashAlgorithm = HashAlgorithm.Sha1, string salt = null)
        {
            var clearText = plain + (string.IsNullOrEmpty(salt) ? "" : salt);
            var clearData = Encoding.UTF8.GetBytes(clearText);

            using (var hashMethod = Create(hashAlgorithm))
            {
                var hashedBytes = hashMethod.ComputeHash(clearData);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool IsEqual(this string hash, string plain, HashAlgorithm hashAlgorithm = HashAlgorithm.Sha1, string salt = null)
        {
            return hash.Equals(plain.CreateHash(hashAlgorithm, salt));
        }

        private static System.Security.Cryptography.HashAlgorithm Create(HashAlgorithm hashAlgorithm)
        {
            switch (hashAlgorithm)
            {
                case HashAlgorithm.Md5:    return MD5.Create();
                case HashAlgorithm.Sha256: return new SHA256Managed();
                case HashAlgorithm.Sha384: return new SHA384Managed();
                case HashAlgorithm.Sha512: return new SHA512Managed();
                case HashAlgorithm.Sha1:   return new SHA1Managed();
                default:                   return new SHA1Managed();
            }
        }
    }

    public enum HashAlgorithm
    {
        Sha1, Sha256, Sha384, Sha512, Md5
    }
}

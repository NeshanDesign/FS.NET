using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ServiceStack;

namespace FsNet.Common.Extentions
{
    //you can create a factory to have different encryption mechanism
    //here, i only use TripleDES algorithm
    public static class CryptoExtentions
    {
        //key size must be 128 or 192 bit, 16 or 24 byte
        private static readonly string Key = string.Join("",Encoding.UTF8.
            GetBytes("it_is_a_key_put_it_in_config_or_resource").
            Select(t => t.ToString("x2")).
            ToArray());
        
        public static string Decrypt(this string encryptedString)
        {
            if (string.IsNullOrEmpty(encryptedString))
            {
                throw new ArgumentNullException(nameof(encryptedString));
            }

            var toEncryptArray = Convert.FromBase64String(encryptedString);
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = GetKey(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var decryptor = tdes.CreateDecryptor();
            var resultArray = decryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray).Replace('\0',' ');//remove extra nulls
        }

        public static string Encrypt(this string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
            {
                throw new ArgumentNullException(nameof(originalString));
            }

            var toEncryptArray = Encoding.UTF8.GetBytes(originalString);

            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = GetKey(),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            var encryptor = tdes.CreateEncryptor();
            var resultArray = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray);
        }

        private static byte[] GetKey()
        {
            var keyByteArray = Enumerable.Range(0, Key.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(Key.Substring(x, 2), 16))
                .ToArray();

            byte[] _key = null;
            if (keyByteArray.Length >= 24) // 192 bit key length, take just 24 first bytes
                _key = keyByteArray.Take(24).ToArray();
            else if (keyByteArray.Length >= 16) // 128 bit key length, take just 16 first bytes
                _key = keyByteArray.Take(16).ToArray();
            else
            {  
                // Create a 16 byte valid key by padding with zero
                var paddings = new List<byte>();
                for (var i = 0; i < 16 - keyByteArray.Length; i++) paddings.Add(0x0);
                keyByteArray = keyByteArray.Combine(paddings.ToArray());
                _key = keyByteArray;
            }
            return _key;
        }
    }

}
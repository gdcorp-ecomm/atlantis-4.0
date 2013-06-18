using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BotDetect.Crypto
{
    internal sealed class RijndaelEncryption
    {
        public const int IVSize = 16;
        public const int SaltSize = 16;

        // Considering the main use of symmetric encryption in Captcha context
        // (encrypting SessionIDs passed in Urls so Session-hijacking is harder)
        // and the common Session validity timeouts (20 minutes - 1 hour),
        // the usual recommendation of at least a 1000 key deriver iterations can
        // be eschewed for significant performance gains.
        // In case stronger encryption is needed for other purposes, this class
        // should probably be split into a "light" and a "heavy" variant.
        // Also, the number of iterations could be made a configuration parameter
        // so customers can use whichever value suits their application and the
        // required point on the security/performance continuum.
        public const int KeyDeriverIterations = 1; 

        private RijndaelEncryption() { }

        /// <summary>
        /// Encrypts a string with the given password
        /// </summary>
        /// <param name="toEncrypt">input UTF8 string</param>
        /// <param name="password">UTF8 password</param>
        /// <returns>Base64-encoded encrypted value</returns>
        public static string Encrypt(string plaintext, string password)
        {
            // get key and salt from password
            byte[] keyArray;
            byte[] saltArray;
            GetRijndaelKey(password, out keyArray, out saltArray);

            // get plaintext bytes
            byte[] plaintextArray = UTF8Encoding.UTF8.GetBytes(plaintext);

            // generate cyphertext bytes (prepended with the random IV used)
            byte[] cyphertextArray = Encrypt(plaintextArray, keyArray);

            // prepend random key salt to resulting cyphertext
            int cyphertextSize = cyphertextArray.Length;
            byte[] resultArray = new byte[SaltSize + cyphertextSize];
            Array.Copy(saltArray, 0, resultArray, 0, SaltSize);
            Array.Copy(cyphertextArray, 0, resultArray, SaltSize, cyphertextSize);

            // Base64 result
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        // Rijndael256 symmetric encryption
        public static byte[] Encrypt(byte[] plaintextArray, byte[] key)
        {
            // cypto settings
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Key = key;
            rijndael.GenerateIV();
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;

            // we need to keep this value and prepend it to the cyphertext
            byte[] ivArray = rijndael.IV;

            // crypto operation
            ICryptoTransform cTransform = rijndael.CreateEncryptor();
            byte[] cyphertextArray = cTransform.TransformFinalBlock(plaintextArray, 0, plaintextArray.Length);
            rijndael.Clear();

            // prepend random IV to cyphertext
            int cyphertextSize = cyphertextArray.Length;
            byte[] resultArray = new byte[IVSize + cyphertextSize];
            Array.Copy(ivArray, 0, resultArray, 0, IVSize);
            Array.Copy(cyphertextArray, 0, resultArray, IVSize, cyphertextSize);

            return resultArray;
        }

        /// <summary>
        /// Decrypts a string using the given password
        /// </summary>
        /// <param name="toDecrypt">input Base64 string</param>
        /// <param name="password">UTF8 password</param>
        /// <returns>UTF8 decryption result</returns>
        public static string Decrypt(string cyphertext, string password)
        {
            // get input bytes from Base64
            byte[] combinedInputArray = Convert.FromBase64String(cyphertext);

            // extract salt from input
            byte[] saltArray = new byte[SaltSize];
            Array.Copy(combinedInputArray, 0, saltArray, 0, SaltSize);

            // get key with from password and salt
            byte[] keyArray = GetRijndaelKey(password, saltArray);

            // extract IV from input
            byte[] ivArray = new byte[IVSize];
            Array.Copy(combinedInputArray, SaltSize, ivArray, 0, IVSize);

            // extract cyphertext from input
            int cyphertextSize = combinedInputArray.Length - SaltSize - IVSize;
            byte[] cyphertextArray = new byte[cyphertextSize];
            Array.Copy(combinedInputArray, SaltSize + IVSize, cyphertextArray, 0, cyphertextSize);

            // get plaintext bytes
            byte[] resultArray = Decrypt(cyphertextArray, keyArray, ivArray);

            // UTF8-encoded result
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static byte[] Decrypt(byte[] cyphertextArray, byte[] key, byte[] iv)
        {
            // crpyto settings
            RijndaelManaged rijndael = new RijndaelManaged();
            rijndael.Key = key;
            rijndael.IV = iv;
            rijndael.Mode = CipherMode.CBC;
            rijndael.Padding = PaddingMode.PKCS7;

            // crpyto operation
            ICryptoTransform cTransform = rijndael.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(cyphertextArray, 0, cyphertextArray.Length);
            rijndael.Clear();

            return resultArray;
        }

        public static void GetRijndaelKey(string password, out byte[] keyArray, out byte[] saltArray)
        {
            // salt is randomized
            Rfc2898DeriveBytes keyDeriver = new Rfc2898DeriveBytes(password, SaltSize, KeyDeriverIterations);
            saltArray = keyDeriver.Salt;

            // most secure Rijndael uses 256 key bits (32 bytes)
            keyArray = keyDeriver.GetBytes(32);
        }

        public static byte[] GetRijndaelKey(string password, byte[] saltArray)
        {
            // salt is randomized
            Rfc2898DeriveBytes keyDeriver = new Rfc2898DeriveBytes(password, saltArray, KeyDeriverIterations);

            // most secure Rijndael uses 256 key bits (32 bytes)
            byte[] keyArray = keyDeriver.GetBytes(32);

            return keyArray;
        }
    }
}

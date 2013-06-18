using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

using BotDetect.Crypto;

namespace BotDetect
{
    internal sealed class CryptoHelper
    {
        private CryptoHelper()
        {
            // constructor omitted, static methods only
        }

        /// <summary>
        /// Encrypts a string with the given password
        /// </summary>
        /// <param name="toEncrypt">input UTF8 string</param>
        /// <param name="password">UTF8 password</param>
        /// <returns>Base64-encoded encrypted value</returns>
        public static string Encrypt(string plaintext, string password)
        {
            return RijndaelEncryption.Encrypt(plaintext, password);
        }

        /// <summary>
        /// Decrypts a string using the given password
        /// </summary>
        /// <param name="toDecrypt">input Base64 string</param>
        /// <param name="password">UTF8 password</param>
        /// <returns>UTF8 decryption result</returns>
        public static string Decrypt(string cyphertext, string password)
        {
            return RijndaelEncryption.Decrypt(cyphertext, password);
        }
    }
}

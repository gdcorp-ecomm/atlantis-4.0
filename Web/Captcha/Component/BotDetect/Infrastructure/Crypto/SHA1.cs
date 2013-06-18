using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace BotDetect.Crypto
{
    internal sealed class SHA1
    {
        private SHA1() { }

        // 160bit (20byte) hashes 
        public static byte[] Hash(byte[] value)
        {
            byte[] hashValue;

            SHA1CryptoServiceProvider hash = new SHA1CryptoServiceProvider();
            hashValue = hash.ComputeHash(value);
            hash.Clear();

            return hashValue;
        }

        public static byte[] Hash(string value)
        {
            byte[] hashValue = Hash(UTF8Encoding.UTF8.GetBytes(value));
            return hashValue;
        }

        public static string HashBase64(byte[] value)
        {
            return Convert.ToBase64String(SHA1.Hash(value));
        }

        public static string HashBase64(string value)
        {
            return Convert.ToBase64String(SHA1.Hash(value));
        }

        // hash check
        public static bool IsHashMatch(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }

            return (Convert.ToBase64String(hash1) == Convert.ToBase64String(hash2));
        }
    }
}

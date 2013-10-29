using Atlantis.Framework.MiniEncrypt;

namespace Atlantis.Framework.BasePages.Cookies
{
    public static class CookieHelper
    {
        /// <summary>
        /// Encrypts a cookie value.
        /// </summary>
        /// <param name="decryptedValue"></param>
        /// <returns></returns>
        public static string EncryptCookieValue(string decryptedValue)
        {
            string result = null;

            if (!string.IsNullOrEmpty(decryptedValue))
            {
                result = CookieEncryption.CreateDisposable().EncryptCookieValue(decryptedValue);

                /*using (CookieCryptWrapper cookieCrypt = new CookieCryptWrapper())
                {
                    result = cookieCrypt.CookieCrypter.Encrypt(decryptedValue);
                }*/
            }

            return result;
        }

        /// <summary>
        /// Decrypts an encrypted value. If the input value is not encrypted, returns null.
        /// </summary>
        /// <param name="encryptedValue"></param>
        /// <returns></returns>
        public static string DecryptCookieValue(string encryptedValue)
        {
            string result = null;

            if (!string.IsNullOrEmpty(encryptedValue))
            {
                string decrypted = string.Empty;
                if (CookieEncryption.CreateDisposable().TryDecrypteCookieValue(encryptedValue, out decrypted))
                {
                    result = decrypted;
                }

                /*using (CookieCryptWrapper cookieCrypt = new CookieCryptWrapper())
                {
                    string decrypted = cookieCrypt.CookieCrypter.Decrypt(encryptedValue);
                    if (decrypted != encryptedValue)
                    {
                        result = decrypted;
                    }
                }*/
            }
            return result;
        }
    }
}

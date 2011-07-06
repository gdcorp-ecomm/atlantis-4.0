using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.BasePages.Cookies
{
  public static class CookieHelper
  {
    public static string EncryptCookieValue(string decryptedValue)
    {
      string result = null;

      if (!string.IsNullOrEmpty(decryptedValue))
      {
        using (CookieCryptWrapper cookieCrypt = new CookieCryptWrapper())
        {
          result = cookieCrypt.CookieCrypter.Encrypt(decryptedValue);
        }
      }

      return result;
    }

    /// <summary>
    /// Decrypts an encrypted value.  If the input value is not encrypted, returns null.
    /// </summary>
    /// <param name="encryptedValue"></param>
    /// <returns></returns>
    public static string DecryptCookieValue(string encryptedValue)
    {
      string result = null;

      if (!string.IsNullOrEmpty(encryptedValue))
      {
        using (CookieCryptWrapper cookieCrypt = new CookieCryptWrapper())
        {
          string decrypted = cookieCrypt.CookieCrypter.Decrypt(encryptedValue);
          if (decrypted != encryptedValue)
          {
            result = decrypted;
          }
        }
      }
      return result;
    }
  }
}

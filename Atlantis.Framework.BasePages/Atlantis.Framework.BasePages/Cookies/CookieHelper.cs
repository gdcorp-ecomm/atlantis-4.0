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
        using (var cookieEncrypt = CookieEncryption.CreateDisposable())
        {
          result = cookieEncrypt.EncryptCookieValue(decryptedValue);
        }
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
        using (var cookieEncrypt = CookieEncryption.CreateDisposable())
        {
          string decrypted;
          if (cookieEncrypt.TryDecrypteCookieValue(encryptedValue, out decrypted))
          {
            result = decrypted;
          }
        }
      }
      return result;
    }
  }
}

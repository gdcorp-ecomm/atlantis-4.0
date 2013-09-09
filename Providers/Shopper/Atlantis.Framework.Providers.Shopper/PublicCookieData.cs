using Atlantis.Framework.MiniEncrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.Providers.Shopper
{
  internal class PublicCookieData
  {
    public static string CreateEncrypted(string shopperId)
    {
      string result;

      using (var cookieHelper = CookieEncryption.CreateDisposable())
      {
        result = cookieHelper.EncryptCookieValue(shopperId);
      }

      return result;
    }

    public string ShopperId = string.Empty;

    internal PublicCookieData(string encryptedCookieValue)
    {
      if (!string.IsNullOrEmpty(encryptedCookieValue))
      {
        using (var cookieHelper = CookieEncryption.CreateDisposable())
        {
          string decryptedValue;
          if (cookieHelper.TryDecrypteCookieValue(encryptedCookieValue, out decryptedValue))
          {
            ShopperId = decryptedValue;
          }
        }
      }
    }
  }
}

using Atlantis.Framework.MiniEncrypt;
using System;

namespace Atlantis.Framework.Providers.Shopper
{
  internal class MemAuthCookieData
  {
    public static string CreateEncrypted(string shopperId, DateTime loginDate, int privateLabelId)
    {
      string delimitedData = shopperId + "|" + loginDate + "|" + privateLabelId;
      string memAuthData;

      using (var cookieHelper = CookieEncryption.CreateDisposable())
      {
        memAuthData = cookieHelper.EncryptCookieValue(delimitedData);
      }

      return memAuthData;
    }

    public string ShopperId = string.Empty;
    public string LoginDate = string.Empty;
    public string PrivateLabelId = string.Empty;

    internal MemAuthCookieData(string encryptedCookieValue)
    {
      string memAuthData = string.Empty;

      if (!string.IsNullOrEmpty(encryptedCookieValue))
      {
        using (var cookieHelper = CookieEncryption.CreateDisposable())
        {
          string decryptedValue;
          if (cookieHelper.TryDecrypteCookieValue(encryptedCookieValue, out decryptedValue))
          {
            memAuthData = decryptedValue;
          }
        }
      }

      if (!string.IsNullOrEmpty(memAuthData))
      {
        string[] parts = memAuthData.Split('|');
        if (parts.Length > 0)
        {
          ShopperId = parts[0];
        }

        if (parts.Length > 1)
        {
          LoginDate = parts[1];
        }

        if (parts.Length > 2)
        {
          PrivateLabelId = parts[2];
        }
      }
    }
  }
}

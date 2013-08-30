using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.MiniEncrypt
{
  public class CookieEncryption : IDisposable
  {
    private gdMiniEncryptLib.IgdCookie _cookieClass;

    private CookieEncryption()
    {
      _cookieClass = new gdMiniEncryptLib.gdCookie();
    }

    ~CookieEncryption()
    {
      _cookieClass.SafeRelease();
    }

    public void Dispose()
    {
      _cookieClass.SafeRelease();
      GC.SuppressFinalize(this);
    }

    public static CookieEncryption CreateDisposable()
    {
      return new CookieEncryption();
    }

    public string EncryptCookieValue(string decryptedValue)
    {
      string result = null;

      if (!string.IsNullOrEmpty(decryptedValue))
      {
        result = _cookieClass.Encrypt(decryptedValue);
      }

      return result;
    }

    public bool TryDecrypteCookieValue(string encryptedValue, out string decryptedValue)
    {
      decryptedValue = null;

      if (!string.IsNullOrEmpty(encryptedValue))
      {
        try
        {
          string decrypted = _cookieClass.Decrypt(encryptedValue);
          if (decrypted != encryptedValue)
          {
            decryptedValue = decrypted;
            return true;
          }
        }
        catch (COMException)
        {
          // Do not log. Most common exception is thrown when encrypted value is not really encrypted
          // return null (never allow unencrypted value to be returned)
        }
      }

      return false;
    }

  }
}

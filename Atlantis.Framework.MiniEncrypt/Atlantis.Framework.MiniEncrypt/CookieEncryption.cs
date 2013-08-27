using System;

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

    public string DecryptCookieValue(string encryptedValue)
    {
      string result = null;

      if (!string.IsNullOrEmpty(encryptedValue))
      {
        string decrypted = _cookieClass.Decrypt(encryptedValue);

        if (decrypted != encryptedValue)
        {
          result = decrypted;
        }
      }
      return result;
    }

  }
}

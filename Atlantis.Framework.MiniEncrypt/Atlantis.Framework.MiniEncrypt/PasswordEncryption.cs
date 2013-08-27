using System;

namespace Atlantis.Framework.MiniEncrypt
{
  public class PasswordEncryption : IDisposable
  {
    private gdMiniEncryptLib.IPassword _passwordClass;

    private PasswordEncryption()
    {
      _passwordClass = new gdMiniEncryptLib.Password();
    }

    ~PasswordEncryption()
    {
      _passwordClass.SafeRelease();
    }

    public void Dispose()
    {
      _passwordClass.SafeRelease();
      GC.SuppressFinalize(this);
    }

    public static PasswordEncryption CreateDisposable()
    {
      return new PasswordEncryption();
    }

    public string EncryptPassword(string password)
    {
      string result = null;

      if (!string.IsNullOrEmpty(password))
      {
        result = _passwordClass.Encrypt(password);
      }

      return result;
    }
  }
}

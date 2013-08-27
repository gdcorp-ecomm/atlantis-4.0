using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atlantis.Framework.MiniEncrypt
{
  public class PasswordEncryption : MiniEncryptBase
  {
    private gdMiniEncryptLib.IPassword _passwordClass;

    private PasswordEncryption()
    {
      _passwordClass = new gdMiniEncryptLib.Password();
    }

    ~PasswordEncryption()
    {
      ReleaseObject(_passwordClass);
    }

    public override void Dispose()
    {
      ReleaseObject(_passwordClass);
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

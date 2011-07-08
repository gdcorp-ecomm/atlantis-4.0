using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.Providers.Preferences.Cookies
{
  internal class CookieCryptWrapper : IDisposable
  {
    private gdMiniEncryptLib.gdCookieClass m_cookie = new gdMiniEncryptLib.gdCookieClass();

    public gdMiniEncryptLib.gdCookieClass CookieCrypter
    {
      get
      {
        return m_cookie;
      }
    }

    #region IDisposable Members

    public void Dispose()
    {
      if (m_cookie != null)
      {
        Marshal.ReleaseComObject(m_cookie);
        m_cookie = null;
      }
      GC.SuppressFinalize(this);
    }

    #endregion

    ~CookieCryptWrapper()
    {
      if (m_cookie != null)
      {
        Marshal.ReleaseComObject(m_cookie);
        m_cookie = null;
      }
    }
  }
}

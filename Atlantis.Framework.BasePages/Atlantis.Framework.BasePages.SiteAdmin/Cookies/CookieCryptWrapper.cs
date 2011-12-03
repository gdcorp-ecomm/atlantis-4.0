using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.BasePages.SiteAdmin.Cookies
{
  internal class CookieCryptWrapper : IDisposable
  {
    private gdMiniEncryptLib.gdCookie m_cookie = new gdMiniEncryptLib.gdCookie();

    public gdMiniEncryptLib.gdCookie CookieCrypter
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

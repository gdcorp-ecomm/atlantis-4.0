using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.BasePages.Authentication
{
  internal class AuthenticationWrapper : IDisposable
  {
    private gdMiniEncryptLib.AuthenticationClass _auth = new gdMiniEncryptLib.AuthenticationClass();

    public gdMiniEncryptLib.AuthenticationClass Authentication
    {
      get { return _auth; }
    }

    public void Dispose()
    {
      if (_auth != null)
      {
        Marshal.ReleaseComObject(_auth);
        _auth = null;
      }
      GC.SuppressFinalize(this);
    }

    ~AuthenticationWrapper()
    {
      if (_auth != null)
      {
        Marshal.ReleaseComObject(_auth);
        _auth = null;
      }
    }
  }
}

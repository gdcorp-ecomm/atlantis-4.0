using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.MiniEncrypt
{
  public abstract class MiniEncryptBase: IDisposable
  {
    protected void ReleaseObject(object comObject)
    {
      if (comObject != null)
      {
        Marshal.ReleaseComObject(comObject);
        comObject = null;
      }
    }

    public virtual void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}

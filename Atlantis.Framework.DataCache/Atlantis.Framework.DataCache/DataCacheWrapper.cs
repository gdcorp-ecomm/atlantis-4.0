using System;
using System.Runtime.InteropServices;

namespace Atlantis.Framework.DataCache
{
  class DataCacheWrapper : System.IDisposable
  {
    public gdDataCacheLib.AccessClass COMAccessClass;

    internal DataCacheWrapper()
    {
      COMAccessClass = new gdDataCacheLib.AccessClass();
    }

    ~DataCacheWrapper()
    {
      if (COMAccessClass != null)
      {
        Marshal.ReleaseComObject(COMAccessClass);
        COMAccessClass = null;
      }
      GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
      if (COMAccessClass != null)
      {
        Marshal.ReleaseComObject(COMAccessClass);
        COMAccessClass = null;
      }
    }
  }
}

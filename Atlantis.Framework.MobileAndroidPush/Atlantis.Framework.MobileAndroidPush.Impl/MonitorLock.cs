using System;
using System.Threading;

namespace Atlantis.Framework.MobileAndroidPush.Impl
{
  internal class MonitorLock : IDisposable
  {
    private readonly object _lockObject;

    public MonitorLock(object lockObject)
    {
      _lockObject = lockObject;
      Monitor.Enter(_lockObject);
    }

    public void Dispose()
    {
      Monitor.Exit(_lockObject);
    }
  }
}

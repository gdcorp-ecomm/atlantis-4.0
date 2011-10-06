using System;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  internal class SlimWrite : IDisposable
  {
    ReaderWriterLockSlim _slimLock;

    internal SlimWrite(ReaderWriterLockSlim slimLock)
    {
      _slimLock = slimLock;
      _slimLock.EnterWriteLock();
    }

    public void Dispose()
    {
      _slimLock.ExitWriteLock();
    }
  }
}

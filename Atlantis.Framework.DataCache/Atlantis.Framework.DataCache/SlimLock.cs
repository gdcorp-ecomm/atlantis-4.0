using System;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  internal class SlimLock : IDisposable
  {
    private ReaderWriterLockSlim _slimLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

    internal SlimRead GetReadLock()
    {
      return new SlimRead(_slimLock);
    }

    internal SlimWrite GetWriteLock()
    {
      return new SlimWrite(_slimLock);
    }

    public void Dispose()
    {
      if (_slimLock != null)
      {
        _slimLock.Dispose();
      }
    }
  }
}

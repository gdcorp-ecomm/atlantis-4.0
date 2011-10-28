using System;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  public class SlimLock : IDisposable
  {
    private ReaderWriterLockSlim _slimLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

    public SlimRead GetReadLock()
    {
      return new SlimRead(_slimLock);
    }

    public SlimWrite GetWriteLock()
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

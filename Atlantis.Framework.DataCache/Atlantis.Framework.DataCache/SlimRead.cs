using System;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  internal class SlimRead : IDisposable
  {
    ReaderWriterLockSlim _slimLock;

    internal SlimRead(ReaderWriterLockSlim slimLock)
    {
      _slimLock = slimLock;
      _slimLock.EnterReadLock();
    }

    public void Dispose()
    {
      _slimLock.ExitReadLock();
    }
  }
}

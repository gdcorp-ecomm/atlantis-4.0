using System;
using System.Threading;

namespace Atlantis.Framework.DotTypeCache
{
  public sealed class ConfigLock
  {
    const int TIME_OUT = 50000;

    ReaderWriterLock _privateLock;
    LockCookie _lockCookie;

    public ConfigLock()
    {
      _privateLock = new ReaderWriterLock();
      _lockCookie = new LockCookie();
    }

    ~ConfigLock()
    {
      if (_privateLock != null)
      {
        if (_privateLock.IsWriterLockHeld)
        {
          if (_privateLock.IsReaderLockHeld)
          {
            _privateLock.DowngradeFromWriterLock(ref  _lockCookie);
            _privateLock.ReleaseReaderLock();
          }
          else
            _privateLock.ReleaseWriterLock();
        }
        else if (_privateLock.IsReaderLockHeld)
        {
          _privateLock.ReleaseReaderLock();
        }
      }
    }

    public void Dispose()
    {
      if (_privateLock != null)
      {
        if (_privateLock.IsWriterLockHeld)
        {
          if (_privateLock.IsReaderLockHeld)
          {
            _privateLock.DowngradeFromWriterLock(ref _lockCookie);
            _privateLock.ReleaseReaderLock();
          }
          else
            _privateLock.ReleaseWriterLock();
        }
        else if (_privateLock.IsReaderLockHeld)
        {
          _privateLock.ReleaseReaderLock();
        }
      }
    }

    #region Public Members

    public bool IsReaderLockHeld
    {
      get { return _privateLock.IsReaderLockHeld; }
    }

    public bool IsWriterLockHeld
    {
      get { return _privateLock.IsWriterLockHeld; }
    }

    public bool GetReaderLock()
    {
      try
      {
        _privateLock.AcquireReaderLock(TIME_OUT);
      }
      catch (Exception)
      {
        if (_privateLock.IsReaderLockHeld)
          _privateLock.ReleaseReaderLock();
        throw;
      }

      return _privateLock.IsReaderLockHeld;
    }

    public bool GetWriterLock()
    {
      try
      {
        if (_privateLock.IsReaderLockHeld)
        {
          _lockCookie = _privateLock.UpgradeToWriterLock(TIME_OUT);
        }
        else
          _privateLock.AcquireWriterLock(TIME_OUT);

      }
      catch (Exception)
      {
        if (_privateLock.IsWriterLockHeld)
        {
          if (_privateLock.IsReaderLockHeld)
          {
            _privateLock.DowngradeFromWriterLock(ref _lockCookie);
            _privateLock.ReleaseReaderLock();
          }
          else
            _privateLock.ReleaseWriterLock();
        }
        else if (_privateLock.IsReaderLockHeld)
        {
          _privateLock.ReleaseReaderLock();
        }

        throw;
      }

      return _privateLock.IsWriterLockHeld;
    }

    public void ReleaseReaderLock()
    {
      if (_privateLock.IsReaderLockHeld)
        _privateLock.ReleaseReaderLock();
    }

    public void ReleaseWriterLock()
    {
      if (_privateLock.IsWriterLockHeld)
        _privateLock.ReleaseWriterLock();
    }

    #endregion

  }
}

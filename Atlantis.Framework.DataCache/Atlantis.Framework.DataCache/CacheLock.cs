using System;
using System.Threading;

namespace Atlantis.Framework.DataCache
{
  class CacheLock
  {

    #if DEBUG
        const int TIME_OUT = 50000;
    #else
        const int TIME_OUT = 5000;
    #endif

    ReaderWriterLock _lock;
    LockCookie _lockCookie;
    
    public CacheLock()
    {
      _lock = new ReaderWriterLock();
      _lockCookie = new LockCookie();
    }

    ~CacheLock()
    {
      if (_lock != null)
      {
        if (_lock.IsWriterLockHeld)
        {
          if (_lock.IsReaderLockHeld)
          {
            _lock.DowngradeFromWriterLock(ref  _lockCookie);
            _lock.ReleaseReaderLock();
          }
          else
            _lock.ReleaseWriterLock();
        }
        else if (_lock.IsReaderLockHeld)
        {
          _lock.ReleaseReaderLock();
        }
      }
    }
    
    public void Dispose()
    {
      if (_lock != null)
      {
        if (_lock.IsWriterLockHeld)
        {
          if (_lock.IsReaderLockHeld)
          {
            _lock.DowngradeFromWriterLock(ref _lockCookie);
            _lock.ReleaseReaderLock();
          }
          else
            _lock.ReleaseWriterLock();
        }
        else if (_lock.IsReaderLockHeld)
        {
          _lock.ReleaseReaderLock();
        }
      }
    }
    
    public bool IsReaderLockHeld
    {
      get { return _lock.IsReaderLockHeld; }
    }

    public bool IsWriterLockHeld
    {
      get { return _lock.IsWriterLockHeld; }
    }

    public bool GetReaderLock()
    {
      try
      {
        _lock.AcquireReaderLock(TIME_OUT);
      }
      catch (Exception ex)
      {
        if( _lock.IsReaderLockHeld )
          _lock.ReleaseReaderLock();
        throw ex;
      }

      return _lock.IsReaderLockHeld;
    }
    
    public bool GetWriterLock()
    {
      try
      {
        if (_lock.IsReaderLockHeld)
        {
          _lockCookie = _lock.UpgradeToWriterLock(TIME_OUT);
        }
        else
          _lock.AcquireWriterLock(TIME_OUT);

      }
      catch (Exception ex)
      {
        if (_lock.IsWriterLockHeld)
        {
          if (_lock.IsReaderLockHeld)
          {
            _lock.DowngradeFromWriterLock(ref _lockCookie);
            _lock.ReleaseReaderLock();
          }
          else
            _lock.ReleaseWriterLock();
        }
        else if (_lock.IsReaderLockHeld)
        {
          _lock.ReleaseReaderLock();
        }

        throw ex;
      }

      return _lock.IsWriterLockHeld;
    }

    public void ReleaseReaderLock()
    {
      if (_lock.IsReaderLockHeld)
        _lock.ReleaseReaderLock();
    }
    
    public void ReleaseWriterLock()
    {
      if( _lock.IsWriterLockHeld )
        _lock.ReleaseWriterLock();
    }
        
  }
}

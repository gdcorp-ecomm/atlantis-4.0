using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Atlantis.Framework.Engine
{
  internal class EngineRequestCache<T> where T : class
  {
#if DEBUG
    const int LOCK_TIME_OUT = 50000;
#else
    const int LOCK_TIME_OUT = 5000;
#endif

    private Dictionary<string, T> _requestItems;
    private ReaderWriterLockSlim _requestLock;

    public EngineRequestCache()
    {
      _requestLock = new ReaderWriterLockSlim();
      _requestItems = new Dictionary<string, T>(1000);
    }

    public T GetRequestObject(ConfigElement configItem)
    {
      T result = null;
      bool requestFound = false;

      if (_requestLock.TryEnterReadLock(LOCK_TIME_OUT))
      {
        try
        {
          requestFound = _requestItems.TryGetValue(configItem.ProgID, out result);
        }
        finally
        {
          _requestLock.ExitReadLock();
        }
      }
      else
      {
        string message = "Acquiring EngineRequestCache read lock timed out. (" + typeof(T).Name + ")" + configItem.ProgID;
        throw new TimeoutException(message);
      }

      // Load the ProgID and cache it if needed
      if (!requestFound)
      {
        Assembly loadedAssembly = Assembly.LoadFrom(configItem.Assembly);

        // Attempt to get the assembly's file version
        object[] fileVersionAttributes = loadedAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true);
        if ((fileVersionAttributes != null) && (fileVersionAttributes.Length > 0))
        {
          AssemblyFileVersionAttribute fileVersionAttribute = fileVersionAttributes[0] as AssemblyFileVersionAttribute;
          if (fileVersionAttribute != null)
          {
            configItem.AssemblyFileVersion = fileVersionAttribute.Version;
          }
        }

        object[] descriptionAttributes = loadedAssembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true);
        if ((descriptionAttributes != null) && (descriptionAttributes.Length > 0))
        {
          AssemblyDescriptionAttribute descriptionAttribute = descriptionAttributes[0] as AssemblyDescriptionAttribute;
          if (descriptionAttribute != null)
          {
            configItem.AssemblyDescription = descriptionAttribute.Description;
          }
        }

        result = (T)loadedAssembly.CreateInstance(configItem.ProgID);
        if (result == null)
        {
          string message = string.Concat("Class '", configItem.ProgID, "' not found in assembly ", configItem.Assembly);
          throw new ArgumentException(message);
        }

        if (_requestLock.TryEnterWriteLock(LOCK_TIME_OUT))
        {
          try
          {
            if (!_requestItems.ContainsKey(configItem.ProgID))
            {
              _requestItems.Add(configItem.ProgID, result);
            }
          }
          finally
          {
            _requestLock.ExitWriteLock();
          }
        }
        else
        {
          string message = "Acquiring EngineRequestCache write lock timed out. (" + typeof(T).Name + ")" + configItem.ProgID;
          throw new TimeoutException(message);
        }
      }
      
      return result;
    }

    public void Clear()
    {
      if (_requestLock.TryEnterWriteLock(LOCK_TIME_OUT))
      {
        try
        {
          _requestItems.Clear();
        }
        finally
        {
          _requestLock.ExitWriteLock();
        }
      }
    }

  }
}

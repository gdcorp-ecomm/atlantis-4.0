using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Atlantis.Framework.Engine
{
  public delegate void RequestCompletedDelegate(ICompletedRequest completedRequest);

  public class Engine
  {
    public static event RequestCompletedDelegate OnRequestCompleted;

    internal static EngineRequestCache<IRequest> RequestCache { get; private set; }
    internal static EngineRequestCache<IAsyncRequest> AsyncRequestCache { get; private set; }
    internal static EngineConfig Config { get; private set; }

    static Exception _lastLoggingException;
    static LoggingStatusType _loggingStatus;

    public static string EngineVersion { get; private set; }
    public static string InterfaceVersion { get; private set; }

    static Engine()
    {
      RequestCache = new EngineRequestCache<IRequest>();
      AsyncRequestCache = new EngineRequestCache<IAsyncRequest>();
      Config = new EngineConfig();

      _lastLoggingException = null;
      _loggingStatus = LoggingStatusType.WorkingNormally;

      EngineVersion = "0.0.0.0";
      InterfaceVersion = "0.0.0.0";

      try
      {
        object[] engineFileVersions = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
        if ((engineFileVersions != null) && (engineFileVersions.Length > 0))
        {
          AssemblyFileVersionAttribute engineFileVersion = engineFileVersions[0] as AssemblyFileVersionAttribute;
          if (engineFileVersion != null)
          {
            EngineVersion = engineFileVersion.Version;
          }
        }

        Type configElementType = typeof(Atlantis.Framework.Interface.ConfigElement);
        object[] interfaceFileVersions = configElementType.Assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
        if ((interfaceFileVersions != null) && (interfaceFileVersions.Length > 0))
        {
          AssemblyFileVersionAttribute interfaceFileVersion = interfaceFileVersions[0] as AssemblyFileVersionAttribute;
          if (interfaceFileVersion != null)
          {
            InterfaceVersion = interfaceFileVersion.Version;
          }
        }
      }
      catch { }

    }

    public static IResponseData ProcessRequest(RequestData request, int requestType)
    {
      SyncRequest syncRequest = new SyncRequest(request, requestType);
      syncRequest.Execute();

      CallRequestCompleted(syncRequest);

      if (!syncRequest.Success)
      {
        throw syncRequest.Exception;
      }

      return syncRequest.ResponseData;
    }

    public static IAsyncResult BeginProcessRequest(RequestData request, int requestType, AsyncCallback callback, object state)
    {
      AsyncRequestBegin asyncRequest = new AsyncRequestBegin(request, requestType, callback, state);
      asyncRequest.Execute();

      if (!asyncRequest.Success)
      {
        throw asyncRequest.Exception;
      }

      return asyncRequest.AsyncResult;
    }

    public static IResponseData EndProcessRequest(IAsyncResult asyncResult)
    {
      AsyncRequestEnd asyncRequest = new AsyncRequestEnd(asyncResult);
      asyncRequest.Execute();

      CallRequestCompleted(asyncRequest);

      if (!asyncRequest.Success)
      {
        throw asyncRequest.Exception;
      }

      return asyncRequest.ResponseData;
    }

    private static void CallRequestCompleted(ICompletedRequest completedRequest)
    {
      if (completedRequest != null)
      {
        var requestCompletedDelegate = OnRequestCompleted;
        if (requestCompletedDelegate != null)
        {
          try
          {
            requestCompletedDelegate(completedRequest);
          }
          catch (Exception ex)
          {
            string message = ex.Message + ex.StackTrace;
            AtlantisException exception = new AtlantisException("RequestCompletedDelegate", 0, message, completedRequest.ToString());
            Engine.LogAtlantisException(exception);
          }
        }
      }
    }

    #region Logging

    public static void LogAtlantisException(AtlantisException exception, IErrorLogger errorLogger)
    {
      try
      {
        if (errorLogger != null)
        {
          errorLogger.LogAtlantisException(exception);
          _loggingStatus = LoggingStatusType.WorkingNormally;
        }
        else
        {
          _loggingStatus = LoggingStatusType.NullLogger;
        }
      }
      catch (Exception ex)
      {
        _loggingStatus = LoggingStatusType.Error;
        _lastLoggingException = ex;
      }

    }

    public static void LogAtlantisException(AtlantisException exception)
    {
      LogAtlantisException(exception, EngineLogging.EngineLogger);
    }

    public static void LogAtlantisException<T>(AtlantisException exception) where T: IErrorLogger, new()
    {
      IErrorLogger errorLogger = new T();
      LogAtlantisException(exception, errorLogger);
    }

    public static LoggingStatusType LoggingStatus
    {
      get { return _loggingStatus; }
    }

    public static Exception LastLoggingError
    {
      get { return _lastLoggingException; }
    }

    #endregion

    public static void ReloadConfig()
    {
      Config.Load();
      ClearAssemblyCache();
    }

    private static void ClearAssemblyCache()
    {
      RequestCache.Clear();
      AsyncRequestCache.Clear();
    }

    public static IList<ConfigElement> GetConfigElements()
    {
      return Config.GetAllConfigs();
    }

    public static bool TryGetConfigElement(int requestType, out ConfigElement configElement)
    {
      return Config.TryGetConfigElement(requestType, out configElement);
    }
  }

}

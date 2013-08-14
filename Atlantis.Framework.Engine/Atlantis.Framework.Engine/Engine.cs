using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Atlantis.Framework.Engine
{
  /// <summary>
  /// Delegate for a post request event.
  /// </summary>
  /// <param name="completedRequest"><c>ICompletedRequest</c> that will be passed to the delegate</param>
  public delegate void RequestCompletedDelegate(ICompletedRequest completedRequest);

  /// <summary>
  /// The Atlantis Framework Engine.
  /// </summary>
  public class Engine
  {
    /// <summary>
    /// This event will get fired after every engine request.
    /// Please ensure that you do not trigger another engine call from inside this event.
    /// </summary>
    public static event RequestCompletedDelegate OnRequestCompleted;

    internal static EngineRequestCache<IRequest> RequestCache { get; private set; }
    internal static EngineRequestCache<IAsyncRequest> AsyncRequestCache { get; private set; }
    internal static EngineConfig Config { get; private set; }

    static Exception _lastLoggingException;
    static LoggingStatusType _loggingStatus;

    /// <summary>
    /// The FileVersion of the Engine Assembly
    /// </summary>
    public static string EngineVersion { get; private set; }

    /// <summary>
    /// The FileVersion of the Interface Assembly
    /// </summary>
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

    /// <summary>
    /// Executes the given request
    /// </summary>
    /// <param name="request"><c>RequestData</c> class that provides the inputs for the request.</param>
    /// <param name="requestType"><c>int</c> request type id that maps to the desired <c>IRequest</c> handler in your atlantis.config file.</param>
    /// <returns><c>IResponseData</c> that is output by the request handler.</returns>
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

    /// <summary>
    /// Executes the given requests BeginRequest for async pages
    /// </summary>
    /// <param name="request"><c>RequestData</c> class that provides the inputs for the request.</param>
    /// <param name="requestType"><c>int</c> request type id that maps to the desired <c>IRequest</c> handler in your atlantis.config file.</param>
    /// <param name="callback"></param>
    /// <param name="state"></param>
    /// <returns>IAsyncResult needed for ASP.NET async pages.</returns>
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

    /// <summary>
    /// Executes the EndRequest for an Engine request that was started with <b>BeginProcessRequest</b>
    /// </summary>
    /// <param name="asyncResult">async result provided by ASP.NET async page end task method</param>
    /// <returns><c>IResponseData</c> that is output by the request handler.</returns>
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

    /// <summary>
    /// Logs an exception to the given <c>IErrorLogger</c>
    /// </summary>
    /// <param name="exception"><c>AtlantisException</c> to log.</param>
    /// <param name="errorLogger">Class that implements <c>IErrorLogger</c></param>
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

    /// <summary>
    /// Logs an exception to the Engines active <c>IErrorLogger</c>.
    /// <para>You can override the default <c>IErrorLogger</c> used by the Engine by 
    /// setting the <c>EngineLogging.EngineLogger</c> static property in your application
    /// startup with your own class that implements <c>IErrorLogger</c>.</para>
    /// </summary>
    /// <param name="exception"><c>AtlantisException</c> to log.</param>
    public static void LogAtlantisException(AtlantisException exception)
    {
      LogAtlantisException(exception, EngineLogging.EngineLogger);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="T"/> that is <c>IEngineLogger</c> and Logs an exception to it.
    /// </summary>
    /// <typeparam name="T">Valid type that implements <c>IEngineLogger</c> and contains a parameterless constructor</typeparam>
    /// <param name="exception"><c>AtlantisException</c> to log.</param>
    public static void LogAtlantisException<T>(AtlantisException exception) where T: IErrorLogger, new()
    {

      IErrorLogger errorLogger = null;
      try
      {
        errorLogger = new T();
      }
      catch (Exception ex)
      {
        _loggingStatus = LoggingStatusType.Error;
        _lastLoggingException = ex;
      }

      if (errorLogger != null)
      {
        LogAtlantisException(exception, errorLogger);
      }

    }

    /// <summary>
    /// Returns the current state of ErrorLogging
    /// </summary>
    public static LoggingStatusType LoggingStatus
    {
      get { return _loggingStatus; }
    }

    /// <summary>
    /// Returns the last exception that has happened when attempting to log an error
    /// </summary>
    public static Exception LastLoggingError
    {
      get { return _lastLoggingException; }
    }

    #endregion

    /// <summary>
    /// Clears the cache of loaded <c>IRequest</c> handlers and reloads the specified atlantis.config
    /// </summary>
    public static void ReloadConfig(string configFilename)
    {
      if (!String.IsNullOrEmpty(configFilename))
      {
        Config.ConfigName = configFilename;
      }

      Config.Load();
      ClearAssemblyCache();
    }

    /// <summary>
    /// Clears the cache of loaded <c>IRequest</c> handlers and reloads the atlantis.config
    /// </summary>
    public static void ReloadConfig()
    {
      ReloadConfig(null);
    }

    /// <summary>
    /// Clears the cache of loaded <c>IRequest</c> handlers
    /// </summary>
    private static void ClearAssemblyCache()
    {
      RequestCache.Clear();
      AsyncRequestCache.Clear();
    }

    /// <summary>
    /// Creates and returns a list of all Config Elements from the atlantis.config
    /// </summary>
    /// <returns>list of all Config Elements from the atlantis.config</returns>
    public static IList<ConfigElement> GetConfigElements()
    {
      return Config.GetAllConfigs();
    }

    /// <summary>
    /// Attempts to find a config element of the given requestType
    /// </summary>
    /// <param name="requestType"><c>int</c> request type</param>
    /// <param name="configElement">output variable for <c>ConfigElement</c> if it is found.</param>
    /// <returns>true if the <c>ConfigElement</c> is found and the output variable is populated.</returns>
    public static bool TryGetConfigElement(int requestType, out ConfigElement configElement)
    {
      return Config.TryGetConfigElement(requestType, out configElement);
    }
  }

}

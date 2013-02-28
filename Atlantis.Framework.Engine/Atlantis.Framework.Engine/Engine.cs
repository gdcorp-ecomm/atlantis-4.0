using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Atlantis.Framework.Engine
{
  public delegate void ProcessRequestStartDelegate(RequestData requestData, int requestType, Guid requestId);
  public delegate void ProcessRequestCompleteDelegate(RequestData requestData, int requestType, Guid requestId, IResponseData oResponse);

  public class Engine
  {
    public static event ProcessRequestStartDelegate OnProcessRequestStart;
    public static event ProcessRequestCompleteDelegate OnProcessRequestComplete;

    static EngineRequestCache<IRequest> _requestCache;
    static EngineRequestCache<IAsyncRequest> _asyncRequestCache;

    static EngineConfig _engineConfig;

    static Exception _lastLoggingException;
    static LoggingStatusType _loggingStatus;

    public static string EngineVersion { get; private set; }
    public static string InterfaceVersion { get; private set; }

    // Thread-safe class initializer
    // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnbda/html/singletondespatt.asp
    static Engine()
    {
      _requestCache = new EngineRequestCache<IRequest>();
      _asyncRequestCache = new EngineRequestCache<IAsyncRequest>();

      _engineConfig = new EngineConfig();
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

    #region Standard Requests

    public static IResponseData ProcessRequest(RequestData request, int requestType)
    {
      IResponseData response = null;
      ConfigElement configItem = null;
      Stopwatch callTimer = null;

      try
      {
        configItem = _engineConfig.GetConfig(requestType);

        IRequest oIRequest = _requestCache.GetRequestObject(configItem);
        Guid requestId = Guid.NewGuid();

        ProcessRequestStartDelegate startDelegate = OnProcessRequestStart;
        if (startDelegate != null)
        {
          startDelegate(request, requestType, requestId);
        }

        callTimer = Stopwatch.StartNew();
        response = oIRequest.RequestHandler((RequestData)request, configItem);
        callTimer.Stop();

        ProcessRequestCompleteDelegate completeDelegate = OnProcessRequestComplete;
        if (completeDelegate != null)
        {
          completeDelegate(request, requestType, requestId, response);
        }
      }
      catch (AtlantisException ex)
      {
        if (callTimer != null)
        {
          callTimer.Stop();
        }

        if (configItem != null)
        {
          configItem.Stats.LogFailure(callTimer);
        }

        LogAtlantisException(ex);
        throw ex;
      }
      catch (Exception ex)
      {
        if (callTimer != null)
        {
          callTimer.Stop();
        }

        if (configItem != null)
        {
          configItem.Stats.LogFailure(callTimer);
        }

        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, false);
        System.Diagnostics.StackFrame sf = st.GetFrame(0);

        AtlantisException exAtlantis = new AtlantisException((RequestData)request,
                                                              sf.GetMethod().ToString(),
                                                              ex.Message.ToString(),
                                                              request.ToXML(),
                                                              ex);
        LogAtlantisException(exAtlantis);
        throw exAtlantis;
      }

      //Check for Exception in Response
      AtlantisException exTest = response.GetException();
      if (exTest != null)
      {
        if (configItem != null)
        {
          configItem.Stats.LogFailure(callTimer);
        }

        LogAtlantisException(exTest);
        throw exTest;
      }

      if (configItem != null)
      {
        configItem.Stats.LogSuccess(callTimer);
      }

      return response;
    }

    #endregion

    #region Async Requests

    public static IAsyncResult BeginProcessRequest(
      RequestData request, int requestType, AsyncCallback callback, object state)
    {
      ConfigElement configItem = null;
      IAsyncResult asyncResult = null;
      IAsyncRequest asyncRequest = null;

      try
      {
        configItem = _engineConfig.GetConfig(requestType);
        asyncRequest = _asyncRequestCache.GetRequestObject(configItem);
        asyncResult = asyncRequest.BeginHandleRequest(request, configItem, callback, state);
      }
      catch (AtlantisException ex)
      {
        if (configItem != null)
        {
          configItem.Stats.LogFailure(null);
        }

        LogAtlantisException(ex);
        throw ex;
      }
      catch (ThreadAbortException)
      {
      }
      catch (Exception ex)
      {
        if (configItem != null)
        {
          configItem.Stats.LogFailure(null);
        }

        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, false);
        System.Diagnostics.StackFrame sf = st.GetFrame(0);

        AtlantisException exAtlantis = new AtlantisException(request,
                                                             sf.GetMethod().ToString(),
                                                             ex.Message.ToString(),
                                                             request.ToXML());
        LogAtlantisException(exAtlantis);
        throw exAtlantis;
      }

      return asyncResult;
    }

    public static IResponseData EndProcessRequest(IAsyncResult asyncResult)
    {
      IAsyncRequest asyncRequest = null;
      IResponseData response = null;
      AsyncState asyncState = null;

      try
      {
        asyncState = asyncResult.AsyncState as AsyncState;

        if (asyncState != null)
        {
          asyncRequest = _asyncRequestCache.GetRequestObject(asyncState.Config);
          response = asyncRequest.EndHandleRequest(asyncResult);
          asyncState.CallTimer.Stop();
        }
        else
        {
          throw new AtlantisException(
            null, "Engine." + MethodBase.GetCurrentMethod().Name, 
            "Invalid AsyncState argument", string.Empty);
        }
      }
      catch (AtlantisException ex)
      {
        if ((asyncState != null) && (asyncState.Config != null))
        {
          asyncState.CallTimer.Stop();
          asyncState.Config.Stats.LogFailure(asyncState.CallTimer);
        }

        LogAtlantisException(ex);
        throw ex;
      }
      catch (Exception ex)
      {
        if ((asyncState != null) && (asyncState.Config != null))
        {
          asyncState.CallTimer.Stop();
          asyncState.Config.Stats.LogFailure(asyncState.CallTimer);
        }

        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(0, false);
        System.Diagnostics.StackFrame sf = st.GetFrame(0);

        AtlantisException exAtlantis = new AtlantisException(asyncState.RequestData,
                                                             sf.GetMethod().ToString(),
                                                             ex.Message.ToString(),
                                                             asyncState.RequestData.ToXML());
        LogAtlantisException(exAtlantis);
        throw exAtlantis;
      }

      //Check for Exception in Response
      AtlantisException exTest = response.GetException();
      if (exTest != null)
      {
        if ((asyncState != null) && (asyncState.Config != null))
        {
          asyncState.Config.Stats.LogFailure(asyncState.CallTimer);
        }

        LogAtlantisException(exTest);
        throw exTest;
      }

      if ((asyncState != null) && (asyncState.Config != null))
      {
        asyncState.Config.Stats.LogSuccess(asyncState.CallTimer);
      }

      return response;
    }

    #endregion

    #region Logging

    public static void LogAtlantisException(AtlantisException exAtlantis)
    {
      try
      {
        IErrorLogger errorLogger = EngineLogging.EngineLogger;
        if (errorLogger != null)
        {
          errorLogger.LogAtlantisException(exAtlantis);
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
      _engineConfig.Load();
      ClearAssemblyCache();
    }

    private static void ClearAssemblyCache()
    {
      _requestCache.Clear();
      _asyncRequestCache.Clear();
    }

    public static IList<ConfigElement> GetConfigElements()
    {
      return _engineConfig.GetAllConfigs();
    }

    public static bool TryGetConfigElement(int requestType, out ConfigElement configElement)
    {
      return _engineConfig.TryGetConfigElement(requestType, out configElement);
    }
  }

}

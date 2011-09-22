using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Engine
{
  public delegate void ProcessRequestStartDelegate(RequestData requestData, int requestType, Guid requestId);
  public delegate void ProcessRequestCompleteDelegate(RequestData requestData, int requestType, Guid requestId, IResponseData oResponse);

  public class Engine
  {
    public static event ProcessRequestStartDelegate OnProcessRequestStart;
    public static event ProcessRequestCompleteDelegate OnProcessRequestComplete;

    static EngineLock _requestLock;
    static Dictionary<string, IRequest> _requestItems;
    static EngineLock _asyncRequestLock;
    static Dictionary<string, IAsyncRequest> _asyncRequestItems;
    static EngineConfig _engineConfig;

    static Exception _lastLoggingException;
    static LoggingStatusType _loggingStatus;

    // Thread-safe class initializer
    // http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnbda/html/singletondespatt.asp
    static Engine()
    {
      _requestLock = new EngineLock();
      _requestItems = new Dictionary<string, IRequest>();

      _asyncRequestLock = new EngineLock();
      _asyncRequestItems = new Dictionary<string, IAsyncRequest>();

      _engineConfig = new EngineConfig();
      _lastLoggingException = null;
      _loggingStatus = LoggingStatusType.WorkingNormally;
    }

    #region Standard Requests

    public static IResponseData ProcessRequest(RequestData request, int requestType)
    {
      IResponseData response = null;
      ConfigElement configItem = null;

      try
      {
        configItem = _engineConfig.GetConfig(requestType);

        IRequest oIRequest = GetRequestObject(configItem);

        Guid requestId = Guid.NewGuid();

        if (OnProcessRequestStart != null)
        {
          OnProcessRequestStart.Invoke(request, requestType, requestId);
        }

        response = oIRequest.RequestHandler((RequestData)request, configItem);

        if (OnProcessRequestComplete != null)
        {
          OnProcessRequestComplete(request, requestType, requestId, response);
        }
      }
      catch (AtlantisException ex)
      {
        LogAtlantisException(ex);
        throw ex;
      }
      catch (Exception ex)
      {
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
        LogAtlantisException(exTest);
        throw exTest;
      }

      return response;
    }

    private static IRequest GetRequestObject(ConfigElement configItem)
    {
      IRequest request = null;

      // Avoid writer lock...check to see if value exist in cache
      _requestLock.GetReaderLock();
      bool requestFound = _requestItems.TryGetValue(configItem.ProgID, out request);
      _requestLock.ReleaseReaderLock();

      if (!requestFound)
      {
        Assembly loadedAssembly = Assembly.LoadFrom(configItem.Assembly);
        request = (IRequest)loadedAssembly.CreateInstance(configItem.ProgID);

        _requestLock.GetWriterLock();
        // Someone else could have put value in cache while waiting
        if (!_requestItems.ContainsKey(configItem.ProgID))
        {
          _requestItems.Add(configItem.ProgID, request);
        }
        _requestLock.ReleaseWriterLock();
      }

      return request;
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
        asyncRequest = GetAsyncRequestObject(configItem);
        asyncResult = asyncRequest.BeginHandleRequest(request, configItem, callback, state);
      }
      catch (AtlantisException ex)
      {
        LogAtlantisException(ex);
        throw ex;
      }
      catch (ThreadAbortException)
      {
      }
      catch (Exception ex)
      {
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
          asyncRequest = GetAsyncRequestObject(asyncState.Config);
          response = asyncRequest.EndHandleRequest(asyncResult);
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
        LogAtlantisException(ex);
        throw ex;
      }
      catch (Exception ex)
      {
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
        LogAtlantisException(exTest);
        throw exTest;
      }

      return response;
    }

    private static IAsyncRequest GetAsyncRequestObject(ConfigElement configItem)
    {
      IAsyncRequest asyncRequest = null;

      // Avoid writer lock...check to see if value exist in cache
      _asyncRequestLock.GetReaderLock();
      bool asyncRequestFound = _asyncRequestItems.TryGetValue(configItem.ProgID, out asyncRequest);
      _asyncRequestLock.ReleaseReaderLock();

      if (!asyncRequestFound)
      {
        Assembly loadedAssembly = Assembly.LoadFrom(configItem.Assembly);
        asyncRequest = (IAsyncRequest)loadedAssembly.CreateInstance(configItem.ProgID);

        _asyncRequestLock.GetWriterLock();
        // Someone else could have put value in cache while waiting
        if (!_asyncRequestItems.ContainsKey(configItem.ProgID))
        {
          _asyncRequestItems.Add(configItem.ProgID, asyncRequest);
        }
        _asyncRequestLock.ReleaseWriterLock();
      }

      return asyncRequest;
    }

    #endregion

    #region Logging

    public static void LogAtlantisException(AtlantisException exAtlantis)
    {
      try
      {
        using (gdSiteLog.WSCgdSiteLogService oLog = new Atlantis.Framework.Engine.gdSiteLog.WSCgdSiteLogService())
        {
          oLog.Url = _engineConfig.LogWebServiceUrl;

          // Get some defaults
          string sourceServer = exAtlantis.SourceServer;
          if (string.IsNullOrEmpty(sourceServer))
          {
            sourceServer = Environment.MachineName;
          }

          string errorDescription = exAtlantis.ErrorDescription;
          if (string.IsNullOrEmpty(errorDescription))
          {
            Exception ex = exAtlantis.GetBaseException();
            if (ex != null)
            {
              errorDescription = ex.Message + Environment.NewLine + ex.StackTrace;
            }
          }

          oLog.LogErrorEx(sourceServer, exAtlantis.SourceFunction, exAtlantis.SourceURL,
                          uint.Parse(exAtlantis.ErrorNumber), errorDescription,
                          exAtlantis.ExData, exAtlantis.ShopperID, exAtlantis.OrderID,
                          exAtlantis.ClientIP, exAtlantis.Pathway, exAtlantis.PageCount);
          _loggingStatus = LoggingStatusType.WorkingNormally;
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
      _asyncRequestLock.GetWriterLock();
      _asyncRequestItems.Clear();
      _asyncRequestLock.ReleaseWriterLock();

      _requestLock.GetWriterLock();
      _requestItems.Clear();
      _requestLock.ReleaseWriterLock();
    }

    public static IList<ConfigElement> GetConfigElements()
    {
      return _engineConfig.GetAllConfigs();
    }

  }

}

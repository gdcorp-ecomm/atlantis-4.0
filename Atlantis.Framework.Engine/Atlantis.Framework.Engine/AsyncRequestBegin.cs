using Atlantis.Framework.Interface;
using System;
using System.Diagnostics;

namespace Atlantis.Framework.Engine
{
  internal class AsyncRequestBegin
  {
    private RequestData _requestData = null;
    private ConfigElement _config = null;
    private IAsyncRequest _asyncHandler = null;
    private AtlantisException _exception = null;
    private AsyncCallback _callback = null;
    private object _state = null;
    private IAsyncResult _asyncResult = null;
    private Stopwatch _timer = null;

    internal AsyncRequestBegin(RequestData requestData, int requestType, AsyncCallback callback, object state)
    {
      _requestData = requestData;
      _callback = callback;
      _state = state;

      try
      {
        _config = Engine.Config.GetConfig(requestType);
        _asyncHandler = Engine.AsyncRequestCache.GetRequestObject(_config);
      }
      catch (Exception ex)
      {
        string description = ex.Message;
        AtlantisException exception = new AtlantisException(requestData, "AsyncRequestBegin", ex.Message + ex.StackTrace, "RequestType=" + requestType);
        Engine.LogAtlantisException(exception);
        throw exception;
      }
    }

    internal void Execute()
    {
      _timer = Stopwatch.StartNew();

      try
      {
        _asyncResult = _asyncHandler.BeginHandleRequest(_requestData, _config, _callback, _state);

        if (_asyncResult == null)
        {
          string message = "Handler returned a null IAsyncResult.";
          string data = "RequestType=" + _config.RequestType.ToString() + ":" + _asyncHandler.GetType().FullName;
          _exception = new AtlantisException(_requestData, "AsyncRequestBegin.Execute", message, data);
        }
      }
      catch (Exception ex)
      {
        _exception = ex as AtlantisException;
        if (_exception == null)
        {
          string message = ex.Message + ex.StackTrace;
          string data = "RequestType=" + _config.RequestType.ToString() + ":" + _asyncHandler.GetType().FullName;
          _exception = new AtlantisException(_requestData, "AsyncRequestBegin.Execute", message, data, ex);
        }
      }
      finally
      {
        _timer.Stop();
      }

      if (_exception != null)
      {
        _config.Stats.LogFailure(_timer);
      }
    }

    public bool Success
    {
      get { return _exception == null; }
    }

    public AtlantisException Exception
    {
      get { return _exception; }
    }

    public IAsyncResult AsyncResult
    {
      get { return _asyncResult; }
    }


  }
}

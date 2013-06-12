using Atlantis.Framework.Interface;
using System;
using System.Diagnostics;
using System.Text;

namespace Atlantis.Framework.Engine
{
  internal class SyncRequest : ICompletedRequest
  {
    private RequestData _requestData = null;
    private ConfigElement _config = null;
    private IRequest _syncHandler = null;
    private IResponseData _responseData = null;
    private AtlantisException _exception = null;
    private Stopwatch _timer = null;

    internal SyncRequest(RequestData requestData, int requestType)
    {
      _requestData = requestData;

      try
      {
        _config = Engine.Config.GetConfig(requestType);
        _syncHandler = Engine.RequestCache.GetRequestObject(_config);
      }
      catch (Exception ex)
      {
        string description = ex.Message;
        AtlantisException exception = new AtlantisException(requestData, "SyncRequest", ex.Message + ex.StackTrace, "RequestType=" + requestType);
        Engine.LogAtlantisException(exception);
        throw exception;
      }
    }

    internal void Execute()
    {
      _timer = Stopwatch.StartNew();

      try
      {
        _responseData = _syncHandler.RequestHandler(_requestData, _config);

        if (_responseData == null)
        {
          string message = "Handler returned a null response.";
          string data = "RequestType=" + _config.RequestType.ToString() + ":" + _syncHandler.GetType().FullName;
          _exception = new AtlantisException(_requestData, "SyncRequest.Execute", message, data);
        }
        else
        {
          _exception = _responseData.GetException();
        }
      }
      catch (Exception ex)
      {
        _exception = ex as AtlantisException;
        if (_exception == null)
        {
          string message = ex.Message + ex.StackTrace;
          string data = "RequestType=" + _config.RequestType.ToString() + ":" + _syncHandler.GetType().FullName;
          _exception = new AtlantisException(_requestData, "SyncRequest.Execute", message, data, ex);
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
      else
      {
        _config.Stats.LogSuccess(_timer);
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

    public IResponseData ResponseData
    {
      get { return _responseData; }
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder("Request: config=");

      if (_config != null)
      {
        sb.Append(_config.RequestType);
      }
      else
      {
        sb.Append("null");
      }

      if (_requestData == null)
      {
        sb.Append(";request=null");
      }

      if (_responseData == null)
      {
        sb.Append(";response=null");
      }

      if (_exception != null)
      {
        sb.Append(";exception=");
        sb.Append(_exception.Message);
      }

      return sb.ToString();
    }


    public ConfigElement Config
    {
      get { return _config; }
    }

    public RequestData RequestData
    {
      get { return _requestData; }
    }

    public TimeSpan ElapsedTime
    {
      get
      {
        return (_timer != null) ? _timer.Elapsed : TimeSpan.Zero;
      }
    }
  }
}

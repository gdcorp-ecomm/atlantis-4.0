using Atlantis.Framework.Interface;
using System;
using System.Text;

namespace Atlantis.Framework.Engine
{
  internal class AsyncRequestEnd : ICompletedRequest
  {
    private IAsyncRequest _asyncHandler = null;
    private AtlantisException _exception = null;
    private AsyncState _asyncState;
    private IResponseData _responseData = null;
    private IAsyncResult _asyncResult;

    internal AsyncRequestEnd(IAsyncResult asyncResult)
    {
      _asyncResult = asyncResult;

      try
      {
        _asyncState = (AsyncState)asyncResult.AsyncState;

        if (_asyncState.Config == null)
        {
          throw new ArgumentException("AsyncState.Config is null.");
        }

        _asyncHandler = Engine.AsyncRequestCache.GetRequestObject(_asyncState.Config);
      }
      catch (Exception ex)
      {
        string description = ex.Message;
        RequestData requestData = null;
        string requestType = "unknown";

        if (_asyncState != null)
        {
          requestData = _asyncState.RequestData;
          if (_asyncState.Config != null)
          {
            requestType = _asyncState.Config.RequestType.ToString();
          }
        }

        AtlantisException exception = new AtlantisException(requestData, "AsyncRequestEnd", ex.Message + ex.StackTrace, "RequestType=" + requestType);
        Engine.LogAtlantisException(exception);
        throw exception;
      }
    }

    internal void Execute()
    {
      try
      {
        _responseData = _asyncHandler.EndHandleRequest(_asyncResult);

        if (_responseData == null)
        {
          string message = "Handler returned a null response.";
          string data = "RequestType=" + _asyncState.Config.RequestType.ToString() + ":" + _asyncHandler.GetType().FullName;
          _exception = new AtlantisException(_asyncState.RequestData, "AsyncRequestEnd.Execute", message, data);
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
          string data = "RequestType=" + _asyncState.Config.RequestType.ToString() + ":" + _asyncHandler.GetType().FullName;
          _exception = new AtlantisException(_asyncState.RequestData, "AsyncRequestEnd.Execute", message, data, ex);
        }
      }
      finally
      {
        _asyncState.CallTimer.Stop();
      }

      if (_exception != null)
      {
        _asyncState.Config.Stats.LogFailure(_asyncState.CallTimer);
      }
      else
      {
        _asyncState.Config.Stats.LogSuccess(_asyncState.CallTimer);
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

      if ((_asyncState != null) && (_asyncState.Config != null))
      {
        sb.Append(_asyncState.Config.RequestType);
      }
      else
      {
        sb.Append("null");
      }

      if ((_asyncState != null) && (_asyncState.RequestData == null))
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
      get { return (_asyncState != null) ? _asyncState.Config : null; }
    }

    public RequestData RequestData
    {
      get { return (_asyncState != null) ? _asyncState.RequestData : null; }
    }

    public TimeSpan ElapsedTime
    {
      get 
      {
        TimeSpan result = TimeSpan.Zero;
        if ((_asyncState != null) && (_asyncState.CallTimer != null))
        {
          result = _asyncState.CallTimer.Elapsed;
        }
        return result;
      }
    }
  }
}


namespace Atlantis.Framework.Interface
{
  public class AsyncState
  {
    private RequestData _requestData;
    private ConfigElement _config;
    private object _requestObject;
    private object _state;

    public AsyncState(RequestData requestData, ConfigElement config, object request, object state)
    {
      _requestData = requestData;
      _config = config;
      _requestObject = request;
      _state = state;
    }

    public RequestData RequestData
    {
      get { return _requestData; }
    }

    public ConfigElement Config
    {
      get { return _config; }
    }

    public object Request
    {
      get { return _requestObject; }
    }

    public object State
    {
      get { return _state; }
    }
  }
}

using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Providers.Localization.Tests.Mocks.Http
{
  public class MockHttpContext : HttpContextBase
  {
    public MockHttpContext(HttpRequestBase request, HttpResponseBase response = null, HttpSessionStateBase session = null,
                           HttpApplicationStateBase application = null) : base()
    {
      _request = request;
      _response = response;
      _session = session;
      _application = application;
    }

    private HttpRequestBase _request = null;
    public override HttpRequestBase Request { get { return _request; } }

    private HttpResponseBase _response = null;
    public override HttpResponseBase Response { get { return _response; } }

    private HttpSessionStateBase _session = null;
    public override HttpSessionStateBase Session { get { return _session; } }

    private HttpApplicationStateBase _application = null;
    public override HttpApplicationStateBase Application { get { return _application; } }

    public override void RewritePath(string path)
    {
      HttpContext.Current.RewritePath(path);
    }

    private Dictionary<string, object> _items = new Dictionary<string, object>();
    public override System.Collections.IDictionary Items
    {
      get
      {
        return _items;
      }
    }
  }
}

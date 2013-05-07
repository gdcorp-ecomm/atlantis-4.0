using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Atlantis.Framework.Providers.SplitTesting.Monitor.Web
{
  public class SplitTestingMonitorRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new T();
    }
  }

  public class SplitTestingMonitorRouteHandler : IRouteHandler
  {
    private readonly string _virtualPath;

    public SplitTestingMonitorRouteHandler(string virtualPath)
    {
      _virtualPath = virtualPath;
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      var result = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(IHttpHandler));
      return result;
    }
  }
}

using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Atlantis.Framework.DotTypeCache.Monitor.Web
{
  public class DotTypeCacheMonitorRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new T();
    }
  }

  public class DotTypeCacheMonitorRouteHandler : IRouteHandler
  {
    private string _virtualPath;

    public DotTypeCacheMonitorRouteHandler(string virtualPath)
    {
      _virtualPath = virtualPath;
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      IHttpHandler result = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(_virtualPath, typeof(IHttpHandler));
      return result;
    }
  }
}

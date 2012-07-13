using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Atlantis.Framework.Engine.Monitor.Web
{
  public class EngineMonitorRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new T();
    }
  }

  public class EngineMonitorRouteHandler : IRouteHandler
  {
    private string _virtualPath = null;

    public EngineMonitorRouteHandler(string virtualPath)
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

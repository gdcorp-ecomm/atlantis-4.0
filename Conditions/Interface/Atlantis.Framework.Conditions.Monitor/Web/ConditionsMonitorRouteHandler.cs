using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Atlantis.Framework.Conditions.Monitor.Web
{
  public class ConditionsMonitorRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new T();
    }
  }

  public class ConditionsMonitorRouteHandler : IRouteHandler
  {
    private readonly string _virtualPath;

    public ConditionsMonitorRouteHandler(string virtualPath)
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

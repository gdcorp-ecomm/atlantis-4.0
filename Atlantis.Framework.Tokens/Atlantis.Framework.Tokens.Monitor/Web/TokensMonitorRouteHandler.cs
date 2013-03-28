using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Atlantis.Framework.Tokens.Monitor.Web
{
  public class TokensMonitorRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new T();
    }
  }

  public class TokensMonitorRouteHandler : IRouteHandler
  {
    private readonly string _virtualPath;

    public TokensMonitorRouteHandler(string virtualPath)
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

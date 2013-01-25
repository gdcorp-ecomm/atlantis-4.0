using System.Web;
using System.Web.Routing;

namespace Atlantis.Framework.DotTypeCache.Monitor.Web
{
  public static class DotTypeCacheMonitorRouteCollectionExtensions
  {
    public static void MapDotTypeCacheMonitorHandler(this RouteCollection routes)
    {
      routes.MapHttpHandler<DotTypeCacheMonitorHttpHandler>("_dottypecache/monitor/{*routeQuery}");
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string url) where T : IHttpHandler, new()
    {
      routes.MapHttpHandler<T>(null, url, null, null);
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string name, string url, object defaults, object constraints) where T : IHttpHandler, new()
    {
      var route = new Route(url, new DotTypeCacheMonitorRouteHandler<T>());
      route.Defaults = new RouteValueDictionary(defaults);
      route.Constraints = new RouteValueDictionary(constraints);
      routes.Add(name, route);
    }
  }
}
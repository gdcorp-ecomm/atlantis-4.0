using System.Web;
using System.Web.Routing;

namespace Atlantis.Framework.Engine.Monitor.Web
{
  public static class EngineMonitorRouteCollectionExtensions
  {
    private static void MapHttpHandler<T>(this RouteCollection routes, string name, string url, object defaults, object constraints) where T : IHttpHandler, new()
    {
      var route = new Route(url, new EngineMonitorRouteHandler<T>());
      route.Defaults = new RouteValueDictionary(defaults);
      route.Constraints = new RouteValueDictionary(constraints);
      routes.Add(name, route);
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string url) where T : IHttpHandler, new()
    {
      routes.MapHttpHandler<T>(null, url, null, null);
    }

    public static void MapEngineMonitorHandler(this RouteCollection routes)
    {
      routes.MapHttpHandler<EngineMonitorHttpHandler>("_engine/monitor/{*routeQuery}");
    }

  }
}

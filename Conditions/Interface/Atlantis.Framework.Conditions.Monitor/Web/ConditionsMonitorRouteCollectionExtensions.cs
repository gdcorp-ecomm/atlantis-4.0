using System.Web;
using System.Web.Routing;

namespace Atlantis.Framework.Conditions.Monitor.Web
{
  public static class ConditionsMonitorRouteCollectionExtensions
  {
    public static void MapConditionsMonitorHandler(this RouteCollection routes)
    {
      routes.MapHttpHandler<ConditionsMonitorHttpHandler>("_conditions/monitor/{*routeQuery}");
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string url) where T : IHttpHandler, new()
    {
      routes.MapHttpHandler<T>(null, url, null, null);
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string name, string url, object defaults, object constraints) where T : IHttpHandler, new()
    {
      var route = new Route(url, new ConditionsMonitorRouteHandler<T>());
      route.Defaults = new RouteValueDictionary(defaults);
      route.Constraints = new RouteValueDictionary(constraints);
      routes.Add(name, route);
    }
  }
}
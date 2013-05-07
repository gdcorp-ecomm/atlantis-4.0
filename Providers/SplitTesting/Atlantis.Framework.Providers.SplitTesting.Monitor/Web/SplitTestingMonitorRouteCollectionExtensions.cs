using System.Web;
using System.Web.Routing;

namespace Atlantis.Framework.Providers.SplitTesting.Monitor.Web
{
  public static class SplitTestingMonitorRouteCollectionExtensions
  {
    public static void MapSplitTestingMonitorHandler(this RouteCollection routes)
    {
      routes.MapHttpHandler<SplitTestingMonitorHttpHandler>("_splittesting/monitor/{*routeQuery}");
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string url) where T : IHttpHandler, new()
    {
      routes.MapHttpHandler<T>(null, url, null, null);
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string name, string url, object defaults, object constraints) where T : IHttpHandler, new()
    {
      var route = new Route(url, new SplitTestingMonitorRouteHandler<T>());
      route.Defaults = new RouteValueDictionary(defaults);
      route.Constraints = new RouteValueDictionary(constraints);
      routes.Add(name, route);
    } 
  }
}
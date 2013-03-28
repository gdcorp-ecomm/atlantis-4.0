using System.Web;
using System.Web.Routing;

namespace Atlantis.Framework.Tokens.Monitor.Web
{
  public static class TokensMonitorRouteCollectionExtensions
  {
    public static void MapTokensMonitorHandler(this RouteCollection routes)
    {
      routes.MapHttpHandler<TokensMonitorHttpHandler>("_tokens/monitor/{*routeQuery}");
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string url) where T : IHttpHandler, new()
    {
      routes.MapHttpHandler<T>(null, url, null, null);
    }

    private static void MapHttpHandler<T>(this RouteCollection routes, string name, string url, object defaults, object constraints) where T : IHttpHandler, new()
    {
      var route = new Route(url, new TokensMonitorRouteHandler<T>());
      route.Defaults = new RouteValueDictionary(defaults);
      route.Constraints = new RouteValueDictionary(constraints);
      routes.Add(name, route);
    }
  }
}
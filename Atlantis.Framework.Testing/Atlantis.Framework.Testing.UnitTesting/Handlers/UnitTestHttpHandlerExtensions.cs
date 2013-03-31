using Atlantis.Framework.Testing.UnitTesting.BaseClasses;
using System.Web;
using System.Web.Routing;

namespace Atlantis.Framework.Testing.UnitTesting.Handlers
{
  public static class UnitTestHttpHandlerExtensions
  {
    public static void MapUnitTestsHandler(this RouteCollection routes)
    {
      routes.MapHttpHandler<UnitTestHandler>("_unittests/{*routeQuery}");
    }

    private static void MapHttpHandler<THandler>(this RouteCollection routes, string url) where THandler : IHttpHandler, new()
    {
      routes.MapHttpHandler<THandler>(null, url, null, null);
    }

    private static void MapHttpHandler<THandler>(this RouteCollection routes, string name, string url, object defaults, object constraints) where THandler : IHttpHandler, new()
    {
      var route = new Route(url, new UnitTestHttpHandlerRouteHandler<THandler>());
      route.Defaults = new RouteValueDictionary(defaults);
      route.Constraints = new RouteValueDictionary(constraints);
      routes.Add(name, route);
    }
  }
}
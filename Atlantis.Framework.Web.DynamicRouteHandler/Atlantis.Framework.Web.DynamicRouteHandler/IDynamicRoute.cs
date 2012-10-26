using System.Web.Routing;

namespace Atlantis.Framework.Web.DynamicRouteHandler
{
  public interface IDynamicRoute
  {
    string RoutePath { get; }

    void RegisterRoute(RouteCollection routeCollection);
  }
}

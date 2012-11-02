using System.Collections.Generic;
using System.Web.Routing;

namespace Atlantis.Framework.Web.DynamicRouteHandler
{
  public interface IDynamicRoute
  {
    DynamicRoutePath RoutePath { get; }

    IEnumerable<DynamicRoutePath> AdditionalRoutePaths { get; }

    void RegisterRoute(RouteCollection routeCollection);
  }
}

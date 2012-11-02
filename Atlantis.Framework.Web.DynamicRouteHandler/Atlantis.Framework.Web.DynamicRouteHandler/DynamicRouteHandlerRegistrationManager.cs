using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Routing;

namespace Atlantis.Framework.Web.DynamicRouteHandler
{
  public static class DynamicRouteHandlerRegistrationManager
  {
    private static readonly HashSet<string> _registeredRouteHashSet = new HashSet<string>();

    private static void RegisterRoute(RouteCollection routeCollection, IDynamicRoute dynamicRoute)
    {
      string routePathFormatted = dynamicRoute.RoutePath.ToLowerInvariant().Trim('/');

      if (!_registeredRouteHashSet.Contains(routePathFormatted))
      {
        dynamicRoute.RegisterRoute(routeCollection);
        _registeredRouteHashSet.Add(routePathFormatted);
      }
      else
      {
        throw new Exception(string.Format("Attempted to register a duplicate route: \"{0}\"", routePathFormatted));
      }
    }

    public static void AutoRegisterRouteHandlers(RouteCollection routeCollection)
    {
      AutoRegisterRouteHandlers(routeCollection, null);
    }

    public static void AutoRegisterRouteHandlers(RouteCollection routeCollection, IEnumerable<Assembly> additionalAssemblies)
    {
      AutoRegisterRouteHandlers(routeCollection, null, additionalAssemblies);
    }

    public static void AutoRegisterRouteHandlers(RouteCollection routeCollection, IEnumerable<string> assemblySearchPatterns, IEnumerable<Assembly> additionalAssemblies)
    {
      using (DynamicRouteHandlerLoader loader = new DynamicRouteHandlerLoader(assemblySearchPatterns, additionalAssemblies))
      {
        foreach (var dynamicRouteHandler in loader.DynamicRouteHandlersFound)
        {
          RegisterRouteHandlers(routeCollection, dynamicRouteHandler.Value);
        }
      }
    }

    public static void RegisterRouteHandlers(RouteCollection routeCollection, params IDynamicRoute[] dynamicRouteHandlers)
    {
      foreach(DynamicRouteHandlerBase dynamicRouteHandler in dynamicRouteHandlers)
      {
        RegisterRoute(routeCollection, dynamicRouteHandler);
      }
    }
  }
}

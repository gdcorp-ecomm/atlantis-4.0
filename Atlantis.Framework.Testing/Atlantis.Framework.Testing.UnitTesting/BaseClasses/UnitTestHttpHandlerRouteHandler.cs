using System;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Atlantis.Framework.Testing.UnitTesting.BaseClasses
{
  public class UnitTestHttpHandlerRouteHandler<T> : IRouteHandler where T : IHttpHandler, new()
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new T();
    }
  }

  public class UnitTestHttpHandlerRouteHandler : IRouteHandler
  {
    private String _VirtualPath = null;

    public UnitTestHttpHandlerRouteHandler(String virtualPath)
    {
      _VirtualPath = virtualPath;
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      IHttpHandler httpHandler = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(_VirtualPath, typeof(IHttpHandler));
      return httpHandler;
    }
  }
}
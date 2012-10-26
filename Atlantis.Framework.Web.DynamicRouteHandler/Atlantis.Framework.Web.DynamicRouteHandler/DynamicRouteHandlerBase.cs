using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;

namespace Atlantis.Framework.Web.DynamicRouteHandler
{
  [InheritedExport(typeof(IDynamicRoute))]
  public abstract class DynamicRouteHandlerBase : IRouteHandler, IDynamicRoute, IHttpHandler, IRequiresSessionState
  {
    private static readonly object _lockSync = new object();
    private static volatile IHttpHandler _httpHandlerInstance;

    protected HttpRequestMethodType RequestMethodType { get; private set; }

    protected abstract HttpRequestMethodType AllowedRequestMethodTypes { get; }

    public virtual bool IsReusable
    {
      get { return false; }
    }

    public abstract string RoutePath { get; }

    protected void EndRequest(int statusCode)
    {
      EndRequest(statusCode, string.Empty);
    }

    protected void EndRequest(int statusCode, string body)
    {
      HttpContext.Current.Response.StatusCode = 403;
      HttpContext.Current.Response.End();
    }

    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      IHttpHandler httpHandler;
      if(IsReusable)
      {
        if(_httpHandlerInstance == null)
        {
          lock(_lockSync)
          {
            if(_httpHandlerInstance == null)
            {
              _httpHandlerInstance = (IHttpHandler)Activator.CreateInstance(GetType());
            }
          }
        }

        httpHandler = _httpHandlerInstance;
      }
      else
      {
        httpHandler = (IHttpHandler)Activator.CreateInstance(GetType());
      }

      return httpHandler;
    }

    protected abstract void HandleRequest();

    public void ProcessRequest(HttpContext httpContext)
    {
      RequestMethodType = HttpRequestMethodType.Unknown;

      switch (httpContext.Request.HttpMethod.ToUpperInvariant())
      {
        case "GET":
          RequestMethodType = HttpRequestMethodType.Get;
          break;
        case "POST":
          RequestMethodType = HttpRequestMethodType.Post;
          break;
        case "PUT":
          RequestMethodType = HttpRequestMethodType.Put;
          break;
        case "DELETE":
          RequestMethodType = HttpRequestMethodType.Delete;
          break;
      }

      if ((AllowedRequestMethodTypes & RequestMethodType) == RequestMethodType)
      {
        HandleRequest();
      }
      else
      {
        EndRequest(403, string.Empty);
      }
    }

    public void RegisterRoute(RouteCollection routeCollection)
    {
      string routePathFormatted = RoutePath.ToLowerInvariant().Trim('/');
      routeCollection.Add(new Route(routePathFormatted, this));
    }
  }
}

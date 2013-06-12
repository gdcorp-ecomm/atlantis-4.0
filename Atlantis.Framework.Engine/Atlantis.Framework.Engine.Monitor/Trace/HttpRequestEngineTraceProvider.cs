using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Engine.Monitor.Trace
{
  public class HttpRequestEngineTraceProvider : ProviderBase
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Lazy<List<ICompletedRequest>> _completedRequests;

    private readonly Lazy<bool> _engineTraceOn;
    private const string _TRACEPARAMETER = "atlantisenginetrace";

    public HttpRequestEngineTraceProvider(IProviderContainer container) 
      : base(container)
    {
      _completedRequests = new Lazy<List<ICompletedRequest>>(() => {return new List<ICompletedRequest>(10);});

      _siteContext = new Lazy<ISiteContext>(Container.Resolve<ISiteContext>);
      _engineTraceOn = new Lazy<bool>(() => { return GetEngineTraceOn(); });
    }

    private bool GetEngineTraceOn()
    {
      bool result = false;
      if ((HttpContext.Current != null) && (_siteContext.Value.IsRequestInternal))
      {
        string queryenginetrace = HttpContext.Current.Request.QueryString[_TRACEPARAMETER];
        if (queryenginetrace != null)
        {
          result = queryenginetrace == "1";
          WriteCookie(result);
        }
        else
        {
          HttpCookie cookieTrace = HttpContext.Current.Request.Cookies[_TRACEPARAMETER];
          if (cookieTrace != null)
          {
            result = cookieTrace.Value == "1";
          }
        }
      }

      return result;
    }

    private void WriteCookie(bool result)
    {
      string newCookieValue = result ? "1" : "0";

      HttpCookie existingCookie = HttpContext.Current.Request.Cookies[_TRACEPARAMETER];
      if ((existingCookie == null) || (existingCookie.Value != newCookieValue))
      {
        HttpContext.Current.Response.Cookies.Set(new HttpCookie(_TRACEPARAMETER, newCookieValue));
      }
    }

    public IEnumerable<ICompletedRequest> CompletedEngineRequests
    {
      get
      {
        return _completedRequests.Value;
      }
    }

    internal void LogCompletedRequest(ICompletedRequest completedRequest)
    {
      if (_engineTraceOn.Value)
      {
        _completedRequests.Value.Add(completedRequest);
      }
    }
  }
}

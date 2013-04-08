using System.Globalization;
using Atlantis.Framework.Interface;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Engine.Monitor.Trace
{
  public class EngineTraceProvider : ProviderBase
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Dictionary<Guid, Dictionary<string, string>> _requestTrace;
    private readonly string _engineTraceQueryStringValue;
    private const string COOKIE_NAME = "AtlantisFramework";
    private const string ENGINETRACE_KEY = "enginetrace";

    public EngineTraceProvider(IProviderContainer container) 
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(Container.Resolve<ISiteContext>);

      _requestTrace = new Dictionary<Guid, Dictionary<string, string>>();
      _engineTraceQueryStringValue = HttpContext.Current.Request.QueryString[ENGINETRACE_KEY] ?? string.Empty;

      EngineTraceTypes result;
      if (!string.IsNullOrEmpty(_engineTraceQueryStringValue) && Enum.TryParse(_engineTraceQueryStringValue, out result))
      {
        SetCookieValueForEngineTrace(_engineTraceQueryStringValue);
      }
      else
      {
        _engineTraceQueryStringValue = string.Empty;
      }
    }

    #region "enginetrace" cookie related
    private bool AtlantisFrameworkCookieExists
    {
      get
      {
        HttpCookie engineTraceCookie = HttpContext.Current.Request.Cookies[COOKIE_NAME];
        return engineTraceCookie != null;
      }
    }

    private HttpCookie AtlantisFrameworkCookie
    {
      get
      {
        return HttpContext.Current.Request.Cookies[COOKIE_NAME];
      }
    }

    private void SetCookieValueForEngineTrace(string value)
    {
      if (!AtlantisFrameworkCookieExists)
      {
        HttpCookie afCookie = _siteContext.Value.NewCrossDomainMemCookie(COOKIE_NAME);
        afCookie.Values[ENGINETRACE_KEY] = value;
        HttpContext.Current.Response.Cookies.Add(afCookie);
      }
      else
      {
        HttpContext.Current.Response.Cookies[COOKIE_NAME][ENGINETRACE_KEY] = value;
      }
    }

    private string GetCookieValueForEngineTrace
    {
      get
      {
        var engineTrace = "0";
        if (AtlantisFrameworkCookieExists)
        {
          engineTrace = AtlantisFrameworkCookie.Values[ENGINETRACE_KEY] ?? "0";
        }
        return engineTrace;
      }
    }

    private EngineTraceTypes EngineTraceType
    {
      get
      {
        EngineTraceTypes traceType;

        var checkTraceType = !string.IsNullOrEmpty(_engineTraceQueryStringValue)
                  ? _engineTraceQueryStringValue
                  : GetCookieValueForEngineTrace;

        switch (checkTraceType)
        {
          case "0":
            traceType = EngineTraceTypes.None;
            break;
          case "1":
            traceType = EngineTraceTypes.Summary;
            break;
          case "2":
            traceType = EngineTraceTypes.Details;
            break;
          default:
            traceType = EngineTraceTypes.None;
            break;
        }
        return traceType;
      }
    }
    #endregion

    internal void Engine_OnProcessRequestStart(RequestData requestData, int requestType, Guid requestId)
    {
      if (HttpContext.Current != null)
      {
        ProcessRequestStart(requestData, requestType, requestId);
      }
    }

    internal void Engine_OnProcessRequestComplete(RequestData requestData, int requestType, Guid requestId, IResponseData oResponse)
    {
      if (HttpContext.Current != null)
      {
        ProcessRequestEnd(requestData, requestType, requestId, oResponse);
      }
    }

    private void ProcessRequestStart(RequestData requestData, int requestType, Guid requestId)
    {
      if (_siteContext.Value.IsRequestInternal && EngineTraceType != EngineTraceTypes.None)
      {
        Dictionary<string, string> request;
        if (!_requestTrace.TryGetValue(requestId, out request))
        {
          request = new Dictionary<string, string>();
        }

        request["requestClass"] = requestData.GetType().ToString();
        request["requestType"] = requestType.ToString(CultureInfo.InvariantCulture);
        request["start"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        if (EngineTraceType == EngineTraceTypes.Details)
        {
          request["requestDetails"] = requestData.ToXML();
        }

        _requestTrace[requestId] = request;
      }
    }

    private void ProcessRequestEnd(RequestData requestData, int requestType, Guid requestId, IResponseData responseData)
    {
      if (_siteContext.Value.IsRequestInternal && EngineTraceType != EngineTraceTypes.None)
      {
        Dictionary<string, string> request;
        if (_requestTrace.TryGetValue(requestId, out request))
        {
          if (EngineTraceType == EngineTraceTypes.Details)
          {
            request["responseDetails"] = responseData.ToXML();
          }
          request["end"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);

          _requestTrace[requestId] = request;
        }
      }
    }

    private List<EngineRequestStats> _engineCallStats;
    public IEnumerable<EngineRequestStats> EngineTraceStats
    {
      get
      {
        _engineCallStats = new List<EngineRequestStats>();

        if (_requestTrace != null)
        {
          foreach (var info in _requestTrace)
          {
            var stats = new EngineRequestStats();
            stats.RequestId = info.Key;

            foreach (var requestInfo in info.Value)
            {
              switch (requestInfo.Key.ToLowerInvariant())
              {
                case "requestclass":
                  stats.RequestClassName = requestInfo.Value;
                  break;
                case "requesttype":
                  stats.RequestType = requestInfo.Value;

                  ConfigElement configElement;
                  if (Engine.TryGetConfigElement(Convert.ToInt32(requestInfo.Value), out configElement))
                  {
                    try
                    {
                      var wsConfigElement = configElement as WsConfigElement;
                      if (wsConfigElement != null)
                      {
                        stats.WebServiceUrl = wsConfigElement.WSURL;
                      }
                    }
                    catch
                    {
                    }
                  }
                  break;
                case "requestdetails":
                  stats.RequestDetails = requestInfo.Value;
                  break;
                case "responsedetails":
                  stats.ResponseDetails = requestInfo.Value;
                  break;
                case "start":
                  stats.StartTime= Convert.ToDateTime(requestInfo.Value);
                  break;
                case "end":
                  stats.EndTime= Convert.ToDateTime(requestInfo.Value);
                  break;
              }
            }
            _engineCallStats.Add(stats);
          }
        }
        return _engineCallStats;
      }
    }
  }
}

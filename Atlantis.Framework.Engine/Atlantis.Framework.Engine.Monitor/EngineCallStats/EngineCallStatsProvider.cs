using System.Globalization;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System;
using System.Collections.Generic;
using System.Web;

namespace Atlantis.Framework.Engine.Monitor.EngineCallStats
{
  public class EngineCallStatsProvider : ProviderBase
  {
    private readonly Lazy<ISiteContext> _siteContext;
    private readonly Dictionary<Guid, Dictionary<string, string>> _requestTrace;

    public EngineCallStatsProvider(IProviderContainer container) 
      : base(container)
    {
      _siteContext = new Lazy<ISiteContext>(HttpProviderContainer.Instance.Resolve<ISiteContext>);

      var currentRequestItems = HttpContext.Current.Items["EngineCallStatsTraceInfo"] ??
                                new Dictionary<Guid, Dictionary<string, string>>();
      _requestTrace = currentRequestItems as Dictionary<Guid, Dictionary<string, string>>;
    }

    public void ProcessRequestStart(RequestData requestData, int requestType, Guid requestId)
    {
      if (_siteContext.Value.IsRequestInternal)
      {
        Dictionary<string, string> request;
        if (!_requestTrace.TryGetValue(requestId, out request))
        {
          request = new Dictionary<string, string>();
        }

        request["requestClass"] = requestData.GetType().ToString();
        request["requestType"] = requestType.ToString(CultureInfo.InvariantCulture);
        request["start"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        request["requestDetails"] = requestData.ToXML();

        _requestTrace[requestId] = request;
      }
    }

    public void ProcessRequestEnd(RequestData requestData, int requestType, Guid requestId, IResponseData responseData)
    {
      if (_siteContext.Value.IsRequestInternal)
      {
        Dictionary<string, string> request;
        if (_requestTrace.TryGetValue(requestId, out request))
        {
          request["responseDetails"] = responseData.ToXML();
          request["end"] = DateTime.Now.ToString(CultureInfo.InvariantCulture);

          _requestTrace[requestId] = request;
        }
      }
    }

    private List<EngineCallRequestStats> _engineCallStats;
    public IEnumerable<EngineCallRequestStats> EngineCallStats
    {
      get
      {
        _engineCallStats = new List<EngineCallRequestStats>();

        if (_requestTrace != null)
        {
          foreach (var info in _requestTrace)
          {
            var stats = new EngineCallRequestStats();
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

    public void LogDebugData(IDebugContext debug)
    {
      if (_siteContext.Value.IsRequestInternal)
      {
        foreach (var stat in EngineCallStats)
        {
          var data = "<p>" + "Request class = " + stat.RequestClassName + "</p>";
          data += "<p>" + "Start time = " + stat.StartTime.ToString(CultureInfo.InvariantCulture) + "</p>";
          data += "<p>" + "End time = " + stat.EndTime.ToString(CultureInfo.InvariantCulture) + "</p>";

          if (!string.IsNullOrEmpty(stat.WebServiceUrl))
          {
            data += "<p>" + "Web Service Url = " + HttpContext.Current.Server.HtmlEncode(stat.WebServiceUrl);
          }

          data += "<p>" + "Request details = " + HttpContext.Current.Server.HtmlEncode(stat.RequestDetails) + "</p>";
          data += "<p>" + "Response details = " + HttpContext.Current.Server.HtmlEncode(stat.ResponseDetails);

          debug.LogDebugTrackingData("Request Type = " + stat.RequestType, data);
        }
      }
    }

    public static void Initialize()
    {
      HttpProviderContainer.Instance.RegisterProvider<EngineCallStatsProvider, EngineCallStatsProvider>();
      Engine.OnProcessRequestStart += Engine_OnProcessRequestStart;
      Engine.OnProcessRequestComplete += Engine_OnProcessRequestComplete;
    }

    private static EngineCallStatsProvider PageStats
    {
      get { return HttpProviderContainer.Instance.Resolve<EngineCallStatsProvider>(); }
    }

    private static void Engine_OnProcessRequestStart(RequestData requestData, int requestType, Guid requestId)
    {
      if (HttpContext.Current != null)
      {
        PageStats.ProcessRequestStart(requestData, requestType, requestId);
      }
    }

    private static void Engine_OnProcessRequestComplete(RequestData requestData, int requestType, Guid requestId, IResponseData oResponse)
    {
      if (HttpContext.Current != null)
      {
        PageStats.ProcessRequestEnd(requestData, requestType, requestId, oResponse);
      }
    }
  }
}

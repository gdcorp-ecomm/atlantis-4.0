using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web;
using System.Xml.Linq;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class Help : IMonitor
  {
    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();
      
      var root = new XElement("MonitorUrls");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      try
      {
        if (HttpContext.Current.Request.ApplicationPath != null)
        {
          string baseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";

          var el = new XElement("MonitorUrl");
          el.Add(new XAttribute("type", "dottypecache"));
          el.Add(new XAttribute("sampleurl", baseUrl + "_dottypecache/monitor/data?tld=com"));
          root.Add(el);

          el = new XElement("MonitorUrl");
          el.Add(new XAttribute("type", "activetlds"));
          el.Add(new XAttribute("sampleurl", baseUrl + "_dottypecache/monitor/activetlds"));
          root.Add(el);

          el = new XElement("MonitorUrl");
          el.Add(new XAttribute("type", "offeredtlds"));
          el.Add(new XAttribute("sampleurl", baseUrl + "_dottypecache/monitor/offeredtlds?type=registration&tld=org|com"));
          root.Add(el);

          el = new XElement("MonitorUrl");
          el.Add(new XAttribute("type", "productids"));
          el.Add(new XAttribute("sampleurl", baseUrl + "_dottypecache/monitor/productids?tld=o2.borg&phase=regreg&plresellertypeid=1&prodtypeid=2"));
          root.Add(el);

          el = new XElement("MonitorUrl");
          el.Add(new XAttribute("type", "tldmlready"));
          el.Add(new XAttribute("sampleurl", baseUrl + "_dottypecache/monitor/tldmlready?tld=borg|a.borg"));
          root.Add(el);
        }
      }
      catch (Exception ex)
      {
        root.Add(new XElement("error", ex.Message));
      }

      return result;
    }

    private XAttribute GetProcessId()
    {
      return new XAttribute("ProcessId", Process.GetCurrentProcess().Id);
    }

    private XAttribute GetMachineName()
    {
      return new XAttribute("MachineName", Environment.MachineName);
    }

    private XAttribute GetFileVersion()
    {
      return new XAttribute("DotTypeCacheVersion", DotTypeCache.FileVersion);
    }

    private XAttribute GetInterfaceVersion()
    {
      return new XAttribute("DotTypeCacheInterfaceVersion", DotTypeCache.InterfaceVersion);
    }
  }
}

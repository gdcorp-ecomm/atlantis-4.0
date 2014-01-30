using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml.Linq;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.Providers.TLDDataCache;
using Atlantis.Framework.Providers.TLDDataCache.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class ActiveTLDs : IMonitor
  {
    const int ActiveTldRequest = 635;

    private readonly ITLDDataCacheProvider _tldDataCacheProvider;
    public ActiveTLDs()
    {
      HttpProviderContainer.Instance.RegisterProvider<ITLDDataCacheProvider, TLDDataCacheProvider>();
      _tldDataCacheProvider = HttpProviderContainer.Instance.Resolve<ITLDDataCacheProvider>();
    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();
      
      var root = new XElement("ActiveTLDs");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      try
      {
        var activeTlds = _tldDataCacheProvider.GetActiveTlds();

        var tldInfo = new XElement("TLDInfo");

        if (activeTlds != null)
        {
          foreach (var flag in activeTlds.AllFlagNames)
          {
            var tlds = activeTlds.GetActiveTLDUnion(flag);

            tldInfo.Add(GetTldElement(flag, tlds));
          }
        }
        root.Add(tldInfo);
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

    private XElement GetTldElement(string flag, HashSet<string> tlds)
    {
      var result = new XElement("Flag");
      result.Add(new XAttribute("name", flag));
      
      var tldsElement = new XElement("TLDs");
      tldsElement.Add(new XAttribute("count", tlds.Count));

      foreach (var tld in tlds)
      {
        var tldElement = new XElement("TLD");
        tldElement.Add(new XAttribute("name", tld));
        tldsElement.Add(tldElement);
      }

      result.Add(tldsElement);
      return result;
    }
  }
}

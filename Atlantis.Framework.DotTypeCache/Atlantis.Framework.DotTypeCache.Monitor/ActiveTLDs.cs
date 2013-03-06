using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class ActiveTLDs : IMonitor
  {
    const int _ACTIVETLDREQUEST = 635;

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      XDocument result = new XDocument();
      
      XElement root = new XElement("ActiveTLDs");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      try
      {
        var request = new ActiveTLDsRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
        var response = (ActiveTLDsResponseData)DataCache.DataCache.GetProcessRequest(request, _ACTIVETLDREQUEST);

        XElement tldInfo = new XElement("TLDInfo");

        foreach (var flag in response.AllFlagNames)
        {
          var tlds = response.GetActiveTLDUnion(flag);

          tldInfo.Add(GetTldElement(flag, tlds));
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
      XElement result = new XElement("Flag");
      result.Add(new XAttribute("name", flag));
      
      XElement tldsElement = new XElement("TLDs");
      tldsElement.Add(new XAttribute("count", tlds.Count));

      foreach (var tld in tlds)
      {
        XElement tldElement = new XElement("TLD");
        tldElement.Add(new XAttribute("name", tld));
        tldsElement.Add(tldElement);
      }

      result.Add(tldsElement);
      return result;
    }
  }
}

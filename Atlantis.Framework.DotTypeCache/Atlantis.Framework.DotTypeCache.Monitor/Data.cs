using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using System.Xml;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class Data : IMonitor
  {
    public Data()
    {
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      XDocument result = new XDocument();

      XElement root = new XElement("Data");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      var items = qsc.AllKeys.SelectMany(qsc.GetValues, (k, v) => new { key = k, value = v });
      var tldValue = string.Empty;
      foreach (var item in items)
      {
        if (item.key.ToLowerInvariant() == "tld")
        {
          tldValue = item.value;
          break;
        }
      }

      if (!string.IsNullOrEmpty(tldValue))
      {
        IDotTypeInfo dotType = DotTypeCache.GetDotTypeInfo(tldValue);
        var className = dotType.GetType().FullName;

        XElement tldInfo = new XElement("TLDInfo");
        tldInfo.Add(DotTypeName(tldValue), DotTypeSource(className));

        root.Add(tldInfo);
      }

      return result;
    }

    private XAttribute DotTypeName(string name)
    {
      return new XAttribute("Name", name.ToUpperInvariant());
    }

    private XAttribute DotTypeSource(string className)
    {
      return new XAttribute("DotTypeSource", className);
    }

    private XAttribute GetFileVersion()
    {
      return new XAttribute("DotTypeCacheVersion", DotTypeCache.FileVersion);
    }

    private XAttribute GetInterfaceVersion()
    {
      return new XAttribute("DotTypeCacheInterfaceVersion", DotTypeCache.InterfaceVersion);
    }

    private XAttribute GetProcessId()
    {
      return new XAttribute("ProcessId", Process.GetCurrentProcess().Id);
    }

    private XAttribute GetMachineName()
    {
      return new XAttribute("MachineName", Environment.MachineName);
    }
  }
}

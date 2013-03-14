using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class OfferedTLDs : IMonitor
  {
    private readonly IDotTypeProvider _dotTypeProvider;
    public OfferedTLDs()
    {
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
      _dotTypeProvider = HttpProviderContainer.Instance.Resolve<IDotTypeProvider>();
    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();
      
      var root = new XElement("OfferedTLDs");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      try
      {
        var items = qsc.AllKeys.SelectMany(qsc.GetValues, (k, v) => new { key = k, value = v });
        string tldProductType = string.Empty;
        var tldNames = new string[] {};
        foreach (var item in items)
        {
          if (!string.IsNullOrEmpty(item.key) && !string.IsNullOrEmpty(item.value))
          {
            switch (item.key.ToLowerInvariant())
            {
              case "type":
                tldProductType = item.value;
                break;
              case "tld":
                var delimiters = new[] { '|', ',' };
                tldNames = item.value.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                break;
            }
          }
        }

        if (!string.IsNullOrEmpty(tldProductType))
        {
          root.Add(ProductType(tldProductType));

          OfferedTLDProductTypes type;
          if (tldProductType.ToLowerInvariant() == "bulktransfer")
          {
            type = OfferedTLDProductTypes.BulkTransfer;
          }
          else
          {
            type = (OfferedTLDProductTypes)Enum.Parse(typeof(OfferedTLDProductTypes), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tldProductType.ToLower()));
          }

          ITLDDataImpl tldData;
          switch (type)
          {
              case OfferedTLDProductTypes.Registration:
                tldData = _dotTypeProvider.GetTLDDataForRegistration;
                break;
              case OfferedTLDProductTypes.Transfer:
                tldData = _dotTypeProvider.GetTLDDataForTransfer;
                break;
              case OfferedTLDProductTypes.Bulk:
                tldData = _dotTypeProvider.GetTLDDataForBulk;
                break;
              case OfferedTLDProductTypes.BulkTransfer:
                tldData = _dotTypeProvider.GetTLDDataForBulkTransfer;
                break;
              default:
                tldData = _dotTypeProvider.GetTLDDataForRegistration;
                break;
          }

          var tlds = tldData.GetDiagnosticsOfferedTLDFlags(tldNames);
          var tldInfo = new XElement("TLDInfo");
          tldInfo.Add(new XAttribute("count", tlds.Count));
          foreach (var tld in tlds)
          {
            tldInfo.Add(GetTldElement(tld.Key, tld.Value));
          }
          root.Add(tldInfo);
        }
      }
      catch (Exception ex)
      {
        root.Add(new XElement("error", ex.Message));
      }

      return result;
    }

    private XAttribute ProductType(string type)
    {
      return new XAttribute("ProductType", type);
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

    private XElement GetTldElement(string tldName, Dictionary<string, bool> flagSets)
    {
      var result = new XElement("TLD");
      result.Add(new XAttribute("name", tldName.ToUpperInvariant()));

      var resultFlag = new XElement("Flags");
      foreach (var flagSet in flagSets)
      {
        resultFlag.Add(new XAttribute(flagSet.Key, flagSet.Value));
      }

      result.Add(resultFlag);
      return result;
    }
  }
}

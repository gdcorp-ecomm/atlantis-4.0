using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
using Atlantis.Framework.DotTypeCache.StaticTypes;
using Atlantis.Framework.Providers.Interface.ProviderContainer;
using Atlantis.Framework.RegDotTypeProductIds.Interface;
using Atlantis.Framework.TLDDataCache.Interface;

namespace Atlantis.Framework.DotTypeCache.Monitor
{
  internal class ProductIDs : IMonitor
  {
    public ProductIDs()
    {
      HttpProviderContainer.Instance.RegisterProvider<IDotTypeProvider, DotTypeProvider>();
    }

    public XDocument GetMonitorData(NameValueCollection qsc)
    {
      var result = new XDocument();

      var root = new XElement("Monitor");
      root.Add(GetProcessId(), GetMachineName(), GetFileVersion(), GetInterfaceVersion());
      result.Add(root);

      try
      {
        var items = qsc.AllKeys.SelectMany(qsc.GetValues, (k, v) => new { key = k, value = v });
        string tldName = string.Empty;
        foreach (var item in items)
        {
          if (!string.IsNullOrEmpty(item.key) && !string.IsNullOrEmpty(item.value))
          {
            switch (item.key.ToLowerInvariant())
            {
              case "tld":
                tldName = item.value;
                break;
            }
          }
        }

        if (!string.IsNullOrEmpty(tldName))
        {
          IDotTypeInfo dotTypeInfo = DotTypeCache.GetDotTypeInfo(tldName);
          if (dotTypeInfo != null)
          {
            var data = new XElement("Data");
            data.Add(new XAttribute("tld", tldName));
            data.Add(new XAttribute("dottypesource", dotTypeInfo.GetType().FullName));

            if (dotTypeInfo.GetType().Name != "InvalidDotType")
            {
              if (dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo" || dotTypeInfo.GetType().Name == "MultiRegDotTypeInfo")
              {
                var request = new ProductIdListRequestData(string.Empty, string.Empty, string.Empty,
                                                            string.Empty, 0, tldName);
                var response =
                  (ProductIdListResponseData)
                  DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductIdList);

                data.Add(XElement.Parse(response.ToXML()));
              }
              else
              {
                var methods = new[]
                                     {
                                       "InitializeRegistrationProductIds", "InitializeTransferProductIds",
                                       "InitializeRenewalProductIds", "InitializePreRegistrationProductIds", "InitializeExpiredAuctionRegProductIds"
                                     };

                foreach (var meth in methods)
                {
                  Type classType = dotTypeInfo.GetType();
                  object objInstance = Activator.CreateInstance(classType);

                  StaticDotTypeTiers tiers = null;
                  try
                  {
                    tiers = (StaticDotTypeTiers)classType.InvokeMember(meth,
                                                                      BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
                                                                      BindingFlags.Instance | BindingFlags.InvokeMethod,
                                                                      null, objInstance, null);
                  }
                  catch (Exception)
                  {
                    continue;
                  }

                  var productType = new XElement("ProductType");
                  productType.Add(new XAttribute("value", tiers.ProductIdType));

                  classType = tiers.GetType();

                  var tier = (List<StaticDotTypeTier>)classType.InvokeMember("_tierGroups",
                                                                            BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
                                                                            BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField,
                                                                            null, tiers, null);

                  var productids = new XElement("ProductIDs");

                  var pidList = new List<int>();
                  foreach (var s in tier)
                  {
                    classType = s.GetType();
                    var tierProducts = (int[])classType.InvokeMember("_productIdsByYearsMinusOne",
                                                                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
                                                                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField,
                                                                    null, s, null);
                    pidList.AddRange(tierProducts);
                  }
                  productids.Add(new XAttribute("value", string.Join(",", pidList)));
                  productType.Add(productids);
                  data.Add(productType);
                }
              }
            }
            root.Add(data);
          }
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

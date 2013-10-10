using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Atlantis.Framework.DotTypeCache.Interface;
using Atlantis.Framework.DotTypeCache.Static;
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

    private ValidDotTypesResponseData LoadValidDotTypes()
    {
      var request = new ValidDotTypesRequestData(string.Empty, string.Empty, string.Empty, string.Empty, 0);
      return (ValidDotTypesResponseData)DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ValidDotTypes);
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
        int tldId = 0;
        string tldName = string.Empty;
        string tldPhase = string.Empty;
        int plResellerTypeId = 0;
        int productTypeId = 0;

        foreach (var item in items)
        {
          if (!string.IsNullOrEmpty(item.key) && !string.IsNullOrEmpty(item.value))
          {
            switch (item.key.ToLowerInvariant())
            {
              case "tld":
                tldName = item.value;
                break;
              case "phase":
                tldPhase = item.value;
                break;
              case "plresellertypeid":
                Int32.TryParse(item.value, out plResellerTypeId);
                break;
              case "prodtypeid":
                Int32.TryParse(item.value, out productTypeId);
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

            var validDotTypesResponse = LoadValidDotTypes();
            validDotTypesResponse.TryGetTldId(tldName, out tldId);
            data.Add(new XAttribute("tldId", tldId));
            data.Add(new XAttribute("dottypesource", dotTypeInfo.GetType().FullName));

            if (dotTypeInfo.GetType().Name != "InvalidDotType")
            {
              if (dotTypeInfo.GetType().Name == "TLDMLDotTypeInfo")
              {

                if (tldId <= 0 || plResellerTypeId <= 0 && productTypeId <= 0)
                {
                  throw new ArgumentException("TldId, PrivateLabelResellerTypeId and productTypeId must be greater than zero.");
                }

                if (string.IsNullOrEmpty(tldPhase))
                {
                  throw new ArgumentException("TLDPhase cannot be empty.");
                }

                var request = new TLDProductDomainAttributesRequestData(tldId, tldPhase, plResellerTypeId, productTypeId);
                var response = (TLDProductDomainAttributesResponseData)
                  DataCache.DataCache.GetProcessRequest(request, DotTypeEngineRequests.ProductDomainAttributes);

                data.Add(XElement.Parse(response.ToXML()));
              }
              else if (dotTypeInfo.GetType().Name == "MultiRegDotTypeInfo")
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

                  StaticDotTypeTiers tiers;
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

                  foreach (var s in tier)
                  {
                    classType = s.GetType();
                    var tierProducts = (int[])classType.InvokeMember("_productIdsByYearsMinusOne",
                                                                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | 
                                                                    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.GetField,
                                                                    null, s, null);

                    var productids = new XElement("ProductIDs");
                    productids.Add(new XAttribute("mindomains", s.MinDomains));
                    productids.Add(new XAttribute("value", string.Join(",", tierProducts)));

                    productType.Add(productids);
                  }
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

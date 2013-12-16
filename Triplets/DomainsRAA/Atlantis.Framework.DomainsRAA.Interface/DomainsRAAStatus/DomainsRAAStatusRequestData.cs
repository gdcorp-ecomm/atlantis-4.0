using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Interface.DomainsRAAStatus
{
  public class DomainsRAAStatusRequestData : RequestData
  {
    private readonly VerifyRequestItems _verificationItems;

    public DomainsRAAStatusRequestData(VerifyRequestItems verificationItems)
    {
      _verificationItems = verificationItems;
      RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public string GetRequestXml(string appName)
    {
      if (string.IsNullOrWhiteSpace(appName))
      {
        throw new Exception("ConfigValue is missing AppName");
      }

      var itemTypes = _verificationItems.Items as VerifyRequestItem[] ?? _verificationItems.Items.ToArray();

      var itemElements = new List<XElement>(itemTypes.Length);

      foreach (var itemType in itemTypes)
      {
        itemElements.Add(new XElement("item",
          new XAttribute("type", itemType.ItemType),
          new XAttribute("value", itemType.ItemTypeValue)
          ));
      }

      var requestElement = new XElement("request",
        new XAttribute("requestedby", appName),
        new XAttribute("requestedip", _verificationItems.RequestedIp),
        new XElement("items", itemElements)
        );

      return requestElement.ToString(SaveOptions.None);
    }
  }
}

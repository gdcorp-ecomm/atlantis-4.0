using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Interface.DomainsRAAVerify
{
  public class DomainsRAAVerifyRequestData : RequestData
  {
    private readonly string _shopperId;
    private readonly VerifyRequestItems _verficationItems;

    public DomainsRAAVerifyRequestData(string shopperId, VerifyRequestItems verificationItems)
    {
      _shopperId = shopperId;
      _verficationItems = verificationItems;

      RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public string GetRequestXml(string appName)
    {
      if (string.IsNullOrWhiteSpace(appName))
      {
        throw new Exception("ConfigValue is missing AppName");
      }

      if (_verficationItems.ReasonCode == DomainsRAAReasonCodes.None)
      {
        throw new Exception("ReasonCode cannot be None");
      }

      var itemTypes = _verficationItems.Items as VerifyRequestItem[] ?? _verficationItems.Items.ToArray();

      var itemElements = new List<XElement>(itemTypes.Length);

      foreach (var itemType in itemTypes)
      {
        itemElements.Add(new XElement("item",
          new XAttribute("type", itemType.ItemType),
          itemType.ItemTypeValue
          ));
      }

      var requestElement = new XElement("request",
        new XElement("verification",
          new XAttribute("shopperid", _shopperId),
          new XAttribute("reasoncode", (int)_verficationItems.ReasonCode),
          new XAttribute("requestedby", appName),
          new XAttribute("requestedip", _verficationItems.RequestedIp),
          new XElement("verificationitems",
            new XAttribute("type", _verficationItems.RegistrationType),
            new XAttribute("domainid", _verficationItems.DomainId),
            itemElements)));

      return requestElement.ToString(SaveOptions.None);
    }
  }
}

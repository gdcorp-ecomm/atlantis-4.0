using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DomainsRAA.Interface.Items;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Interface.DomainsRAASetVerified
{
  public class DomainsRAASetVerifiedRequestData : RequestData
  {
    private readonly VerificationItemElement _verificationItemElement;

    public DomainsRAASetVerifiedRequestData(VerificationItemElement verificationItemElement)
    {
      _verificationItemElement = verificationItemElement;
      RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public string GetRequestXml(string appName)
    {
      if (string.IsNullOrWhiteSpace(appName))
      {
        throw new Exception("ConfigValue is missing AppName");
      }

      var itemTypes = _verificationItemElement.VerificationItems.Items as ItemElement[] ?? _verificationItemElement.VerificationItems.Items.ToArray();

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
          new XAttribute("shopperid", _verificationItemElement.ShopperId),
          new XAttribute("reasoncode", (int)_verificationItemElement.ReasonCode),
          new XAttribute("requestedby", appName),
          new XAttribute("requestedip", _verificationItemElement.RequestedIp),
          new XElement("verificationitems",
            new XAttribute("domainid", _verificationItemElement.VerificationItems.DomainId),
            new XAttribute("verifiedip", _verificationItemElement.VerificationItems.VerfiedIp),
            itemElements)));

      return requestElement.ToString(SaveOptions.None);
    }
  }
}

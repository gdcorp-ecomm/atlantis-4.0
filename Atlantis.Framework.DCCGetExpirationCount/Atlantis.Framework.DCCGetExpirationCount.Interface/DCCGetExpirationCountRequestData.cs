using System;
using System.Globalization;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetExpirationCount.Interface
{
  public class DCCGetExpirationCountRequestData : RequestData
  {
    public DCCGetExpirationCountRequestData(string shopperId, string applicationName, int daysFromExpiration)
    {
      ShopperID = shopperId;
      ApplicationName = applicationName;
      DaysFromExpiration = daysFromExpiration;
      RequestTimeout = TimeSpan.FromSeconds(4);
    }

    public int DaysFromExpiration { get; private set; }
    public string ApplicationName { get; private set; }

    public override string ToXML()
    {
      var requestElement = new XElement("getexpirationdomaincountsbyshopperid");
      requestElement.Add(new XElement("username", ApplicationName));

      var shopperElement = new XElement("shopper",
        new XAttribute("shopperid", ShopperID),
        new XAttribute("daysfromexpiration", DaysFromExpiration.ToString(CultureInfo.InvariantCulture)));
      requestElement.Add(shopperElement);

      return requestElement.ToString(SaveOptions.DisableFormatting);
    }
  }
}

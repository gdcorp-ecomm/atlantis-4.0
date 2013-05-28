using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class RemoveFromShopperWatchListRequestData : RequestData
  {
    public IList<string> GtldIds { get; set; }

    public RemoveFromShopperWatchListRequestData(string shopperId, IList<string> gTldIds)
    {
      ShopperID = shopperId;
      GtldIds = gTldIds;

      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      var gTldsElement = new XElement("gTlds");

      foreach (var gtld in GtldIds)
      {
        var gtldElement = new XElement("gTld");
        gtldElement.Add(new XAttribute("id", gtld));
        gTldsElement.Add(gtldElement);
      }

      return gTldsElement.ToString();
    }
  }
}

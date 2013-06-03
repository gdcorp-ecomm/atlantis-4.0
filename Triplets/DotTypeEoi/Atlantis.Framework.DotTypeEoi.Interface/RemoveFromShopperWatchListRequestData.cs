using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class RemoveFromShopperWatchListRequestData : RequestData
  {
    public IList<IDotTypeEoiGtld> Gtlds { get; set; }

    public RemoveFromShopperWatchListRequestData(string shopperId, IList<IDotTypeEoiGtld> gTlds)
    {
      ShopperID = shopperId;
      Gtlds = gTlds;

      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      var gTldsElement = new XElement("gTlds");

      foreach (var gTld in Gtlds)
      {
        var gtldElement = new XElement("gTld");
        gtldElement.Add(new XAttribute("id", gTld.Id));
        gTldsElement.Add(gtldElement);
      }

      return gTldsElement.ToString();
    }
  }
}

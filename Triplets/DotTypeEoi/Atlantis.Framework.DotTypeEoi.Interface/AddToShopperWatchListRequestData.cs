using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Providers.DotTypeEoi.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class AddToShopperWatchListRequestData : RequestData
  {
    public string DisplayTime { get; set; }
    public IList<IDotTypeEoiGtld> Gtlds { get; set; }

    public AddToShopperWatchListRequestData(string shopperId, string displayTime, IList<IDotTypeEoiGtld> gTlds)
    {
      ShopperID = shopperId;
      DisplayTime = displayTime;
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
        gtldElement.Add(new XAttribute("gTldSubCategoryId", gTld.GtldSubCategoryId));
        gTldsElement.Add(gtldElement);
      }

      return gTldsElement.ToString();
    }
  }
}

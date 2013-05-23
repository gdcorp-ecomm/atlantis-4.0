using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeEoi.Interface
{
  public class RemoveFromShopperWatchListRequestData : RequestData
  {
    public readonly string GTldsXml;

    public RemoveFromShopperWatchListRequestData(string shopperId, string gTldsXml)
    {
      if (string.IsNullOrEmpty(gTldsXml))
      {
        throw new ArgumentException("gTldsXml cannot be null or empty.");
      }

      ShopperID = shopperId;
      GTldsXml = gTldsXml;
      
      RequestTimeout = TimeSpan.FromSeconds(10);
    }

    public override string ToXML()
    {
      var result = new XElement("request");
      result.Add(new XAttribute("shopperId", ShopperID));

      var gTlds = new XElement("gTldsXml") { Value = GTldsXml };
      result.Add(gTlds);

      return result.ToString(SaveOptions.DisableFormatting);
    }
  }
}

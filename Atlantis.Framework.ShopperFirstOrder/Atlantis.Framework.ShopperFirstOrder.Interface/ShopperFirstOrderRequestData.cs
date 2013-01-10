using Atlantis.Framework.Interface;
using System;
using System.Xml.Linq;

namespace Atlantis.Framework.ShopperFirstOrder.Interface
{
  public class ShopperFirstOrderRequestData : RequestData
  {
    public ShopperFirstOrderRequestData(string shopperId, string sourceURL, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(2);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      XElement requestXml = new XElement("ShopperFirstOrderRequestData");
      requestXml.Add(new XAttribute("shopperId", ShopperID));
      return requestXml.ToString(SaveOptions.DisableFormatting);
    }
  }
}

using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaIsMirageCurrent.Interface
{
  public class MyaIsMirageCurrentRequestData : RequestData
  {
    public MyaIsMirageCurrentRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(3);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MyaIsMirageCurrent is not a cacheable request.");
    }

    public override string ToXML()
    {
      XDocument xmlDoc = new XDocument(new XElement("MyaIsMirageCurrentRequestData", new XElement("ShopperId", ShopperID)));
      return xmlDoc.ToString();
    }
  }
}

using System;
using System.Xml.Linq;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MyaCurrentProductGroups.Interface
{
  public class MyaCurrentProductGroupsRequestData : RequestData
  {
    public int PrivateLabelId { get; set; }

    public MyaCurrentProductGroupsRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(3);
      PrivateLabelId = privateLabelId;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MyaCurrentProductGroups is not a cacheable request.");
    }

    public override string ToXML()
    {
      XDocument xmlDoc = new XDocument(new XElement("MyaCurrentProductGroupsRequestData", new XElement("ShopperId", ShopperID), new XElement("PrivateLabelId", PrivateLabelId.ToString())));
      return xmlDoc.ToString();
    }
  }
}

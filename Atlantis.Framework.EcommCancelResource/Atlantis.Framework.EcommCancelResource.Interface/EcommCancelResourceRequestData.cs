using System;
using System.Xml.Linq;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommCancelResource.Interface
{
  public class EcommCancelResourceRequestData : RequestData
  {
    public int ResourceId { get; private set; }
    public string ResourceType { get; private set; }
    public string AutoRenewType { get; private set; }
    public string EnteredBy { get; private set; }
    public string IpAddress { get; private set; }
    public string RequestXml { get; private set; }

    public EcommCancelResourceRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  int resourceId,
                                  string resourceType,
                                  string autoRenewType,
                                  string enteredBy,
                                  string ipAddress)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResourceId = resourceId;
      ResourceType = resourceType;
      AutoRenewType = autoRenewType;
      EnteredBy = enteredBy;
      IpAddress = ipAddress;
      RequestXml = CreateWsRequestXML();
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public EcommCancelResourceRequestData(string shopperId,
                              string sourceUrl,
                              string orderId,
                              string pathway,
                              int pageCount,
                              string xml)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestXml = xml;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }


    private string CreateWsRequestXML()
    {
      var xDoc = new XDocument(
        new XElement("cancellation",
          new XAttribute("shopperid", ShopperID),
          new XAttribute("cancel_by", EnteredBy),
          new XAttribute("UserIP", IpAddress),
        new XElement("cancel",
          new XAttribute("type", AutoRenewType),
          new XAttribute("id", string.Format("{0}:{1}", ResourceType, ResourceId))
        )));

      return xDoc.ToString();

    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EcommCancelResourceRequestData");
    }
  }
}

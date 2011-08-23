using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CreateShopper.Interface
{
  public class CreateShopperRequestData : RequestData
  {
    public CreateShopperRequestData(string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId)
      : base(string.Empty, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      RequestedBy = string.Empty;
      IpAddress = GetLocalAddress();
    }

    public CreateShopperRequestData(string sourceUrl, string orderId, string pathway, int pageCount, int privateLabelId, string requestedBy, string ipAddress)
      : base(string.Empty, sourceUrl, orderId, pathway, pageCount)
    {
      PrivateLabelId = privateLabelId;
      RequestedBy = requestedBy;
      IpAddress = string.IsNullOrEmpty(ipAddress) ? GetLocalAddress() : ipAddress;
    }

    public string RequestedBy { get; set; }
    public int PrivateLabelId { get; private set; }
    public string IpAddress { get; set; }

    public override string GetCacheMD5()
    {
      throw new Exception("CreateShopper is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ShopperCreate");
      xtwRequest.WriteAttributeString("PLID", PrivateLabelId.ToString());
      xtwRequest.WriteAttributeString("IPAddress", IpAddress);
      xtwRequest.WriteAttributeString("RequestedBy", RequestedBy);
      xtwRequest.WriteEndElement(); // CreateShopper

      return sbRequest.ToString();
    }

    static string GetLocalAddress()
    {
      string sLocalAddress = "";

      IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      if (addresses.Length > 0)
        sLocalAddress = addresses[0].ToString();

      return sLocalAddress;
    }

  }
}

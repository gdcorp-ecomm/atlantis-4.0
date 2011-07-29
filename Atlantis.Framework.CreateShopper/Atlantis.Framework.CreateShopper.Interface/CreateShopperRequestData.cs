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
    string m_sRequestedBy;

    public CreateShopperRequestData(string sourceUrl, string orderID, string pathway, int pageCount, int privateLabelID)
      : base(string.Empty, sourceUrl, orderID, pathway, pageCount)
    {
      PrivateLabelId = privateLabelID;
      m_sRequestedBy = "";

    }

    public int PrivateLabelId { get; private set; }

    // **************************************************************** //

    #region RequestData Members

    // **************************************************************** //

    public override string GetCacheMD5()
    {
      throw new Exception("CreateShopper is not a cacheable request.");
    }

    // **************************************************************** //

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("ShopperCreate");

      xtwRequest.WriteAttributeString("PLID", PrivateLabelId.ToString());
      xtwRequest.WriteAttributeString("IPAddress", GetLocalAddress());
      xtwRequest.WriteAttributeString("RequestedBy", m_sRequestedBy);

      xtwRequest.WriteEndElement(); // CreateShopper

      return sbRequest.ToString();
    }

    // **************************************************************** //

    string GetLocalAddress()
    {
      string sLocalAddress = "";

      IPAddress[] addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

      if (addresses.Length > 0)
        sLocalAddress = addresses[0].ToString();

      return sLocalAddress;
    }


    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}

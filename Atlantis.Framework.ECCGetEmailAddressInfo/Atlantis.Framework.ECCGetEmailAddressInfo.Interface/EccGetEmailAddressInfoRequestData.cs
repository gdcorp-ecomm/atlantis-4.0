using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EccGetEmailAddressInfo.Interface
{
  public class EccGetEmailAddressInfoRequestData : RequestData
  {
    public EccGetEmailAddressInfoRequestData(string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount, string emailAddress, EmailTypes productType) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      Type = productType;
      EmailAddress = emailAddress;

    }

    public string EmailAddress { get; set; }
    public EmailTypes Type { get; set; }
    public bool IncludeActiveOnly { get; set; }
    public bool IncludeDynamicData { get; set; }
    public string ResellerId { get; set; }
    public string Subaccount { get; set; }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      StringBuilder result = new StringBuilder();


      using (XmlTextWriter xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {

        xmlWriter.WriteStartElement("requestInfo");
        xmlWriter.WriteStartElement("dictionary");

        xmlWriter.WriteStartElement("ShopperId");
        xmlWriter.WriteValue(base.ShopperID);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("SourceUrl");
        xmlWriter.WriteValue(base.SourceURL);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("OrderId");
        xmlWriter.WriteValue(base.OrderID);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("Pathway");
        xmlWriter.WriteValue(base.Pathway);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("PageCount");
        xmlWriter.WriteValue(base.PageCount);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("EmailAddress");
        xmlWriter.WriteValue(EmailAddress);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("ResellerId");
        xmlWriter.WriteValue(ResellerId);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("Type");
        xmlWriter.WriteValue(Type.ToString());
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("IncludeActiveOnly");
        xmlWriter.WriteValue(IncludeActiveOnly.ToString());
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("IncludeDynamicData");
        xmlWriter.WriteValue(IncludeDynamicData.ToString());
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("Subaccount");
        xmlWriter.WriteValue(Subaccount);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }
  }
}

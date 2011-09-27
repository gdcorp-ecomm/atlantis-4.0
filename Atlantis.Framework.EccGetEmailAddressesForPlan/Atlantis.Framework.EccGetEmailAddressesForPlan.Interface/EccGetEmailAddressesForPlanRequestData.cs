using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EccGetEmailAddressesForPlan.Interface
{
  public class EccGetEmailAddressesForPlanRequestData : RequestData
  {
    public EccGetEmailAddressesForPlanRequestData(string sShopperID, string sSourceURL, string sOrderID, string sPathway, int iPageCount, int resellerId, string accountUid, bool includeActiveOnly) : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      ResellerId = resellerId;
      AccountUid = accountUid;
      IncludeActiveOnly = includeActiveOnly;
    }

    public bool IncludeActiveOnly { get; set; }
    public int ResellerId { get; set; }
    public string AccountUid { get; set; }

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


        xmlWriter.WriteStartElement("ResellerId");
        xmlWriter.WriteValue(ResellerId);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("AccountUid");
        xmlWriter.WriteValue(AccountUid);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("IncludeActiveOnly");
        xmlWriter.WriteValue(IncludeActiveOnly.ToString());
        xmlWriter.WriteEndElement();


        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

  }
}

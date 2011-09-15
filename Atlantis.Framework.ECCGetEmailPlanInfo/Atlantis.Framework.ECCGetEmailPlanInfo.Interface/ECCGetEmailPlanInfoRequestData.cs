using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCGetEmailPlanInfo.Interface
{
  public class EccGetEmailPlanInfoRequestData : RequestData
  {
    public string AccountUid { get; set; }
    public int ResellerId { get; set; }
    public EmailTypes EmailType { get; set; }
    public bool DeepLoad { get; set; }
    public string Subaccount { get; set; }
    
    public EccGetEmailPlanInfoRequestData(string shopperId,
                                          string sourceUrl,
                                          string orderId,
                                          string pathway,
                                          int pageCount,
                                          int resellerId,
                                          EmailTypes emailType,
                                          TimeSpan requestTimeout,
                                          string accountUid,
                                          string subaccount,
                                          bool deepLoad)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      EmailType = emailType;
      ResellerId = resellerId;
      RequestTimeout = requestTimeout;
      AccountUid = accountUid;
      Subaccount = subaccount;
      DeepLoad = deepLoad;
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }

    public override string ToXML()
    {
      var result = new StringBuilder();
      
      using (var xmlWriter = new XmlTextWriter(new StringWriter(result)))
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

        xmlWriter.WriteStartElement("EmailType");
        xmlWriter.WriteValue(Enum.GetName(typeof(EmailTypes), this.EmailType));
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("ResellerId");
        xmlWriter.WriteValue(this.ResellerId);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("RequestTimeout");
        xmlWriter.WriteValue(this.RequestTimeout);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("AccountUid");
        xmlWriter.WriteValue(this.AccountUid);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }
  }

}

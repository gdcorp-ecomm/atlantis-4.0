using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Ecc.Interface.Enums;
using Atlantis.Framework.Interface;


namespace Atlantis.Framework.ECCGetEmailPlansForShopper.Interface
{
  public class ECCGetEmailPlansForShopperRequestData : RequestData
  {
    public ECCGetEmailPlansForShopperRequestData(string shopperId,
                  string sourceUrl,
                  string orderId,
                  string pathway,
                  int pageCount,
                  int resellerId,
                  EmailTypes emailType,
                  TimeSpan requestTimeout)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      EmailType = emailType;
      ResellerId = resellerId;
      RequestTimeout = requestTimeout;
    }

    public override string ToXML()
    {
      var result = new StringBuilder();


      using (var xmlWriter = new XmlTextWriter(new StringWriter(result)))
      {

        xmlWriter.WriteStartElement("requestInfo");
        xmlWriter.WriteStartElement("dictionary");

        xmlWriter.WriteStartElement("ShopperId");
        xmlWriter.WriteValue(ShopperID);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("SourceUrl");
        xmlWriter.WriteValue(SourceURL);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("OrderId");
        xmlWriter.WriteValue(OrderID);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("Pathway");
        xmlWriter.WriteValue(Pathway);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("PageCount");
        xmlWriter.WriteValue(PageCount);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("EmailType");
        xmlWriter.WriteValue(Enum.GetName(typeof(EmailTypes), EmailType));
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("ResellerId");
        xmlWriter.WriteValue(ResellerId);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("RequestTimeout");
        xmlWriter.WriteValue(RequestTimeout);
        xmlWriter.WriteEndElement();


        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

    public int ResellerId { get; set; }
    public EmailTypes EmailType { get; set; }
    public TimeSpan RequestTimeout { get; set; }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes(string.Concat(base.ShopperID,
                                            "||",
                                            EmailType,
                                            "||",
                                            ResellerId));

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }

  }
}

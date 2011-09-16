using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ECCSetEmailAccount.Interface
{
  public class ECCDeleteEmailAccountRequestData : RequestData
  {
    public ECCDeleteEmailAccountRequestData(string shopperId,
        string sourceUrl,
        string orderId,
        string pathway,
        int pageCount,
        int resellerId,
        TimeSpan requestTimeout,        
        string emailAddress,                
        string subaccount       
      )
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      ResellerId = resellerId;
      RequestTimeout = requestTimeout;      
      EmailAddress = emailAddress;            
      Subaccount = subaccount;      
    }

    public ECCDeleteEmailAccountRequestData(string shopperId,
        string sourceUrl,
        string orderId,
        string pathway,
        int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    public int ResellerId { get; set; }
    public TimeSpan RequestTimeout { get; set; }    
    public string EmailAddress { get; set; }        
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


        xmlWriter.WriteStartElement("ResellerId");
        xmlWriter.WriteValue(this.ResellerId);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteStartElement("RequestTimeout");
        xmlWriter.WriteValue(this.RequestTimeout);
        xmlWriter.WriteEndElement();

        xmlWriter.WriteEndElement();
        xmlWriter.WriteEndElement();
      }

      return result.ToString();
    }

  }
}

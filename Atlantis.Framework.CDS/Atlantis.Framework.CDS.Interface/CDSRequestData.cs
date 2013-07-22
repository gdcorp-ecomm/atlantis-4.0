using Atlantis.Framework.Interface;
using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.CDS.Interface
{
  public class CDSRequestData : RequestData
  {
    public string Query { get; private set; }

    public CDSRequestData(string query)
    {
      Query = query;
      RequestTimeout = TimeSpan.FromSeconds(12);
    }

    public override string ToXML()
    {
      var sbRequest = new StringBuilder();
      var xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("RequestData", GetType().FullName);
      xtwRequest.WriteAttributeString("ShopperID", ShopperID);
      xtwRequest.WriteAttributeString("SourceURL", SourceURL);
      xtwRequest.WriteAttributeString("OrderID", OrderID);
      xtwRequest.WriteAttributeString("Pathway", Pathway);
      xtwRequest.WriteAttributeString("PageCount", Convert.ToString(PageCount));
      xtwRequest.WriteAttributeString("Query", Query);
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      return BuildHashFromStrings(Query);
    }
  }
}

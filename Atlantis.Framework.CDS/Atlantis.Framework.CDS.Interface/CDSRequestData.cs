using Atlantis.Framework.Interface;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Atlantis.Framework.CDS.Interface
{
  public class CDSRequestData : RequestData
  {
    const string TypeName = "CDSRequestData";
    public string AppName { get; set; }
    public string Query { get; private set; }

    public CDSRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount,
                                  string query)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      Query = query;
      RequestTimeout = TimeSpan.FromSeconds(20);
    }

    public override string ToXML()
    {
      var sbRequest = new StringBuilder();
      var xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("INFO");
      xtwRequest.WriteAttributeString("RequestData", this.GetType().FullName);
      xtwRequest.WriteAttributeString("AppName", AppName ?? "Unknown");
      xtwRequest.WriteAttributeString("ShopperID", ShopperID);
      xtwRequest.WriteAttributeString("SourceURL", SourceURL);
      xtwRequest.WriteAttributeString("OrderID", OrderID);
      xtwRequest.WriteAttributeString("Pathway", Pathway);
      xtwRequest.WriteAttributeString("PageCount", System.Convert.ToString(PageCount));
      xtwRequest.WriteAttributeString("Query", Query);
      xtwRequest.WriteEndElement();

      return sbRequest.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();

      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:Query:{1}", TypeName, Query));
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }
  }
}

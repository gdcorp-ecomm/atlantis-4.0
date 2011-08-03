using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Document.Interface
{
  public class DocumentRequestData : RequestData
  {
    public DocumentRequestData(
      string shopperId,
      string sourceUrl,
      string orderId,
      string pathway,
      int pageCount,
      int privateLabelId,
      string documentName)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BackColor = "#fff";
      ForeColor = "#000";
      Title = string.Empty;
      MarginWidth = 100;
      FullDocument = true;
      PrivateLabelId = privateLabelId;
      Name = documentName;
      RequestTimeout = TimeSpan.FromSeconds(2d);
    }

    public int PrivateLabelId { get; private set; }
    public string Name { get; private set; }
    public bool ShowError { get; set; }
    public bool FullDocument { get; set; }
    public int MarginWidth { get; set; }
    public string Title { get; set; }
    public string ForeColor { get; set; }
    public string BackColor { get; set; }

    #region RequestData Members

    public override string ToXML()
    {
      var sbResult = new StringBuilder();
      var xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("d3s");
      xtwResult.WriteAttributeString("privateLabelID", PrivateLabelId.ToString());
      
      xtwResult.WriteStartElement("document");
      xtwResult.WriteAttributeString("name", Name);
      xtwResult.WriteAttributeString("showError", ShowError.ToString().ToLowerInvariant());
      xtwResult.WriteAttributeString("fullDocument", FullDocument.ToString().ToLowerInvariant());
      xtwResult.WriteAttributeString("marginWidth", MarginWidth.ToString());
      xtwResult.WriteAttributeString("title", Title);

      xtwResult.WriteStartElement("format");
      xtwResult.WriteAttributeString("backColor", BackColor);
      xtwResult.WriteAttributeString("foreColor", ForeColor);
      xtwResult.WriteEndElement(); // format

      xtwResult.WriteEndElement(); // document
      xtwResult.WriteEndElement(); // d3s

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = Encoding.ASCII.GetBytes(ToXML());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    #endregion
  }
}

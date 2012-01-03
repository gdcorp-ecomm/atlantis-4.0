using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using System.Security.Cryptography;

namespace Atlantis.Framework.GetDomainInfo.Interface
{
  public class GetDomainInfoRequestData : RequestData
  {
    private string _shopperId = string.Empty;
    private string _domainName = string.Empty;

    public string ShopperId
    {
      get { return _shopperId; }
      set { _shopperId = value; }
    }
    public string DomainName
    {
      get { return _domainName; }
      set { _domainName = value; }
    }

    public GetDomainInfoRequestData(string sShopperID,
                  string sSourceURL,
                  string sOrderID,
                  string sPathway,
                  int iPageCount,
                  string shopperId,
                  string domainName
                  )
      : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      _shopperId = shopperId;
      _domainName = domainName;
    }

    public override string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter writer = new XmlTextWriter(new StringWriter(sbResult));

      const string GROUP_NAME = "Dev-Web-Sales";

      writer.WriteStartElement("request");

      writer.WriteStartElement("username");
      writer.WriteString(GROUP_NAME);
      writer.WriteEndElement();

      writer.WriteStartElement("option");
      writer.WriteAttributeString("name", "include_istransferprotected");
      writer.WriteAttributeString("value", "1");
      writer.WriteEndElement();

      writer.WriteStartElement("option");
      writer.WriteAttributeString("name", "include_isexpirationprotected");
      writer.WriteAttributeString("value", "1");
      writer.WriteEndElement();

      writer.WriteStartElement("option");
      writer.WriteAttributeString("name", "include_isproxied");
      writer.WriteAttributeString("value", "1");
      writer.WriteEndElement();

      writer.WriteStartElement("domain");
      writer.WriteAttributeString("domainname", _domainName ?? string.Empty);
      writer.WriteAttributeString("shopperid", _shopperId ?? string.Empty);
      writer.WriteEndElement();

      writer.WriteEndElement();

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 md5 = new MD5CryptoServiceProvider();

      byte[] data = Encoding.UTF8.GetBytes("GetDomainInfoRequestData::" + _shopperId + ":" + _domainName);

      byte[] hash = md5.ComputeHash(data);
      string result = Encoding.UTF8.GetString(hash);
      return result;
    }
  }
}

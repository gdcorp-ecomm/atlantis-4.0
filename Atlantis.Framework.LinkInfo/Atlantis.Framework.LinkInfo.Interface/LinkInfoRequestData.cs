using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.LinkInfo.Interface
{
  public class LinkInfoRequestData : RequestData
  {
    bool _allowEmptyLinkSet = false;
    int _contextId = 0;

    public LinkInfoRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount, int contextId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _contextId = contextId;
    }

    public int ContextID
    {
      get { return _contextId; }
      set { _contextId = value; }
    }

    public bool AllowEmptyLinkSet
    {
      get { return _allowEmptyLinkSet; }
      set { _allowEmptyLinkSet = value; }
    }

    #region RequestData Members

    public override string ToXML()
    {
      StringBuilder sbResult = new StringBuilder();
      XmlTextWriter xtwResult = new XmlTextWriter(new StringWriter(sbResult));

      xtwResult.WriteStartElement("LinkInfo");

      xtwResult.WriteStartElement("param");
      xtwResult.WriteAttributeString("name", "contextID");
      xtwResult.WriteAttributeString("value", _contextId.ToString());
      xtwResult.WriteEndElement(); // param

      xtwResult.WriteEndElement(); // LinkInfo

      return sbResult.ToString();
    }

    public override string GetCacheMD5()
    {
      MD5 oMD5 = new MD5CryptoServiceProvider();
      oMD5.Initialize();
      byte[] stringBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(_contextId.ToString());
      byte[] md5Bytes = oMD5.ComputeHash(stringBytes);
      string sValue = BitConverter.ToString(md5Bytes, 0);
      return sValue.Replace("-", "");
    }

    #endregion

  }
}

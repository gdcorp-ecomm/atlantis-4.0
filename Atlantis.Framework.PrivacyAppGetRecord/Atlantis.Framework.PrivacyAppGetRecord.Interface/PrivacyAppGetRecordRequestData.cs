using System;
using System.IO;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.PrivacyAppGetRecord.Interface
{
  public class PrivacyAppGetRecordRequestData : RequestData
  {
    private int _appId = 0;
    private string _hashKey = string.Empty;

    public PrivacyAppGetRecordRequestData(
      string shopperId,
      string sourceURL,
      string orderId,
      string pathway,
      int pageCount)
      : base(shopperId, sourceURL, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public string HashKey
    {
      get { return _hashKey; }
      set { _hashKey = value; }
    }

    public int ApplicationId
    {
      get { return _appId; }
      set { _appId = value; }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("PrivacyAppGetRecord is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));
      xtwRequest.WriteStartElement("request");


      xtwRequest.WriteEndElement();
      return sbRequest.ToString();
    }

  }

}

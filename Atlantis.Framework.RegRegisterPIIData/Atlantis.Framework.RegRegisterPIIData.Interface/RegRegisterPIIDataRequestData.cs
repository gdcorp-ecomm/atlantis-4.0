using System;
using Atlantis.Framework.Interface;
using System.Text;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Atlantis.Framework.RegRegisterPIIData.Interface
{
  public class RegRegisterPIIDataRequestData : RequestData
  {
    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(10);
    private string _sourceCode = string.Empty;
    private int _tldId = -1;
    private Dictionary<string, string> _requestValues = new Dictionary<string,string>();

    public RegRegisterPIIDataRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount,
      string sourceCode, int tldId)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      _sourceCode = sourceCode;
      _tldId = tldId;
    }

    public void AddRequestValue(string key, string value)
    {
      _requestValues[key] = value;
    }

    public int TldId
    {
      get { return _tldId; }
      set { _tldId = value; }
    }

    public string SourceCode
    {
      get { return _sourceCode; }
      set { _sourceCode = value; }
    }

    [Obsolete("Please use RequestTimeout instead.")]
    public TimeSpan ServiceTimeout
    {
      get { return RequestTimeout; }
      set { RequestTimeout = value; }
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("RegRegisterPIIData is not a cacheable request.");
    }

    public override string ToXML()
    {
      StringBuilder sbRequest = new StringBuilder();
      XmlTextWriter xtwRequest = new XmlTextWriter(new StringWriter(sbRequest));

      xtwRequest.WriteStartElement("application");
      xtwRequest.WriteAttributeString("clientapplication", _sourceCode);
      xtwRequest.WriteAttributeString("servername", Environment.MachineName);
      xtwRequest.WriteAttributeString("tldid", _tldId.ToString());

      foreach (string key in _requestValues.Keys)
      {
        if (!string.IsNullOrEmpty(key))
        {
          if (_requestValues[key] != null)
          {
            xtwRequest.WriteStartElement("key");
            xtwRequest.WriteAttributeString("name", key);
            xtwRequest.WriteAttributeString("value", _requestValues[key]);
            xtwRequest.WriteEndElement(); // key
          }
        }
      }

      xtwRequest.WriteEndElement(); // application
      return sbRequest.ToString();
    }
  }
}

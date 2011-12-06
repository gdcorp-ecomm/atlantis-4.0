using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.RegIsValidXN.Interface
{
  public class RegIsValidXNResponseData : IResponseData
  {
    private string _responseXml = string.Empty;
    private bool _isValid = false;
    private AtlantisException _ex;

    public RegIsValidXNResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isValid = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = false;

      XmlDocument _xmlDoc = new XmlDocument();
      _xmlDoc.LoadXml(_responseXml);
      XmlNode responseNode = _xmlDoc.SelectSingleNode("/response");
      if (responseNode != null)
      {
        XmlAttribute attr = (XmlAttribute)responseNode.Attributes.GetNamedItem("valid");
        if (attr != null)
        {
          result = (attr.Value == "1");
        }
      }

      return result;
    }

    public bool IsValid
    {
      get { return _isValid; }
    }

    public RegIsValidXNResponseData(string responseXml, AtlantisException ex)
    {
      _responseXml = responseXml;
      _isValid = false;
      _ex = ex;
    }

    public RegIsValidXNResponseData(string responseXml, RequestData oRequestData, Exception ex)
    {
      _responseXml = responseXml;
      _isValid = false;
      _ex = new AtlantisException(oRequestData, "RegIsValidXNResponseData", ex.Message, oRequestData.ToXML());
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _ex;
    }

    #endregion
  }
}

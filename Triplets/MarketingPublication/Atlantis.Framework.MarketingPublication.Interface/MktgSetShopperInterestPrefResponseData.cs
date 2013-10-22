using Atlantis.Framework.Interface;
using System;
using System.Xml;
using System.Xml.Linq;

namespace Atlantis.Framework.MarketingPublication.Interface
{
  public class MktgSetShopperInterestPrefResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;

    // Sample private constructor
    public MktgSetShopperInterestPrefResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = false;
      if (!string.IsNullOrEmpty(_responseXml))
      {
        XmlDocument _xmlDoc = new XmlDocument();
        _xmlDoc.LoadXml(_responseXml);
        string output = _xmlDoc.InnerText;
        result = output == "SUCCESS" ? true : false;
      }
      return result;
    }

    public MktgSetShopperInterestPrefResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public MktgSetShopperInterestPrefResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "MktgSetShopperInterestPrefResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}

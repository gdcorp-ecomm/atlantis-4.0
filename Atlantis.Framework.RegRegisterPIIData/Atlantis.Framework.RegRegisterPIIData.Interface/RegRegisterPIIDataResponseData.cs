using System;
using Atlantis.Framework.Interface;
using System.Xml;

namespace Atlantis.Framework.RegRegisterPIIData.Interface
{
  public class RegRegisterPIIDataResponseData : IResponseData
  {
    private string _responseXml = string.Empty;
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _token = string.Empty;

    public RegRegisterPIIDataResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _isSuccess = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = false;
      _token = string.Empty;

      XmlDocument _xmlDoc = new XmlDocument();
      _xmlDoc.LoadXml(_responseXml);
      XmlNode successNode = _xmlDoc.SelectSingleNode("success");
      if (successNode != null)
      {
        _token = successNode.InnerText;
        if (!string.IsNullOrEmpty(_token))
        {
          result = true;
        }
      }

      return result;
    }

    public string Token
    {
      get { return _token; }
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
    }

    public RegRegisterPIIDataResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _isSuccess = false;
      _exception = exAtlantis;
    }

    public RegRegisterPIIDataResponseData(string responseXml, RequestData requestData, Exception ex)
    {
      _responseXml = responseXml;
      _isSuccess = false;
      _exception = new AtlantisException(requestData, "RegRegisterPIIDataResponseData", ex.Message, ex.StackTrace);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _responseXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}

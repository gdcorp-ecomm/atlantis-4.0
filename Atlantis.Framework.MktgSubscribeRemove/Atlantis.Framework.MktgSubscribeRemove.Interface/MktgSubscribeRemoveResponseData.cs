using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeRemove.Interface
{
  public class MktgSubscribeRemoveResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private bool _isSuccess = false;
    private string _responseXml = string.Empty;
    
    public MktgSubscribeRemoveResponseData(string responseXml)
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

    public MktgSubscribeRemoveResponseData(AtlantisException exception)
    {
      _exception = exception;
    }

    public MktgSubscribeRemoveResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "MktgSubscribeRemoveResponseData", ex.Message, ex.StackTrace);
    }

    public bool IsSuccess
    {
      get { return _isSuccess; }
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

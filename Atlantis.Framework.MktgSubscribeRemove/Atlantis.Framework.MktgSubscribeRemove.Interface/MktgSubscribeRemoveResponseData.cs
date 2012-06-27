using System;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgSubscribeRemove.Interface
{
  public class MktgSubscribeRemoveResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _responseXml = string.Empty;

    public bool IsSuccess { get; private set; }

    public MktgSubscribeRemoveResponseData(string responseXml)
    {
      _responseXml = responseXml;
      IsSuccess = ParseResponse();
    }

    private bool ParseResponse()
    {
      bool result = false;
      if (!string.IsNullOrEmpty(_responseXml))
      {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(_responseXml);
        string output = xmlDoc.InnerText;
        result = output == "SUCCESS" ? true : false;
      }
      return result;
    }

    public MktgSubscribeRemoveResponseData(RequestData requestData, Exception ex)
    {
      _exception = new AtlantisException(requestData, "MktgSubscribeRemoveResponseData", ex.Message, ex.StackTrace);
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

using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MessagingProcess.Interface
{
  public class MessagingProcessResponseData : IResponseData
  {
    string _responseXml;
    AtlantisException _ex;

    public MessagingProcessResponseData(string responseXml)
    {
      _responseXml = responseXml;
      _ex = null;
    }

    public MessagingProcessResponseData(string responseXml, AtlantisException exAtlantis)
    {
      _responseXml = responseXml;
      _ex = exAtlantis;
    }

    public MessagingProcessResponseData(string sResponseXML, RequestData oRequestData, Exception ex)
    {
      _responseXml = sResponseXML;
      _ex = new AtlantisException(oRequestData,
                                   "MessagingProcessResponseData", 
                                   ex.Message, 
                                   oRequestData.ToXML());
    }

    public bool IsSuccess
    {
      get { return _responseXml.ToLowerInvariant().Contains("success"); }
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

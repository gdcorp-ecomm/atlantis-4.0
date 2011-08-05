using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MessagingShopperNote.Interface
{
  public class MessagingShopperNoteResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _responseXml = string.Empty;
    private bool _success = false;

    public bool IsSuccess
    {
      get { return _success; }
    }

    public MessagingShopperNoteResponseData(string xml)
    {
      _responseXml = xml;
      _success = true;
    }

     public MessagingShopperNoteResponseData(string xml, AtlantisException atlantisException)
    {
      _responseXml = xml;
      _exception = atlantisException;
    }

    public MessagingShopperNoteResponseData(string xml, RequestData requestData, Exception exception)
    {
      _responseXml = xml;
      _exception = new AtlantisException(requestData,
                                   "MessagingShopperNoteResponseData",
                                   exception.Message,
                                   requestData.ToXML());
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

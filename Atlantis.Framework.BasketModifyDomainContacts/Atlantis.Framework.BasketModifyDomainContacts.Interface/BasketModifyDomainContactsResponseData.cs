using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.BasketModifyDomainContacts.Interface
{
  public class BasketModifyDomainContactsResponseData : IResponseData
  {
    private string _responseXml;
    private AtlantisException _exception = null;

    public BasketModifyDomainContactsResponseData(string responseXml)
    {
      _responseXml = responseXml;
    }

    public BasketModifyDomainContactsResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public BasketModifyDomainContactsResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "BasketModifyDomainContactsResponseData",
                                   exception.Message, 
                                   requestData.ToXML());
    }

    public string ToXML()
    {
      return _responseXml;
    }

    public bool IsSuccess
    {
      get { return _responseXml.IndexOf("success", StringComparison.OrdinalIgnoreCase) > -1; }
    }

    public AtlantisException GetException()
    {
      return _exception;
    }
  }
}

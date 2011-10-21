using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketAttribute.Interface
{
  public class AddBasketAttributeResponseData : IResponseData
  {
    private readonly AtlantisException _exception;
    private readonly string _resultXml = string.Empty;

    private readonly bool _success;
    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }
    
    public AddBasketAttributeResponseData(string resultXml)
    {
      _resultXml = resultXml;
      _success = true;
    }

    public AddBasketAttributeResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public AddBasketAttributeResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "AddBasketAttribResponseData",
                                   exception.Message,
                                   string.Empty);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXml;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.AddBasketShipping.Interface
{
  public class AddBasketShippingResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public bool IsSuccess    
    {
      get
      {
        return _success;
      }
    }
    
    public AddBasketShippingResponseData(string resultXML)
    {
      _resultXML = resultXML;
      _success = true;
    }
    
    public AddBasketShippingResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public AddBasketShippingResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "AddBasketShippingResponseData",
                                   exception.Message,
                                   string.Empty);
    }

    #region IResponseData Members

    public string ToXML()
    {
      return _resultXML;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion
  }
}

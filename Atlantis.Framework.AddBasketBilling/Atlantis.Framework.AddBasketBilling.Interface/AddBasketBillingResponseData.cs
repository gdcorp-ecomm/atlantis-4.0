using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketBilling.Interface
{
  public class AddBasketBillingResponseData:IResponseData
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

    public AddBasketBillingResponseData(string resultXML)
    {
      _resultXML = resultXML;
      _success = true;
    }

    public AddBasketBillingResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public AddBasketBillingResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "AddBasketBillingResponseData",
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

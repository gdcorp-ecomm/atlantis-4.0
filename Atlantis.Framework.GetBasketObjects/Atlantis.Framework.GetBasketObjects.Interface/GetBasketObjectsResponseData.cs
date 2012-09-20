using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketObjects.Interface
{
  public class GetBasketObjectsResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;
    CartBasketOrder _currentBasket;

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public CartBasketOrder CurrentBasket
    {
      get
      {
        return _currentBasket;
      }
    }

    public GetBasketObjectsResponseData(string xml)
    {
      _currentBasket = new CartBasketOrder(xml);
    }

     public GetBasketObjectsResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public GetBasketObjectsResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "GetBasketObjectsResponseData",
                                   exception.Message,
                                   requestData.ToXML());
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

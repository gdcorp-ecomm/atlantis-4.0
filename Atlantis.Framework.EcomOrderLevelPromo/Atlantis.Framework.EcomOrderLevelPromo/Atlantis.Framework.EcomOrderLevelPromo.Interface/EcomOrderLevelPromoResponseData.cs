using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcomOrderLevelPromo.Interface
{
  public class EcomOrderLevelPromoResponseData : IResponseData
  {

    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    Dictionary<string, OrderLevelPromo> _orderLevelPromos = new Dictionary<string, OrderLevelPromo>();

    public Dictionary<string, OrderLevelPromo> OrderLevelPromos
    {
      get
      {
        return _orderLevelPromos;
      }
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }

    public EcomOrderLevelPromoResponseData(Dictionary<string,OrderLevelPromo> results)
    {
      if (results != null)
      {
        _orderLevelPromos = results;
        _success = true;
      }
    }

     public EcomOrderLevelPromoResponseData(AtlantisException atlantisException)
    {
      this._exception = atlantisException;
    }

    public EcomOrderLevelPromoResponseData(RequestData requestData, Exception exception)
    {
      this._exception = new AtlantisException(requestData,
                                   "EcomOrderLevelPromoResponseData",
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

  public class OrderLevelPromo
  {
    public string ISCCode { get; set; }
    public string ISCDescription { get; set; }
    public bool IsActive { get; set; }
  }
}

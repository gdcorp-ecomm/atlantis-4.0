using System;
using System.Collections.Generic;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.OrderShippingXml.Interface
{
  public class OrderShippingXmlResponseData : IResponseData
  {
    private AtlantisException _exception = null;
    private string _resultXML = string.Empty;
    private bool _success = false;

    public Dictionary<int, ShippingItem> ShippingOrderDict = new Dictionary<int, ShippingItem>(1);

    public OrderShippingXmlResponseData(Dictionary<int, ShippingItem> shippingItemsDict)
    {
      _success = true;
      ShippingOrderDict = shippingItemsDict;
    }

    public OrderShippingXmlResponseData(AtlantisException atlantisException)
    {
      _exception = atlantisException;
    }

    public OrderShippingXmlResponseData(RequestData requestData, Exception exception)
    {
      _exception = new AtlantisException(requestData,
                                   "OrderShippingXmlResponseData",
                                   exception.Message,
                                   requestData.ToXML());
    }

    public bool IsSuccess
    {
      get
      {
        return _success;
      }
    }


    #region IResponseData Members

    public string ToXML()
    {
      return string.Empty;
    }

    public AtlantisException GetException()
    {
      return _exception;
    }

    #endregion

  }
}

using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.TransferBasket.Interface
{
  public class TransferBasketRequestData : RequestData
  {
    private string _toShopperId = string.Empty;
    public string ToShopperId
    {
      get
      {
        return _toShopperId;
      }
      set
      {
        _toShopperId = value;
      }
    }

    private string _fromShopperId = string.Empty;
    public string FromShopperId
    {
      get
      {
        return _fromShopperId;
      }
      set
      {
        _fromShopperId = value;
      }
    }

    public TransferBasketRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount,
                                string fromShopperId,
                                string toShopperId)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      _fromShopperId = fromShopperId;
      _toShopperId = toShopperId;
    }

    public override string GetCacheMD5()
    {
      throw new Exception("TransferBasket is not a cacheable request.");
    }
  }
}

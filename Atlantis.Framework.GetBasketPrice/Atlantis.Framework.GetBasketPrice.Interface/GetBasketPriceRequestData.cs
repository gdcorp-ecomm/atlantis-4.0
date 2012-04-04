using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasketPrice.Interface
{
  public class GetBasketPriceRequestData : RequestData
  {
    // **************************************************************** //

    public GetBasketPriceRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount)
                                : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = _requestTimeout;
      DeleteRefund = false;
      PaymentType = string.Empty;
    }

    // **************************************************************** //

    public GetBasketPriceRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount,
                                bool bDeleteRefund,
                                string sPaymentType)
                                : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = _requestTimeout;
      DeleteRefund = bDeleteRefund;
      PaymentType = sPaymentType;
    }

    string _basketType = string.Empty;
    public string BasketType
    {
      get { return _basketType; }
      set { _basketType = value; }
    }

    public bool DeleteRefund { get; set; }

    public string PaymentType { get; set; }

    string _basketAttributes = string.Empty;
    public string BasketAttributes
    {
      get { return _basketAttributes; }
      set { _basketAttributes = value; }
    }

    private TimeSpan _requestTimeout = TimeSpan.FromSeconds(20);

    // **************************************************************** //

    #region RequestData Members

    // **************************************************************** //

    public override string GetCacheMD5()
    {
      throw new Exception("GetBasketPrice is not a cacheable request");
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //

  }
}

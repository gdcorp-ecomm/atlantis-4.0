using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetBasket.Interface
{
  public class GetBasketRequestData : RequestData
  {

    public GetBasketRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount)
                                : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount) 
    {
      RequestTimeout = TimeSpan.FromSeconds(3);
      DeleteRefund = false;
    }

    public GetBasketRequestData(string sShopperID,
                                string sSourceURL,
                                string sOrderID,
                                string sPathway,
                                int iPageCount,
                                bool bDeleteRefund)
                                : base(sShopperID, sSourceURL, sOrderID, sPathway, iPageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(3);
      DeleteRefund = bDeleteRefund;
    }

    string _basketType = string.Empty;
    public string BasketType
    {
      get { return _basketType; }
      set { _basketType = value; }
    }

    public bool DeleteRefund
    {
      get; set;
    }

    #region RequestData Members

    public override string GetCacheMD5()
    {
      throw new Exception("GetBasket is not a cacheable request");
    }

    #endregion

  }
}

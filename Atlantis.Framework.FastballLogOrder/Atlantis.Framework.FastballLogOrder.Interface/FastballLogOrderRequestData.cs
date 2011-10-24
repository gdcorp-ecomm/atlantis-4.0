using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.FastballLogOrder.Interface
{
  public class FastballLogOrderRequestData : RequestData
  {
    private string _basketType = string.Empty;
    public string BasketType
    {
      get
      {
        return _basketType;
      }
    }

    /// <summary>
    /// Used by Marketplace to log cart orders
    /// </summary>
    /// <param name="shopperID"></param>
    /// <param name="sourceURL"></param>
    /// <param name="orderID"></param>
    /// <param name="pathway"></param>
    /// <param name="pageCount"></param>
    /// <param name="basketType"></param>
    public FastballLogOrderRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount,
                                string basketType)
            : base(shopperID, sourceURL, orderID, pathway, pageCount)
        {
          _basketType = basketType;
        }

    /// <summary>
    /// Used to log cart orders
    /// </summary>
    /// <param name="shopperID"></param>
    /// <param name="sourceURL"></param>
    /// <param name="orderID"></param>
    /// <param name="pathway"></param>
    /// <param name="pageCount"></param>
    public FastballLogOrderRequestData(string shopperID,
                                string sourceURL,
                                string orderID,
                                string pathway,
                                int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}

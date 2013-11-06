using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.Basket.Interface
{
  public class BasketItemCountRequestData : RequestData
  {
    public BasketItemCountRequestData(string shopperId)
    {
      RequestTimeout = TimeSpan.FromSeconds(2);
      ShopperID = shopperId;
    }
  }
}

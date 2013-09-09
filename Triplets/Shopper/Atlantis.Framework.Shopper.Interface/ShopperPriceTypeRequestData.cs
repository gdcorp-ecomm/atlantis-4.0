using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.Shopper.Interface
{
  public class ShopperPriceTypeRequestData : RequestData
  {
    public int PrivateLabelId {get; private set;}
    
    public ShopperPriceTypeRequestData(string shopperId, int privateLabelId)
    {
      ShopperID = shopperId;
      PrivateLabelId = privateLabelId;
      RequestTimeout = new TimeSpan(0, 0, 5);
    }
  }
}

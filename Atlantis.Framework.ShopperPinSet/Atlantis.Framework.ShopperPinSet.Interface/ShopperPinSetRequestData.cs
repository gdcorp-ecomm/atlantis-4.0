using System;

using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperPinSet.Interface
{
  public class ShopperPinSetRequestData : RequestData
  {
    public ShopperPinSetRequestData(string shopperID,
                            string sourceURL,
                            string orderID,
                            string pathway,
                            int pageCount)
      : base(shopperID, sourceURL, orderID, pathway, pageCount)
    {
      RequestTimeout = new TimeSpan(0, 0, 5);
    }

    public override string GetCacheMD5()
    {
      throw new Exception("ShopperPinSet is not a cacheable request.");
    }
  }
}

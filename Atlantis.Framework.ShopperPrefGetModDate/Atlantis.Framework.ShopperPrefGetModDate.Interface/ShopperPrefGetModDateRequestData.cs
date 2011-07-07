using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperPrefGetModDate.Interface
{
  public class ShopperPrefGetModDateRequestData : RequestData
  {

    public ShopperPrefGetModDateRequestData(string shopperId,
                                  string sourceUrl,
                                  string orderId,
                                  string pathway,
                                  int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      RequestTimeout = TimeSpan.FromSeconds(5);
    }


    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in ShopperPrefGetModDateRequestData");     
    }
  }
}

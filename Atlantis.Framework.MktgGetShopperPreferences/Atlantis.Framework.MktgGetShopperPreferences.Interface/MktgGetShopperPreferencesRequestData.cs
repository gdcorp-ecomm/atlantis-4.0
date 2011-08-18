using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.MktgGetShopperPreferences.Interface
{
  public class MktgGetShopperPreferencesRequestData: RequestData
  {

    public MktgGetShopperPreferencesRequestData(
      string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("MktgGetShopperPreferencesRequestData is not a cacheable request.");
    }
  }
}

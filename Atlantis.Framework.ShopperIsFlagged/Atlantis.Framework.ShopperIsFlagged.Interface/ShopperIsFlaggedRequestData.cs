using System;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ShopperIsFlagged.Interface
{
  public class ShopperIsFlaggedRequestData: RequestData
  {
    public ShopperIsFlaggedRequestData(string shopperId, string sourceUrl, string orderId, string pathway, int pageCount)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    { }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException();
    }
  }
}

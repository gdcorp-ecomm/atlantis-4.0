using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;

namespace Atlantis.Framework.Products.Impl
{
  public class NonUnifiedPfidRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = NonUnifiedPfidResponseData.NotFound;

      var nonUnifiedRequest = (NonUnifiedPfidRequestData)requestData;
      int pfid = 0;
      using (GdDataCacheOutOfProcess outCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        pfid = outCache.ConvertToPFID(nonUnifiedRequest.UnifiedProductId, nonUnifiedRequest.PrivateLabelId);
      }

      return NonUnifiedPfidResponseData.FromNonUnifiedPfid(pfid);
    }
  }
}

using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;

namespace Atlantis.Framework.Products.Impl
{
  public class NonUnifiedPfidRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = NonUnifiedPfidResponseData.NotFound;

      try
      {
        var nonUnifiedRequest = (NonUnifiedPfidRequestData)requestData;
        int pfid = 0;
        using (GdDataCacheOutOfProcess outCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          pfid = outCache.ConvertToPFID(nonUnifiedRequest.UnifiedProductId, nonUnifiedRequest.PrivateLabelId);
        }

        result = NonUnifiedPfidResponseData.FromNonUnifiedPfid(pfid);
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "NonUnifiedPfidRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
      }

      return result;
    }
  }
}

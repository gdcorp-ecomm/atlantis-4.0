using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.TLDDataCache.Interface;
using System;

namespace Atlantis.Framework.TLDDataCache.Impl
{
  public class OfferedTLDsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = null;

      try
      {
        var offeredTldRequest = ((OfferedTLDsRequestData)requestData);

        if (offeredTldRequest.TLDProductType != OfferedTLDProductTypes.Invalid)
        {
          string cacheXml;
          using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
          {
            cacheXml = comCache.GetTLDList(offeredTldRequest.PrivateLabelId, (int)offeredTldRequest.TLDProductType);
          }
          result = OfferedTLDsResponseData.FromCacheXml(cacheXml);
        }
        else
        {
          result = OfferedTLDsResponseData.Empty;
        }
      }
      catch (Exception ex)
      {
        AtlantisException exception = new AtlantisException(requestData, "OfferedTLDsRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        result = OfferedTLDsResponseData.FromException(exception);
      }

      return result;
    }
  }
}

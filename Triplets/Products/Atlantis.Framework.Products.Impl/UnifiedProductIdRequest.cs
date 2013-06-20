using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;

namespace Atlantis.Framework.Products.Impl
{
  public class UnifiedProductIdRequest : IRequest
  {
    private const string GET_UNIFIED_ID_BY_PFID_REQUEST_FORMAT = "<GetUnifiedIDByPFID><param name=\"n_pf_id\" value=\"{0}\"/><param name=\"n_privateLabelID\" value=\"{1}\"/></GetUnifiedIDByPFID>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = UnifiedProductIdResponseData.NotFound;

      try
      {
        var unifiedProductIdRequest = (UnifiedProductIdRequestData)requestData;
        string requestXml = string.Format(GET_UNIFIED_ID_BY_PFID_REQUEST_FORMAT, unifiedProductIdRequest.NonUnifiedPfid, unifiedProductIdRequest.PrivateLabelId);
        using (var outCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          string responseXml = outCache.GetCacheData(requestXml);
          result = UnifiedProductIdResponseData.FromCacheData(responseXml);
        }
      }
      catch(Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "UnifiedProductIdRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }
  }
}

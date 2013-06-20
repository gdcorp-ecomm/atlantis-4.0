using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;

namespace Atlantis.Framework.Products.Impl
{
  public class ProductInfoRequest : IRequest
  {
    private const string _REQUESTFORMAT = "<GetProductInfoByUnifiedPFID><param name=\"n_gdshop_product_unifiedProductID\" value=\"{0}\"/><param name=\"n_privateLabelID\" value=\"{1}\"/></GetProductInfoByUnifiedPFID>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData result = ProductInfoResponseData.None;

      try
      {
        var productInfoRequestData = (ProductInfoRequestData)requestData;
        string requestXml = string.Format(_REQUESTFORMAT, productInfoRequestData.UnifiedProductId, productInfoRequestData.PrivateLabelId);
        using (var outCache = GdDataCacheOutOfProcess.CreateDisposable())
        {
          string responseXml = outCache.GetCacheData(requestXml);
          result = ProductInfoResponseData.FromCacheData(responseXml);
        }
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData, "ProductInfoRequest.RequestHandler", ex.Message + ex.StackTrace, requestData.ToXML());
        Engine.Engine.LogAtlantisException(aex);
      }

      return result;
    }
  }
}

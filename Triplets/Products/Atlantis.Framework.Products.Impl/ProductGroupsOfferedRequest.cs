using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Products.Interface;
using System;

namespace Atlantis.Framework.Products.Impl
{
  public class ProductGroupsOfferedRequest : IRequest
  {
    const string _GETPRODUCTOFFERINGSBYPLID_FORMAT = "<GetProductOfferingsByPLID><param name=\"n_privateLabelID\" value=\"{0}\"/></GetProductOfferingsByPLID>";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ProductGroupsOfferedRequestData request = (ProductGroupsOfferedRequestData)requestData;
      string requestXml = string.Format(_GETPRODUCTOFFERINGSBYPLID_FORMAT, request.PrivateLabelId.ToString());
      string responseXml;

      using (var outCache = GdDataCacheOutOfProcess.CreateDisposable())
      {
        responseXml = outCache.GetCacheData(requestXml);
      }

      ProductGroupsOfferedResponseData result = ProductGroupsOfferedResponseData.FromCacheXml(responseXml);

      if ((result.Count == 0) && (request.PrivateLabelId == 1))
      {
        throw new Exception("ProductOffer GetCacheData returned no offerings for private label 1.");
      }

      return result;
    }
  }
}

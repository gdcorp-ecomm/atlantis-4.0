using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommPricing.Impl
{
    public class PriceGroupsByCountrySiteRequest : IRequest
    {
      const string _ATLANTISPRICEGROUPCOUNTRYMAPSETTING = "ATLANTIS_PRICEGROUP_COUNTRYSITE_MAPPING";

      #region IRequest Members

      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result = PriceGroupsByCountrySiteResponseData.NoPriceGroupsMappingResponse;
        if (requestData is PriceGroupsByCountrySiteRequestData)
        {
          string priceGroupMapping;
          using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
          {
            priceGroupMapping = comCache.GetAppSetting(_ATLANTISPRICEGROUPCOUNTRYMAPSETTING);
          }
          result = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(priceGroupMapping);
        }
        return result;
      }

      #endregion
    }
}

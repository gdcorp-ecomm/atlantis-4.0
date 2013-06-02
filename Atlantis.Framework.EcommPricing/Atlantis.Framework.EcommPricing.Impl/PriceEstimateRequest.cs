using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.EcommPricing.Impl
{
    public class PriceEstimateRequest : IRequest
    {
      private const string _REQUESTFORMAT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\" source_code=\"{3}\"><Item unifiedProductID=\"{4}\" priceGroupID=\"{5}\"/></PriceEstimateRequest>";
      private const string _REQUESTFORMATDISCOUNT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\"><Item unifiedProductID=\"{4}\" discount_code=\"{3}\"/></PriceEstimateRequest>";

      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result;

        try
        {
          var priceEstimateRequest = (PriceEstimateRequestData)requestData;
          
          string serviceRequest = string.Empty;
          if (!string.IsNullOrEmpty(priceEstimateRequest.DiscountCode))
          {
            serviceRequest = string.Format(_REQUESTFORMATDISCOUNT,
                                              priceEstimateRequest.PrivateLabelId,
                                              priceEstimateRequest.ShopperPriceType,
                                              priceEstimateRequest.CurrencyType,
                                              priceEstimateRequest.DiscountCode,
                                              priceEstimateRequest.UnifiedProductId,
                                              priceEstimateRequest.PriceGroupId);
          }
          else
          {
            serviceRequest = string.Format(_REQUESTFORMAT,
                                              priceEstimateRequest.PrivateLabelId,
                                              priceEstimateRequest.ShopperPriceType,
                                              priceEstimateRequest.CurrencyType,
                                              priceEstimateRequest.PromoCode,
                                              priceEstimateRequest.UnifiedProductId,
                                              priceEstimateRequest.PriceGroupId);
          }

          string pricingResponse;

          using (var comCache = GdDataCacheOutOfProcess.CreateDisposable())
          {
            pricingResponse = comCache.GetPriceEstimate(serviceRequest);
          }

          result = PriceEstimateResponseData.FromXml(pricingResponse);
        }
        catch (Exception ex)
        {
          string message = ex.Message + ex.StackTrace;
          var aex = new AtlantisException(requestData, "RequestHandler", message, requestData.ToXML());
          result = PriceEstimateResponseData.FromException(aex);
        }

        return result;
      }
    }
}

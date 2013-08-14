using Atlantis.Framework.DataCacheService;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using System;

namespace Atlantis.Framework.EcommPricing.Impl
{
    public class PriceEstimateRequest : IRequest
    {
      private const string _REQUESTFORMAT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\" source_code=\"{3}\"><Item unifiedProductID=\"{5}\" priceGroupID=\"{6}\"/></PriceEstimateRequest>";
      private const string _REQUESTFORMATYARD = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\" source_code=\"{3}\" YARD=\"{7}\"><Item unifiedProductID=\"{5}\" priceGroupID=\"{6}\" /></PriceEstimateRequest>";
      private const string _REQUESTFORMATDISCOUNT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\"><Item unifiedProductID=\"{5}\" discount_code=\"{4}\"/></PriceEstimateRequest>";
      private const string _REQUESTFORMATDISCOUNTYARD = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\" YARD=\"{7}\"><Item unifiedProductID=\"{5}\" discount_code=\"{4}\"/></PriceEstimateRequest>";

      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result;

        try
        {
          var priceEstimateRequest = (PriceEstimateRequestData)requestData;
          
          string serviceRequest = string.Empty;
          string formattedXML = string.Empty;

          if (!string.IsNullOrEmpty(priceEstimateRequest.DiscountCode))
          {
            formattedXML = priceEstimateRequest.YARD < 1 ? _REQUESTFORMATDISCOUNT :  _REQUESTFORMATDISCOUNTYARD;
          }
          else
          {
            formattedXML = priceEstimateRequest.YARD < 1 ? _REQUESTFORMAT : _REQUESTFORMATYARD;
          }
          
          serviceRequest = string.Format(formattedXML,
                                              priceEstimateRequest.PrivateLabelId,
                                              priceEstimateRequest.ShopperPriceType,
                                              priceEstimateRequest.CurrencyType,
                                              priceEstimateRequest.PromoCode,
                                              priceEstimateRequest.DiscountCode,
                                              priceEstimateRequest.UnifiedProductId,
                                              priceEstimateRequest.PriceGroupId,
                                              priceEstimateRequest.YARD);          

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

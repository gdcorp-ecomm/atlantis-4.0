using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Atlantis.Framework.EcommPricing.Impl
{
    public class PriceEstimateRequest : IRequest
    {
      private const string _REQUESTFORMAT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\" source_code=\"{3}\"><Item pf_id=\"{4}\" priceGroupID=\"{5}\"/></PriceEstimateRequest>";
      private const string _REQUESTFORMATDISCOUNT = "<PriceEstimateRequest privateLabelID=\"{0}\" membershipLevel=\"{1}\" transactionCurrency=\"{2}\"><Item pf_id=\"{4}\" discount_code=\"{3}\"/></PriceEstimateRequest>";

      public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
      {
        IResponseData result;

        try
        {
          var wsConfigElement = (WsConfigElement)config;
          string wsUrl = wsConfigElement.WSURL;
          if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
          {
            throw new AtlantisException(requestData, "PriceEstimateRequest::RequestHandler", "PriceEstimateRequest WS URL in atlantis.config must use https.", string.Empty);
          }
          
          var priceEstimateRequest = (PriceEstimateRequestData)requestData;
          
          string serviceRequest = string.Empty;
          if (!string.IsNullOrEmpty(priceEstimateRequest.DiscountCode))
          {
            serviceRequest = string.Format(_REQUESTFORMATDISCOUNT,
                                              priceEstimateRequest.PrivateLabelId,
                                              priceEstimateRequest.ShopperPriceType,
                                              priceEstimateRequest.CurrencyType,
                                              priceEstimateRequest.DiscountCode,
                                              DataCache.DataCache.GetPFIDByUnifiedID(priceEstimateRequest.UnifiedProductId, priceEstimateRequest.PrivateLabelId),
                                              priceEstimateRequest.PriceGroupId);
          }
          else
          {
            serviceRequest = string.Format(_REQUESTFORMAT,
                                              priceEstimateRequest.PrivateLabelId,
                                              priceEstimateRequest.ShopperPriceType,
                                              priceEstimateRequest.CurrencyType,
                                              priceEstimateRequest.PromoCode,
                                              DataCache.DataCache.GetPFIDByUnifiedID(priceEstimateRequest.UnifiedProductId, priceEstimateRequest.PrivateLabelId),
                                              priceEstimateRequest.PriceGroupId);
          }

          using (var oSvc = new PricingService.Service())
          {
            
            oSvc.Url = wsUrl;
            oSvc.Timeout = (int) priceEstimateRequest.RequestTimeout.TotalMilliseconds;

            X509Certificate2 clientCert = wsConfigElement.GetClientCertificate();
            oSvc.ClientCertificates.Add(clientCert);

            string ecommResult = oSvc.GetPriceEstimateEx(serviceRequest);

            result = PriceEstimateResponseData.FromXml(ecommResult);
          }
          
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

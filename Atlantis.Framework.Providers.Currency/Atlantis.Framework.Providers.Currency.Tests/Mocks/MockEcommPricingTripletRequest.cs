using Atlantis.Framework.EcommPricing.Impl;
using Atlantis.Framework.EcommPricing.Interface;
using Atlantis.Framework.Interface;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Atlantis.Framework.Providers.Currency.Tests.Mocks
{
  public class MockEcommPricingTripletRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      if (requestData is ValidateNonOrderRequestData)
        return ValidateNonOrderHandleRequest(requestData, config);

      if (requestData is PriceEstimateRequestData)
        return PriceEstimateHandleRequest(requestData, config);

      if (requestData is ListPriceRequestData)
        return ListPriceHandleRequest(requestData, config);

      if (requestData is PromoPriceRequestData)
        return PromoPriceHandleRequest(requestData, config);

      if (requestData is ProductIsOnSaleRequestData)
        return ProductIsOnSaleHandleRequest(requestData, config);

      if (requestData is PriceGroupsByCountrySiteRequestData)
        return PriceGroupsByCountrySiteHandleRequest(requestData, config);

      return null;
    }

    private IResponseData PriceGroupsByCountrySiteHandleRequest(RequestData requestData, ConfigElement config)
    {
      string mapping = "|US:1||UK:|:99|PH:2|IN:BAD|CA:3|AU:4|XXXX:-99";
      var response = PriceGroupsByCountrySiteResponseData.FromCountrySiteMapping(mapping);
      return response;
    }

    private IResponseData ProductIsOnSaleHandleRequest(RequestData requestData, ConfigElement config)
    {
      var handler = new ProductIsOnSaleRequest();
      return handler.RequestHandler(requestData, config);
    }

    private IResponseData PromoPriceHandleRequest(RequestData requestData, ConfigElement config)
    {
      var handler = new PromoPriceRequest();
      return handler.RequestHandler(requestData, config);
    }

    private IResponseData ListPriceHandleRequest(RequestData requestData, ConfigElement config)
    {
      var handler = new ListPriceRequest();
      return handler.RequestHandler(requestData, config);
    }

    private IResponseData PriceEstimateHandleRequest(RequestData requestData, ConfigElement config)
    {
      PriceEstimateRequestData request = requestData as PriceEstimateRequestData;
      if (request.PromoCode == "VALID")
      {
        return GetMockIscPriceEstimate(request, config);        
      }
      else if (request.PromoCode == "INVALID" || request.PromoCode == "EXPIRED" || request.PromoCode == "INACTIVE")
      {
        return PriceEstimateResponseData.NoPriceFoundResponse;
      }
      else
      {
        var handler = new PriceEstimateRequest();
        return handler.RequestHandler(requestData, config);
      }
    }

    private IResponseData GetMockIscPriceEstimate(PriceEstimateRequestData request, ConfigElement config)
    {
      string text = GetTextDataResource("PriceEstimateValid.xml");
      XElement root = XElement.Parse(text);
      XElement item = root.Elements("PriceEstimate")
        .Where(pe => pe.Attribute("transactionCurrency").Value == request.CurrencyType)
        .FirstOrDefault(pe => pe.Elements("Item").Any(i => i.Attribute("pf_id").Value == request.UnifiedProductId.ToString()));

      if (item != null)
      {
        return PriceEstimateResponseData.FromXml(item.ToString());
      }
      else
      {
        return PriceEstimateResponseData.NoPriceFoundResponse;
      }
      
    }

    #endregion

    #region ValidateNonOrder

    private IResponseData ValidateNonOrderHandleRequest(RequestData requestData, ConfigElement config)
    {
      IResponseData response = null;
      var request = requestData as ValidateNonOrderRequestData;

      if (request.PromoCode == "INACTIVE")
      {
        response = ValidateNonOrderResponseData.InActiveResponse;
      }
      else if (request.PromoCode == "EXPIRED")
      {
        string text = GetTextDataResource("ValidateNonOrderExpired.xml");
        response = ValidateNonOrderResponseData.FromCacheDataXml(text);
      }
      else if (request.PromoCode == "INVALID")
      {
        string text = GetTextDataResource("ValidateNonOrderInvalid.xml");
        response = ValidateNonOrderResponseData.FromCacheDataXml(text);
      }
      else if (request.PromoCode == "VALID")
      {
        string text = GetTextDataResource("ValidateNonOrderValid.xml");
        response = ValidateNonOrderResponseData.FromCacheDataXml(text);
      }
      else
      {
        var handler = new ValidateNonOrderRequest();
        response = handler.RequestHandler(requestData, config);
      }

      return response;
    }

    #endregion

    private static string GetTextDataResource(string dataName)
    {
      string result;
      string resourcePath = "Atlantis.Framework.Providers.Currency.Tests." + dataName;
      Assembly assembly = Assembly.GetExecutingAssembly();
      using (StreamReader textReader = new StreamReader(assembly.GetManifestResourceStream(resourcePath)))
      {
        result = textReader.ReadToEnd();
      }
      return result;
    }
  }
}

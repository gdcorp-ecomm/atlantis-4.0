using System;
using Atlantis.Framework.GetOverrideHash.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetOverrideHash.Impl
{
  public class GetOverrideHashRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;
      string hash = null;

      try
      {
        var oGetOverrideHashRequestData = (GetOverrideHashRequestData)requestData;

        string now = DateTime.Now.Date.ToString();

        object hashObject;
        if (oGetOverrideHashRequestData.GetPriceTypeHash)
        {
          using (var pricetype = new OverridePriceWrapper("pricetype"))
          {
            hashObject = pricetype.PriceType.GetHash(oGetOverrideHashRequestData.PrivateLabelID.ToString(),
                                                oGetOverrideHashRequestData.UnifiedPFID.ToString(),
                                                oGetOverrideHashRequestData.OverridePriceTypeId.ToString(),
                                                now);
          }
        }
        else
        {
          using (var price = new OverridePriceWrapper())
          {
            if (oGetOverrideHashRequestData.GetCostHash)
            {
              hashObject = price.Price.GetCostHash(oGetOverrideHashRequestData.PrivateLabelID.ToString(),
                                          oGetOverrideHashRequestData.UnifiedPFID.ToString(),
                                          oGetOverrideHashRequestData.OverrideListPrice.ToString(),
                                          oGetOverrideHashRequestData.OverrideCurrentPrice.ToString(),
                                          oGetOverrideHashRequestData.OverrideCurrentCost.ToString(),
                                          now);
            }
            else
            {
              hashObject = price.Price.GetHash(oGetOverrideHashRequestData.PrivateLabelID.ToString(),
                                          oGetOverrideHashRequestData.UnifiedPFID.ToString(),
                                          oGetOverrideHashRequestData.OverrideListPrice.ToString(),
                                          oGetOverrideHashRequestData.OverrideCurrentPrice.ToString(),
                                          now);
            }
          }
        }

        hash = Convert.ToString(hashObject);
        responseData = new GetOverrideHashResponseData(hash);
      }
      catch (Exception ex)
      {
        responseData = new GetOverrideHashResponseData(hash, requestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}

using System;

using Atlantis.Framework.GetOverrideHash.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetOverrideHash.Impl
{
  public class GetOverrideHashRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData;
      string sHash = null;

      try
      {
        var oGetOverrideHashRequestData = (GetOverrideHashRequestData)oRequestData;

        string now = DateTime.Now.Date.ToString();

        object oHash;
        if (oGetOverrideHashRequestData.GetPriceTypeHash)
        {
          using (var pricetype = new OverridePriceWrapper("pricetype"))
          {
            oHash = pricetype.PriceType.GetHash(oGetOverrideHashRequestData.PrivateLabelID.ToString(),
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
              oHash = price.Price.GetCostHash(oGetOverrideHashRequestData.PrivateLabelID.ToString(),
                                          oGetOverrideHashRequestData.UnifiedPFID.ToString(),
                                          oGetOverrideHashRequestData.OverrideListPrice.ToString(),
                                          oGetOverrideHashRequestData.OverrideCurrentPrice.ToString(),
                                          oGetOverrideHashRequestData.OverrideCurrentCost.ToString(),
                                          now);
            }
            else
            {
              oHash = price.Price.GetHash(oGetOverrideHashRequestData.PrivateLabelID.ToString(),
                                          oGetOverrideHashRequestData.UnifiedPFID.ToString(),
                                          oGetOverrideHashRequestData.OverrideListPrice.ToString(),
                                          oGetOverrideHashRequestData.OverrideCurrentPrice.ToString(),
                                          now);
            }
          }
        }

        sHash = Convert.ToString(oHash);

        oResponseData = new GetOverrideHashResponseData(sHash);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new GetOverrideHashResponseData(sHash, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new GetOverrideHashResponseData(sHash, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

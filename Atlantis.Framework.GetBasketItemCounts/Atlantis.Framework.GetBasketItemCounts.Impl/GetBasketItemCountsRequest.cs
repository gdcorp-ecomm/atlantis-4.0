using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.GetBasketItemCounts.Interface;

namespace Atlantis.Framework.GetBasketItemCounts.Impl
{
  public class GetBasketItemCountsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetBasketItemCountsResponseData oResponseData;
      string responseXml = string.Empty;

      try
      {
        var oGetBasketItemCountsRequestData = (GetBasketItemCountsRequestData)oRequestData;

        using (var oBasketWS = new WSCgdBasket.WscgdBasketService())
        {
          oBasketWS.Url = ((WsConfigElement) oConfig).WSURL;
          responseXml = string.Empty;
          responseXml = oBasketWS.GetItemCounts(oGetBasketItemCountsRequestData.ShopperID);
          if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            var exAtlantis = new AtlantisException(oRequestData,
                                                                 "GetBasketItemCountsRequest.RequestHandler",
                                                                 responseXml,
                                                                 oRequestData.ToXML());

            oResponseData = new GetBasketItemCountsResponseData(responseXml, exAtlantis);
          }
          else
            oResponseData = new GetBasketItemCountsResponseData(responseXml);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new GetBasketItemCountsResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new GetBasketItemCountsResponseData(responseXml, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

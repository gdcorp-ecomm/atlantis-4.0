using System;
using Atlantis.Framework.DeleteItem.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DeleteItem.Impl
{
  public class DeleteItemRequest : IRequest
  {
    #region IRequest Members

    private const int LOCKCUSTOMER = 1;
    private const int LOCKMANAGER = 2;

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DeleteItemResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        DeleteItemRequestData oDeleteItemRequestData = (DeleteItemRequestData)oRequestData;
        WSCgdBasket.WscgdBasketService oBasketWS = new WSCgdBasket.WscgdBasketService();
        oBasketWS.Url = ((WsConfigElement)oConfig).WSURL;

        int lockingMode = oDeleteItemRequestData.IsManager ? LOCKMANAGER : LOCKCUSTOMER;

        sResponseXML = oBasketWS.DeleteItemsByPairWithType(
          oDeleteItemRequestData.ShopperID,
          oDeleteItemRequestData.BasketType,
          oDeleteItemRequestData.ItemKeysToDelete, 
          lockingMode);

        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "DeleteItemRequest.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new DeleteItemResponseData(sResponseXML, exAtlantis);
        }
        else
          oResponseData = new DeleteItemResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DeleteItemResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new DeleteItemResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

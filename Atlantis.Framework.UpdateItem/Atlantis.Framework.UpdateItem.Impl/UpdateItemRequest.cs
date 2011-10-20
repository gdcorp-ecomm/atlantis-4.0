
using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.UpdateItem.Interface;
namespace Atlantis.Framework.UpdateItem.Impl
{
  public class UpdateItemRequest : IRequest
  {
    #region IRequest Members

    private const int LOCKCUSTOMER = 1;
    private const int LOCKMANAGER = 2;

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      UpdateItemResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        UpdateItemRequestData oUpdateItemRequestData = (UpdateItemRequestData)oRequestData;
        WSCgdBasket.WscgdBasketService oBasketWS = new WSCgdBasket.WscgdBasketService();
        oBasketWS.Url = ((WsConfigElement)oConfig).WSURL;

        int lockingMode = oUpdateItemRequestData.IsManager ? LOCKMANAGER : LOCKCUSTOMER;
        sResponseXML = string.Empty;
        
        sResponseXML=oBasketWS.UpdateItemByType(oUpdateItemRequestData.ShopperID, oUpdateItemRequestData.BasketType,
              oUpdateItemRequestData.RowID, oUpdateItemRequestData.ItemID, oUpdateItemRequestData.ItemXML());
        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "UpdateItemRequest.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new UpdateItemResponseData(sResponseXML, exAtlantis);
        }
        else
          oResponseData = new UpdateItemResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new UpdateItemResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new UpdateItemResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}



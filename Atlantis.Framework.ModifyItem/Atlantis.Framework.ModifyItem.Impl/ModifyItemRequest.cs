using System;
using System.Collections.Generic;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ModifyItem.Interface;

namespace Atlantis.Framework.ModifyItem.Impl
{
  public class ModifyItemRequest : IRequest
  {
    // **************************************************************** //

    #region IRequest Members

    // **************************************************************** //

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      ModifyItemResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        ModifyItemRequestData oModifyItemRequestData = (ModifyItemRequestData)oRequestData;
        using (WSCgdBasket.WscgdBasketService oBasketWS = new WSCgdBasket.WscgdBasketService())
        {
          oBasketWS.Url = ((WsConfigElement)oConfig).WSURL;
          oBasketWS.Timeout = (int)oModifyItemRequestData.RequestTimeout.TotalMilliseconds;
          if (oModifyItemRequestData.ItemCount == 0)
          {
            if (!string.IsNullOrEmpty(oModifyItemRequestData.BasketType))
            {
              sResponseXML = oBasketWS.ModifyItemByType(
                oModifyItemRequestData.ShopperID,
                oModifyItemRequestData.BasketType,
                oModifyItemRequestData.Index,
                oModifyItemRequestData.Quantity);
            }
            else
            {
              sResponseXML = oBasketWS.ModifyItem(oModifyItemRequestData.ShopperID,
                                                  oModifyItemRequestData.Index,
                                                  oModifyItemRequestData.Quantity);
            }
          }
          else
          {
            string requestXML = oModifyItemRequestData.ItemXML();
            sResponseXML = oBasketWS.ModifyItemsByType(oModifyItemRequestData.ShopperID,
                oModifyItemRequestData.BasketType, requestXML);
          }
        }
        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "ShopperRequest.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new ModifyItemResponseData(sResponseXML, exAtlantis);
        }
        else
          oResponseData = new ModifyItemResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ModifyItemResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ModifyItemResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}

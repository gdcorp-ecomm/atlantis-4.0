using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.UpdateShopper.Interface;

namespace Atlantis.Framework.UpdateShopper.Impl
{
  public class UpdateShopperRequest : IRequest
  {
    // **************************************************************** //

    #region IRequest Members

    // **************************************************************** //

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      UpdateShopperResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        UpdateShopperRequestData oSearchRequestData = (UpdateShopperRequestData)oRequestData;
        using (WscgdShopper.WSCgdShopperService shopperWS = new WscgdShopper.WSCgdShopperService())
        {
          shopperWS.Url = ((WsConfigElement)oConfig).WSURL;
          shopperWS.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;
          sResponseXML = shopperWS.UpdateShopper(oSearchRequestData.ToXML());
          if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 sResponseXML, string.Empty);

            oResponseData = new UpdateShopperResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            oResponseData = new UpdateShopperResponseData(sResponseXML);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new UpdateShopperResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new UpdateShopperResponseData(sResponseXML, 
                                                       oRequestData, 
                                                       ex);
      }

      return oResponseData;
    }

    // **************************************************************** //

    #endregion

    // **************************************************************** //
  }
}

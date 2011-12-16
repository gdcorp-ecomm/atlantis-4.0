using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ShopperPinSet.Interface;

namespace Atlantis.Framework.ShopperPinSet.Impl
{
  public class ShopperPinSetRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      ShopperPinSetResponseData oResponseData = null;
      string sResponseXML = "";
      int isSet = 0;
      try
      {
        ShopperPinSetRequestData oShopperPinSetRequestData = (ShopperPinSetRequestData)oRequestData;
        using (WscgdShopper.WSCgdShopperService oSvc = new Atlantis.Framework.ShopperPinSet.Impl.WscgdShopper.WSCgdShopperService())
        {
          oSvc.Url = ((WsConfigElement)oConfig).WSURL;

          isSet = oSvc.IsShopperPINSet(oShopperPinSetRequestData.ShopperID);
        }
        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "ShopperPinSetRequest.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new ShopperPinSetResponseData(isSet.ToString(), exAtlantis);
        }
        else
          oResponseData = new ShopperPinSetResponseData(isSet.ToString());
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new ShopperPinSetResponseData(isSet.ToString(), exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new ShopperPinSetResponseData(isSet.ToString(), oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

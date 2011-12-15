using System;
using Atlantis.Framework.ClearBasket.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ClearBasket.Impl
{
    public class ClearBasketRequest:IRequest
    {
        #region IRequest Members

        public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
        {
            ClearBasketResponseData oResponseData = null;
            string sResponseXML = "";

            try
            {
                ClearBasketRequestData oClearBasketRequestData = (ClearBasketRequestData)oRequestData;
                sResponseXML = string.Empty;
                using (WSCgdBasket.WscgdBasketService oBasketWS = new WSCgdBasket.WscgdBasketService())
                {
                  oBasketWS.Url = ((WsConfigElement)oConfig).WSURL;
                  oBasketWS.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;
                  sResponseXML = oBasketWS.ClearByType(oClearBasketRequestData.ShopperID, oClearBasketRequestData.BasketType);
                }
                if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                         "ClearBasketRequest.RequestHandler",
                                                                         sResponseXML,
                                                                         oRequestData.ToXML());

                    oResponseData = new ClearBasketResponseData(sResponseXML, exAtlantis);
                }
                else
                    oResponseData = new ClearBasketResponseData(sResponseXML);
            }
            catch (AtlantisException exAtlantis)
            {
                oResponseData = new ClearBasketResponseData(sResponseXML, exAtlantis);
            }
            catch (Exception ex)
            {
                oResponseData = new ClearBasketResponseData(sResponseXML, oRequestData, ex);
            }

            return oResponseData;
        }

        #endregion
    }
}

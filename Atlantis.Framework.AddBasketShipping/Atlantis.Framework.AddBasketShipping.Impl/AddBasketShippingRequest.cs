using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AddBasketShipping.Interface;
namespace Atlantis.Framework.AddBasketShipping.Impl
{
  public class AddBasketShippingRequest:IRequest
  {
    #region IRequest Members

    
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AddBasketShippingRequestData addShippingRequest = (AddBasketShippingRequestData)oRequestData;
      AddBasketShippingResponseData  oResponseData = null;
      string requestXML = addShippingRequest.ToXML();
      string sResponseXML = string.Empty;
      try
      {
        WscgdBasket.WscgdBasketService oBasketWS = new WscgdBasket.WscgdBasketService();
        oBasketWS.Url = ((WsConfigElement)oConfig).WSURL;
        sResponseXML = oBasketWS.AddShippingToBasket(addShippingRequest.ShopperID, requestXML);
        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "AddBasketShipping.RequestHandler",
                                                               sResponseXML,
                                                               string.Empty);
          oResponseData = new AddBasketShippingResponseData(oRequestData, exAtlantis);
        }
        else
          oResponseData = new AddBasketShippingResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new AddBasketShippingResponseData(oRequestData, exAtlantis);
      }
      return oResponseData;
    }

    #endregion
  }
}

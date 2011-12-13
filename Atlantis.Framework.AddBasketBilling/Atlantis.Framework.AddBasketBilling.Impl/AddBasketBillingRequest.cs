using System;
using Atlantis.Framework.AddBasketBilling.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketBilling.Impl
{
  public class AddBasketBillingRequest:IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AddBasketBillingRequestData addBillingRequest = (AddBasketBillingRequestData)oRequestData;
      AddBasketBillingResponseData oResponseData = null;
      string requestXML = addBillingRequest.ToXML();
      string sResponseXML = string.Empty;

      try
      {
        WscgdBasket.WscgdBasketService wsBasket = new WscgdBasket.WscgdBasketService();
        wsBasket.Url = ((WsConfigElement)oConfig).WSURL;
        wsBasket.Timeout = (int)addBillingRequest.RequestTimeout.TotalMilliseconds;

        sResponseXML = wsBasket.AddBillingToBasket(addBillingRequest.ShopperID, requestXML);

        if (sResponseXML.IndexOf("<MESSAGE>Success</MESSAGE>", StringComparison.OrdinalIgnoreCase)==-1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "AddBasketBilling.RequestHandler",
                                                               sResponseXML,
                                                               string.Empty);
          oResponseData = new AddBasketBillingResponseData(oRequestData, exAtlantis);
        }
        else
        {
          oResponseData = new AddBasketBillingResponseData(sResponseXML);
        }

      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new AddBasketBillingResponseData(oRequestData, exAtlantis);
      }

      return oResponseData;
    }

    #endregion

  }
}

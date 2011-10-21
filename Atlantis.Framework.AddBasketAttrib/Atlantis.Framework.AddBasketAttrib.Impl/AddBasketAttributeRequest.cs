using System;
using Atlantis.Framework.AddBasketAttribute.Impl.BasketWS;
using Atlantis.Framework.AddBasketAttribute.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AddBasketAttribute.Impl
{
  public class AddBasketAttributeRequest:IRequest
  {
    #region IRequest Members
   
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AddBasketAttributeRequestData addBasketAttributeRequestData = (AddBasketAttributeRequestData)oRequestData;
      AddBasketAttributeResponseData  oResponseData;
      string requestXml = addBasketAttributeRequestData.ToXML();
      WscgdBasketService basketWebServiceClient = null;
      string responseXml;

      try
      {
        basketWebServiceClient = new WscgdBasketService();
        basketWebServiceClient.Url = ((WsConfigElement)oConfig).WSURL;
        basketWebServiceClient.Timeout = (int)addBasketAttributeRequestData.RequestTimeout.TotalMilliseconds;

        responseXml = basketWebServiceClient.ModifyBasketAttributesByType(addBasketAttributeRequestData.ShopperID,addBasketAttributeRequestData.BasketType,requestXml);
        if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "AddBasketAttribute.RequestHandler",
                                                               responseXml,
                                                               string.Empty);

          oResponseData = new AddBasketAttributeResponseData(oRequestData, exAtlantis);
        }
        else
        {
          oResponseData = new AddBasketAttributeResponseData(responseXml);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new AddBasketAttributeResponseData(oRequestData, exAtlantis);
      }
      finally
      {
        if(basketWebServiceClient != null)
        {
          basketWebServiceClient.Dispose();
        }
      }

      return oResponseData;
    }

    #endregion
  }
}

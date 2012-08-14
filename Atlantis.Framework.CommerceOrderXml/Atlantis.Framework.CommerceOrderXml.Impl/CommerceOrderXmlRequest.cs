using System;
using Atlantis.Framework.CommerceOrderXml.Impl.WsceCommerce;
using Atlantis.Framework.CommerceOrderXml.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.CommerceOrderXml.Impl
{
  public class CommerceOrderXmlRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CommerceOrderXmlResponseData responseData = null;

      try
      {
        CommerceOrderXmlRequestData commerceOrderRequestData = (CommerceOrderXmlRequestData)requestData;
        using(WsceCommerce.WsceCommerceService commerceOrderWS = new WsceCommerceService())
        {
          commerceOrderWS.Url = (((WsConfigElement)config).WSURL);
          commerceOrderWS.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          string responseXml = commerceOrderWS.GetOrderXML(commerceOrderRequestData.RecentOrderId, commerceOrderRequestData.ShopperID);

          responseData = new CommerceOrderXmlResponseData(responseXml);        
        }

      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new CommerceOrderXmlResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new CommerceOrderXmlResponseData(requestData, ex);
      }

      return responseData;
    }
  }
}

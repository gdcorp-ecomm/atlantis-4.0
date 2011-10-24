using System;
using Atlantis.Framework.EmailMobileTemplate.Impl.MessagingDeliveryWs;
using Atlantis.Framework.EmailMobileTemplate.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EmailMobileTemplate.Impl
{
  public class EmailMobileTemplateRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      EmailMobileTemplateResponseData responseData;
      string mobileTemplateHtml;

      try
      {
        string messageDeliveryWebServiceUrl = ((WsConfigElement)oConfig).WSURL;

        EmailMobileTemplateRequestData requestData = (EmailMobileTemplateRequestData) oRequestData;

        MessagingDeliveryWS messagingDeliveryWs = new MessagingDeliveryWS();
        messagingDeliveryWs.Url = messageDeliveryWebServiceUrl;
        messagingDeliveryWs.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

        mobileTemplateHtml = messagingDeliveryWs.GetMobileViewTemplate(requestData.ShopperID, 
                                                                       requestData.PrivateLabelId,
                                                                       requestData.IscCode,
                                                                       requestData.BounceBackEmailId, 
                                                                       requestData.MessageGuid,
                                                                       requestData.ShopperIdInLinkMatchesShopperCookie);

        responseData = new EmailMobileTemplateResponseData(mobileTemplateHtml);
      }
      catch (Exception ex)
      {
        responseData = new EmailMobileTemplateResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}

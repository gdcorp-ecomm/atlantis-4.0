using System;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAResend;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Impl
{
   public class DomainsRAAResendRequest : IRequest
  {
     public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
     {
       DomainsRAAResendResponseData responseData;
       var requestXml = string.Empty;

       try
       {
         var verifyRequestData = (DomainsRAAResendRequestData) requestData;
         var wsConfigElement = (WsConfigElement) config;

         var clientCertificate = wsConfigElement.GetClientCertificate();

         var domainsRAAServiceClient = ServiceReferenceHelper.GetWebServiceInstance(wsConfigElement.WSURL, verifyRequestData.RequestTimeout, clientCertificate);

         requestXml = verifyRequestData.GetRequestXml(wsConfigElement.GetConfigValue("AppName"));

         var responseXml = domainsRAAServiceClient.Resend(requestXml);

         responseData = DomainsRAAResendResponseData.FromData(responseXml);

       }
       catch (Exception ex)
       {
         var aex = new AtlantisException(requestData, "DomainsRAAResendResponseData", ex.Message, requestXml);
         responseData = DomainsRAAResendResponseData.FromData(aex);
       }

       return responseData;
     }
  }
}

using System;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAQueueVerify;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Impl
{
   public class DomainsRAAQueueVerifyRequest : IRequest
  {
     public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
     {
       DomainsRAAQueueVerifyResponseData responseData;
       var requestXml = string.Empty;

       try
       {
         var verifyRequestData = (DomainsRAAQueueVerifyRequestData) requestData;
         var wsConfigElement = (WsConfigElement) config;

         var clientCertificate = wsConfigElement.GetClientCertificate();

         var domainsRAAServiceClient = ServiceReferenceHelper.GetWebServiceInstance(wsConfigElement.WSURL, verifyRequestData.RequestTimeout, clientCertificate);

         requestXml = verifyRequestData.GetRequestXml(wsConfigElement.GetConfigValue("AppName"));

         var responseXml = domainsRAAServiceClient.QueueVerify(requestXml);

         responseData = DomainsRAAQueueVerifyResponseData.FromData(responseXml);

       }
       catch (Exception ex)
       {
         var aex = new AtlantisException(requestData, "DomainsRAAVerifyResponseData", ex.Message, requestXml);
         responseData = DomainsRAAQueueVerifyResponseData.FromData(aex);
       }

       return responseData;
     }
  }
}

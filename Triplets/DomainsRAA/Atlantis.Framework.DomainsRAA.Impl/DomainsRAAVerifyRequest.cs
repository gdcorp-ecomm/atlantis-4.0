using System;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAVerify;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Impl
{
   public class DomainsRAAVerifyRequest : IRequest
  {
     public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
     {
       DomainsRAAVerifyResponseData responseData;
       var requestXml = string.Empty;

       try
       {
         var verifyRequestData = (DomainsRAAVerifyRequestData) requestData;
         var wsConfigElement = (WsConfigElement) config;

         var clientCertificate = wsConfigElement.GetClientCertificate();

         var domainsRAAServiceClient = ServiceReferenceHelper.GetWebServiceInstance(wsConfigElement.WSURL, verifyRequestData.RequestTimeout, clientCertificate);

         requestXml = verifyRequestData.GetRequestXml(wsConfigElement.GetConfigValue("AppName"));

         var responseXml = domainsRAAServiceClient.QueueVerify(requestXml);

         responseData = DomainsRAAVerifyResponseData.FromData(responseXml);

       }
       catch (Exception ex)
       {
         var aex = new AtlantisException(requestData, "DomainsRAAVerifyResponseData", ex.Message, requestXml);
         responseData = DomainsRAAVerifyResponseData.FromData(aex);
       }

       return responseData;
     }
  }
}

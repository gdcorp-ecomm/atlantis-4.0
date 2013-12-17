using System;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAASetVerified;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Impl
{
  public class DomainsRAASetVerifiedRequest : IRequest
  {
     public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
     {
       DomainsRAASetVerifiedResponseData responseData;
       var requestXml = string.Empty;

       try
       {
         var verifyRequestData = (DomainsRAASetVerifiedRequestData) requestData;
         var wsConfigElement = (WsConfigElement) config;

         var clientCertificate = wsConfigElement.GetClientCertificate();

         var domainsRAAServiceClient = ServiceReferenceHelper.GetWebServiceInstance(wsConfigElement.WSURL, verifyRequestData.RequestTimeout, clientCertificate);

         requestXml = verifyRequestData.GetRequestXml(wsConfigElement.GetConfigValue("AppName"));

         var responseXml = domainsRAAServiceClient.SetVerified(requestXml);

         responseData = DomainsRAASetVerifiedResponseData.FromData(responseXml);

       }
       catch (Exception ex)
       {
         var aex = new AtlantisException(requestData, "DomainsRAAVerifyResponseData", ex.Message, requestXml);
         responseData = DomainsRAASetVerifiedResponseData.FromData(aex);
       }

       return responseData;
     }
  }
}

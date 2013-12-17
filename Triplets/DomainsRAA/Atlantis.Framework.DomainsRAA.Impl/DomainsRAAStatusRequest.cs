using System;
using Atlantis.Framework.DomainsRAA.Interface.DomainsRAAStatus;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainsRAA.Impl
{
  public class DomainsRAAStatusRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DomainsRAAStatusResponseData responseData;
      var requestXml = string.Empty;

      try
      {
        var statusRequestData = (DomainsRAAStatusRequestData)requestData;
        var wsConfigElement = (WsConfigElement)config;

        var clientCertificate = wsConfigElement.GetClientCertificate();

        var domainsRAAServiceClient = ServiceReferenceHelper.GetWebServiceInstance(wsConfigElement.WSURL, statusRequestData.RequestTimeout, clientCertificate);
        

        requestXml = statusRequestData.GetRequestXml(wsConfigElement.GetConfigValue("AppName"));

        var responseXml = domainsRAAServiceClient.GetStatus(requestXml);
        responseData = DomainsRAAStatusResponseData.FromData(responseXml);
      }
      catch (Exception ex)
      {
        var aex = new AtlantisException(requestData, "DomainsRAAStatusResponseData", ex.Message, requestXml);
        responseData = DomainsRAAStatusResponseData.FromData(aex);
      }

      return responseData;
    }
  }
}

using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ResourcePricing.Interface;

namespace Atlantis.Framework.ResourcePricing.Impl
{
  public class ResourcePricingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ResourcePricingResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var request = (ResourcePricingRequestData)requestData;

        using (var oSvc = new WSgdPricingData.Service1())
        {
          oSvc.Url = ((WsConfigElement)config).WSURL;
          oSvc.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

          X509Certificate2 clientCertificate = ((WsConfigElement)config).GetClientCertificate();

          if (clientCertificate == null)
            throw new AtlantisException(request, "ResourcePricing::RequestHandler", "No client certificate supplied to connect to the ResourcePricing webservice", null);

          //attach the client certificate (specified in atlantis.config)
          oSvc.ClientCertificates.Add(clientCertificate);

          responseXml = oSvc.GetResourcePricing(request.ResourceID, request.ResourceType, request.IDType, request.Currency, request.AddlUnifiedProductIDList);
          if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            var exAtlantis = new AtlantisException(request,
                                                   "ResourcePricingRequest.RequestHandler",
                                                   responseXml,
                                                   request.ToXML());

            responseData = new ResourcePricingResponseData(responseXml, exAtlantis);
          }
          else
          {
            responseData = new ResourcePricingResponseData(requestData, responseXml);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new ResourcePricingResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new ResourcePricingResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }

  }
}
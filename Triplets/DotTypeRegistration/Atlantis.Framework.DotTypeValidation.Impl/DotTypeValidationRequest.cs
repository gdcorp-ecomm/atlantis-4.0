using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.DotTypeValidation.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DotTypeValidation.Impl
{
  public class DotTypeValidationRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DotTypeValidationResponseData responseData;
      string responseXml = string.Empty;

      try
      {
        var dotTypeValidationRequestData = (DotTypeValidationRequestData)requestData;
        var wsConfigElement = ((WsConfigElement)config);

        using (var regAppTokenWebSvc = new RegAppTokenWebSvc.RegAppTokenWebSvc())
        {
          regAppTokenWebSvc.Url = ((WsConfigElement)config).WSURL;
          regAppTokenWebSvc.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

          if (!string.IsNullOrEmpty(wsConfigElement.GetConfigValue("ClientCertificateName")))
          {
            X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
            regAppTokenWebSvc.ClientCertificates.Add(clientCertificate);
          }

          responseXml = regAppTokenWebSvc.RegisterPIIDataExt(dotTypeValidationRequestData.ToXML());

          responseData = new DotTypeValidationResponseData(responseXml);
        }
      }
      catch (Exception ex)
      {
        responseData = new DotTypeValidationResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }
  }
}

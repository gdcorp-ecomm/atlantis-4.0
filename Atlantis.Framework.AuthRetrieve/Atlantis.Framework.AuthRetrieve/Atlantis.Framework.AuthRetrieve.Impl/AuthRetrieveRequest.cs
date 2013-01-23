using System;
using Atlantis.Framework.AuthRetrieve.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthRetrieve.Impl
{
  public class AuthRetrieveRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthRetrieveResponseData responseData;
      try
      {
        var authData = (AuthRetrieveRequestData)requestData;
        string responseXml;
        string errorXml;
        using (var svc = new RetrieveAuthSvc.RetrieveAuth())
        {
          svc.Url = ((WsConfigElement)config).WSURL;
          svc.Timeout = (int)authData.RequestTimeout.TotalMilliseconds;
          var configElement = (WsConfigElement)config;
          svc.ClientCertificates.Add(configElement.GetClientCertificate("CertificateName"));
          responseXml = svc.GetAuthData(authData.SpKey, authData.Artifact, out errorXml);
        }

        if (!string.IsNullOrEmpty(errorXml))
        {
          var exAtlantis = new AtlantisException(requestData, "AuthRetrieveRequest.RequestHandler", errorXml, requestData.ToXML());          
          responseData = new AuthRetrieveResponseData(exAtlantis);
        }
        else
        {
          responseData = new AuthRetrieveResponseData(responseXml);
        }
      }
      catch (Exception ex)
      {
        responseData = new AuthRetrieveResponseData(requestData, ex);
      }
      return responseData;
    }
  }
}

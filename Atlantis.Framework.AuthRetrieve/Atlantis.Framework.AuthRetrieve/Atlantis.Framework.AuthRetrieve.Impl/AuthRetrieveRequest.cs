using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.AuthRetrieve.Interface;

namespace Atlantis.Framework.AuthRetrieve.Impl
{
  public class AuthRetrieveRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      AuthRetrieveResponseData responseData = null;
      string responseXML = string.Empty;
      string errorXML = string.Empty;
      XmlDocument responseDoc = new XmlDocument();
      string requestDataXML = string.Empty;
      try
      {
        AuthRetrieveRequestData authData = (AuthRetrieveRequestData)requestData;
        using (RetrieveAuthSvc.RetrieveAuth oSvc = new RetrieveAuthSvc.RetrieveAuth())
        {
          oSvc.Url = ((WsConfigElement)config).WSURL;
          oSvc.Timeout = (int)authData.RequestTimeout.TotalMilliseconds;
          WsConfigElement configElement = (WsConfigElement)config;
          oSvc.ClientCertificates.Add(configElement.GetClientCertificate("CertificateName"));
          responseXML = oSvc.GetAuthData(authData.SPKey, authData.Artifact, out  errorXML);          
        }

        if (!string.IsNullOrEmpty(errorXML))
        {
          AtlantisException exAtlantis = new AtlantisException(requestData,
                                                               "AuthRetrieveRequest.RequestHandler",
                                                               errorXML,
                                                               requestData.ToXML());

          responseData = new AuthRetrieveResponseData(requestData, exAtlantis);
        }
        else
        {
          responseData = new AuthRetrieveResponseData(responseXML);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new AuthRetrieveResponseData(requestData, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new AuthRetrieveResponseData(requestData, ex);
      }

      return responseData;
    }


  }
}

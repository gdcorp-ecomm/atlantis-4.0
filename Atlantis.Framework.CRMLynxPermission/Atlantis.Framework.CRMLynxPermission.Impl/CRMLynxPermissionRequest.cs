using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Atlantis.Framework.CRMLynxPermission.Impl.CrmPermissionsService;
using Atlantis.Framework.CRMLynxPermission.Interface;
using Atlantis.Framework.Interface;
namespace Atlantis.Framework.CRMLynxPermission.Impl
{
  public class CRMLynxPermissionRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      CRMLynxPermissionResponseData responseData;

      try
      {
        var request = (CRMLynxPermissionRequestData)requestData;

        var crmPermissionsClient = GetWebServiceInstance(((WsConfigElement)config), request.RequestTimeout);
        var userHasAccess = crmPermissionsClient.UserHasAccess(request.ManagerUserId, request.PermissionKey);

        responseData = new CRMLynxPermissionResponseData(userHasAccess);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new CRMLynxPermissionResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new CRMLynxPermissionResponseData(requestData, ex);
      }

      return responseData;
    }

    private static PermissionsClient GetWebServiceInstance(WsConfigElement webServiceConfig, TimeSpan requestTimeout)
    {
      var basicBinding = new BasicHttpBinding(BasicHttpSecurityMode.Transport)
                {
                  SendTimeout = requestTimeout,
                  OpenTimeout = requestTimeout,
                  CloseTimeout = requestTimeout,
                  ReceiveTimeout = TimeSpan.FromMinutes(10),
                  AllowCookies = false,
                  BypassProxyOnLocal = false,
                  HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                  MessageEncoding = WSMessageEncoding.Text,
                  TextEncoding = System.Text.Encoding.UTF8,
                  UseDefaultWebProxy = true,
                  Name = "CertificateBinding",
                  Security =
                    {
                      Mode = BasicHttpSecurityMode.Transport,
                      Transport =
                        {
                          ClientCredentialType = HttpClientCredentialType.Certificate,
                          ProxyCredentialType = HttpProxyCredentialType.None,
                          Realm = string.Empty
                        },
                      Message = new BasicHttpMessageSecurity()
                                  {
                                    ClientCredentialType = BasicHttpMessageCredentialType.Certificate,
                                    AlgorithmSuite = SecurityAlgorithmSuite.Default
                                  }
                    }
                };

      var endpointAddressBuilder = new EndpointAddressBuilder
                                     {
                                       Identity = EndpointIdentity.CreateDnsIdentity("localhost"),
                                       Uri = new Uri(webServiceConfig.WSURL),
                                     };

      var pc = new PermissionsClient(basicBinding, endpointAddressBuilder.ToEndpointAddress());
      if (pc.ClientCredentials != null)
        pc.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, webServiceConfig.GetConfigValue("CertificateName"));

      return pc;
    }
  }
}

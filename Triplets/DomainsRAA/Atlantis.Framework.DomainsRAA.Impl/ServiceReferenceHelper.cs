
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using Atlantis.Framework.DomainsRAA.Impl.DomainsRAAService;

namespace Atlantis.Framework.DomainsRAA.Impl
{
  internal class ServiceReferenceHelper
  {
    public static IRegRaaVerifyWebSvc GetWebServiceInstance(string webServiceUrl, TimeSpan requestTimeout, X509Certificate2 clientCertificate)
    {
      var wsHttpBinding = new WSHttpBinding(SecurityMode.Transport)
      {
        ReceiveTimeout = requestTimeout, 
        SendTimeout = requestTimeout
      };

      wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

      var endpointAddress = new EndpointAddress(webServiceUrl);

      var raaServiceClient = new RegRaaVerifyWebSvcClient(wsHttpBinding, endpointAddress);

      if (raaServiceClient.ClientCredentials != null)
      {
        wsHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
        raaServiceClient.ClientCredentials.ClientCertificate.Certificate = clientCertificate;
      }

      return raaServiceClient;
    }
  }
}

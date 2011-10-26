using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivacyAppInsertEmailAddress.Impl.InsertUpdateWS;
using Atlantis.Framework.PrivacyAppInsertEmailAddress.Interface;

namespace Atlantis.Framework.PrivacyAppInsertEmailAddress.Impl
{
  public class PrivacyAppInsertEmailAddressRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData response;
      wscgdPrivacyAppService service = null;
      string pbstrErrorMessage = string.Empty;
      string emailHash = string.Empty;
      try
      {
        PrivacyAppInsertEmailAddressRequestData request = (PrivacyAppInsertEmailAddressRequestData)oRequestData;
        service = new wscgdPrivacyAppService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        AddClientCertificate(service, oConfig);
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        emailHash = service.InsertEmailAddress(request.EmailAddress, out pbstrErrorMessage);

        response = new PrivacyAppInsertEmailAddressResponseData(emailHash, pbstrErrorMessage);
      }
      catch (AtlantisException exAtlantis)
      {
        response = new PrivacyAppInsertEmailAddressResponseData(emailHash, pbstrErrorMessage, exAtlantis);
      }
      catch (Exception ex)
      {
        response = new PrivacyAppInsertEmailAddressResponseData(emailHash, pbstrErrorMessage, oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }
      
      return response;
    }

    private void AddClientCertificate(wscgdPrivacyAppService service, ConfigElement oConfig)
    {
      X509Certificate cert = GetCertificate(oConfig);
      if(cert != null)
      {
        service.ClientCertificates.Add(cert);
      }
    }

    private X509Certificate GetCertificate(ConfigElement oConfig)
    {
      X509Certificate cert = null;
      string certificateName = oConfig.GetConfigValue("CertificateName");
      if (!string.IsNullOrEmpty(certificateName))
      {
        X509Store certStore = new X509Store(StoreLocation.LocalMachine);
        certStore.Open(OpenFlags.ReadOnly);
        X509CertificateCollection certs = certStore.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true);
        if(certs.Count > 0)
        {
          cert = certs[0];
        }
      }
      return cert;
    }

    #endregion
  }
}

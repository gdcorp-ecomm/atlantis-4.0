using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivacyAppInsertUpdate.Impl.InsertUpdateWS;
using Atlantis.Framework.PrivacyAppInsertUpdate.Interface;

namespace Atlantis.Framework.PrivacyAppInsertUpdate.Impl
{
  public class PrivacyAppInsertUpdateRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData response = null;
      int result = 0;
      string pbstrOutput = string.Empty;

      try
      {
        PrivacyAppInsertUpdateRequestData request = (PrivacyAppInsertUpdateRequestData)oRequestData;
        using (wscgdPrivacyAppService service = new wscgdPrivacyAppService())
        {
          service.Url = ((WsConfigElement)oConfig).WSURL;
          AddClientCertificate(service, oConfig);
          service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          string bstrXML = request.PrivacyXML;
          result = service.InsertUpdate(bstrXML, out pbstrOutput);
        }

        response = new PrivacyAppInsertUpdateResponseData(result, pbstrOutput);
      }
      catch (Exception ex)
      {
        response = new PrivacyAppInsertUpdateResponseData(result, pbstrOutput, oRequestData, ex);
      }
      
      return response;
    }

    private void AddClientCertificate(wscgdPrivacyAppService service, ConfigElement oConfig)
    {
      X509Certificate cert = GetCertificate(oConfig);
      if(cert != null)
        service.ClientCertificates.Add(cert);
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
          cert = certs[0];
      }
      return cert;
    }

    #endregion
  }
}

using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PrivacyAppGetRecord.Impl.privacyWS;
using Atlantis.Framework.PrivacyAppGetRecord.Interface;

namespace Atlantis.Framework.PrivacyAppGetRecord.Impl
{
  public class PrivacyAppGetRecordRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;

      string responseXml = string.Empty;
      wscgdPrivacyAppService service = null;

      try
      {
        PrivacyAppGetRecordRequestData request = (PrivacyAppGetRecordRequestData)oRequestData;

        service = new wscgdPrivacyAppService();
        service.Url = ((WsConfigElement)oConfig).WSURL;
        AddClientCertificate(service, oConfig);
        service.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
        service.GetRecord(request.HashKey, request.ApplicationId, out responseXml);

        result = new PrivacyAppGetRecordResponseData(responseXml);

      }
      catch (AtlantisException exAtlantis)
      {
        result = new PrivacyAppGetRecordResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        result = new PrivacyAppGetRecordResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if (service != null)
        {
          service.Dispose();
        }
      }

      return result;
    }

    #endregion

    #region x509 Certificate Configuration
    private void AddClientCertificate(wscgdPrivacyAppService service, ConfigElement oConfig)
    {
      X509Certificate cert = GetCertificate(oConfig);
      if (cert != null)
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
        if (certs.Count > 0)
        {
          cert = certs[0];
        }
      }
      return cert;
    }

    #endregion
  }
}

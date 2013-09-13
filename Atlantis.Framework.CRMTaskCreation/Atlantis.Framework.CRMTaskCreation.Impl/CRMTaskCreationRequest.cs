using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Atlantis.Framework.Interface;
using Atlantis.Framework.CRMTaskCreation.Interface;

namespace Atlantis.Framework.CRMTaskCreation.Impl
{
  public class CRMTaskCreationRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;

      try
      {

        var taskRequest = (CRMTaskCreationRequestData)oRequestData;
        string response = string.Empty;
        if (!String.IsNullOrEmpty(taskRequest.ShopperID) &&
            !String.IsNullOrEmpty(taskRequest.OrderID))
        {

          using (var service = new CrmAppTaskCreation.TaskCreation()
                        {
                          Url = ((WsConfigElement)oConfig).WSURL,
                          Timeout =
                              (int)
                              taskRequest.RequestTimeout.TotalMilliseconds
                        })
          {
            AddClientCertificate(service, oConfig);
            response = service.CreateTask(taskRequest.ClientId, taskRequest.ToXml());
          }
          result = new CRMTaskCreationResponseData(response);
        }
        else
        {
          throw new ArgumentException("ShopperID or OrderID are incorrect.");
        }
      }

      catch (AtlantisException aex)
      {
        result = new CRMTaskCreationResponseData(aex);
      }
      catch (Exception ex)
      {
        result = new CRMTaskCreationResponseData(oRequestData, ex);
      }

      return result;
    }

    #endregion

    #region x509 Certificate Configuration
    private void AddClientCertificate(CrmAppTaskCreation.TaskCreation service, ConfigElement oConfig)
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

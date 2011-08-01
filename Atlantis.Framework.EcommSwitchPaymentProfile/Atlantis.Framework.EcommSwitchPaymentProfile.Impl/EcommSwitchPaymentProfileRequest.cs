using System;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Atlantis.Framework.EcommSwitchPaymentProfile.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommSwitchPaymentProfile.Impl
{
  public class EcommSwitchPaymentProfileRequest : IRequest
  {
    private const string FAIL = "FAIL";

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommSwitchPaymentProfileResponseData response;
      var switchPaymentProfileRequestData = (EcommSwitchPaymentProfileRequestData) requestData;

      try
      {
        using (var paymentProfileService = new PaymentProfileUpdate.QueuePPUpdateWebSvc())
        {
          paymentProfileService.Url = ((WsConfigElement) config).WSURL;
          paymentProfileService.Timeout = (int) switchPaymentProfileRequestData.RequestTimeout.TotalMilliseconds;

          X509Certificate clientCert = GetClientCertificate(config);
          if (clientCert != null)
          {
            paymentProfileService.ClientCertificates.Add(clientCert);
          }

          string shopperNote = string.Format("{0}:{1}-Profile Update REQUESTOR_IP:{2}",
                                             switchPaymentProfileRequestData.ResourceType,
                                             switchPaymentProfileRequestData.ResourceId,
                                             switchPaymentProfileRequestData.RequestorIp);
          string enteredBy = string.Format("Cust-{0}", switchPaymentProfileRequestData.ShopperID);

          var resultXml = paymentProfileService.SetResourceProfile(switchPaymentProfileRequestData.ShopperID,
                                                                switchPaymentProfileRequestData.ResourceId,
                                                                switchPaymentProfileRequestData.ResourceType,
                                                                switchPaymentProfileRequestData.IdType,
                                                                switchPaymentProfileRequestData.NewPaymentProfileId,
                                                                shopperNote, enteredBy);

          string result = ParseResultXml(resultXml);

          response = new EcommSwitchPaymentProfileResponseData(result);
        }
      }
      catch (AtlantisException aEx)
      {
        response = new EcommSwitchPaymentProfileResponseData(aEx, FAIL);
      }
      catch (Exception ex)
      {
        string data = string.Format("error-string-format");
        var aEx = new AtlantisException(requestData, "EcommSwitchPaymentProfileRequest.RequestHandler", ex.Message, data, ex);
        response = new EcommSwitchPaymentProfileResponseData(aEx, FAIL);
      }

      return response;
    }

    private static X509Certificate GetClientCertificate(ConfigElement oConfig)
    {
      X509Certificate clientCertificate = null;

      string certSubject = oConfig.GetConfigValue("CertificateName");
      if (!string.IsNullOrEmpty(certSubject))
      {
        var certStore = new X509Store(StoreLocation.LocalMachine);
        certStore.Open(OpenFlags.ReadOnly);

        X509CertificateCollection matchingCerts = certStore.Certificates.Find(X509FindType.FindBySubjectName, certSubject, true);
        if (matchingCerts.Count > 0)
        {
          clientCertificate = matchingCerts[0];
        }
      }

      return clientCertificate;
    }

    private static string ParseResultXml(string resultXml)
    {
      if (string.IsNullOrEmpty(resultXml))
      {
        return string.Empty;
      }

      var xml = new XmlDocument();
      xml.LoadXml(resultXml);

      var status = xml.SelectSingleNode("//Status");
      return status == null ? string.Empty : status.InnerText;
    }
  }
}

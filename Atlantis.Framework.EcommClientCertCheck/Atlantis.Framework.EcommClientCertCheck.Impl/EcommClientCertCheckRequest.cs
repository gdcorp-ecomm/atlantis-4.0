using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.EcommClientCertCheck.Impl.EcommClientCertCheckService;
using Atlantis.Framework.EcommClientCertCheck.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EcommClientCertCheck.Impl
{
  public class EcommClientCertCheckRequest : IRequest
  {
    private const string CERT_SUBJECT_FORMAT = "O={0}, OU={1}, CN={2}";

    /// <summary>
    /// The certificates are always entered in the database in the O={0}, OU={1}, CN={2} order.  We must make sure to pass the subject this way.
    /// </summary>
    /// <param name="certificateSubject"></param>
    /// <returns></returns>
    private static string GetCertSubjectInCorrectOrder(string certificateSubject)
    {
      string formattedSubject = string.Empty;
      
      if(!string.IsNullOrEmpty(certificateSubject))
      {
        string certItemO = string.Empty;
        string certItemOu = string.Empty;
        string certItemCn = string.Empty;

        string[] certSubjectItems = certificateSubject.Split(',');

        for(int i = 0; i < certSubjectItems.Length; i++)
        {
          string[] certSubjectItemPair = certSubjectItems[i].Split('=');
          if(certSubjectItemPair.Length == 2)
          {
            string key = certSubjectItemPair[0].Trim().ToUpper();
            switch (key)
            {
              case "O":
                certItemO = certSubjectItemPair[1].Trim();
                break;
              case "OU":
                certItemOu = certSubjectItemPair[1].Trim();
                break;
              case "CN":
                certItemCn = certSubjectItemPair[1].Trim();
                break;
            }
          }
        }

        formattedSubject = string.Format(CERT_SUBJECT_FORMAT, certItemO, certItemOu, certItemCn);
      }

      return formattedSubject;
    }

    private static X509Certificate2 GetClientCertificate(string certificateName)
    {
      X509Certificate2 clientCertificate = null;
      X509Store store = null;

      if (!string.IsNullOrEmpty(certificateName))
      {
        try
        {
          store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
          store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);

          foreach (X509Certificate2 certificate in store.Certificates)
          {
            if (certificate.FriendlyName.Equals(certificateName, StringComparison.CurrentCultureIgnoreCase))
            {
              clientCertificate = certificate;
              break;
            }
          }
        }
        finally
        {
          if (store != null)
          {
            store.Close();
          }
        }
      }

      return clientCertificate;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData;

      EcommClientCertCheckRequestData ecommClientCertCheckRequestData = (EcommClientCertCheckRequestData) requestData;
      WsConfigElement wsConfig = (WsConfigElement)config;

      Service clientCertCheckService = null;

      try
      {
        clientCertCheckService = new Service();

        if(!wsConfig.WSURL.ToLower().StartsWith("https:"))
        {
          throw new Exception("You must call EcommClientCertCheck over https");
        }

        clientCertCheckService.Url = wsConfig.WSURL;
        clientCertCheckService.Timeout = (int) ecommClientCertCheckRequestData.RequestTimeout.TotalMilliseconds;

        X509Certificate2 clientCertificate = GetClientCertificate(config.GetConfigValue("CertificateName"));
        if (clientCertificate == null)
        {
          throw new Exception("Certificate not found.");
        }

        clientCertCheckService.ClientCertificates.Add(clientCertificate);

        string formattedCertSubject = GetCertSubjectInCorrectOrder(ecommClientCertCheckRequestData.CertificateSubject);

        bool isAuthorized = clientCertCheckService.Check(formattedCertSubject,
                                                         ecommClientCertCheckRequestData.ApplicationName,
                                                         ecommClientCertCheckRequestData.MethodName,
                                                         IPAddress.Loopback.ToString());

        responseData = new EcommClientCertCheckResponeData(isAuthorized);
      }
      catch(Exception ex)
      {
        responseData = new EcommClientCertCheckResponeData(ecommClientCertCheckRequestData, ex);
      }
      finally
      {
        if(clientCertCheckService != null)
        {
          clientCertCheckService.Dispose();
        }
      }

      return responseData;
    }
  }
}

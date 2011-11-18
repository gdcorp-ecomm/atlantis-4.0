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
    private const string CACHE_KEY = "Atlantis.Framework.EcommClientCertCheck.EcommClientCertCheckResponseData|{0}|{1}|{2}|{3}";
    private const string CERT_SUBJECT_FORMAT = "O={0}, OU={1}, CN={2}";
    private const string APPLICATION_NAME_FORMAT = "{0}:{1}";

    private static readonly object _lockSync = new object();
    private static volatile EcommClientCertCheckCache _cacheInstance;

    private static EcommClientCertCheckCache CacheInstance
    {
      get
      {
        if (_cacheInstance == null)
        {
          lock(_lockSync)
          {
            if (_cacheInstance == null)
            {
              _cacheInstance = new EcommClientCertCheckCache();
            }
          }
        }
        return _cacheInstance;
      }
    }

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

    private EcommClientCertCheckResponeData GetRequestFromService(EcommClientCertCheckRequestData requestData, WsConfigElement wsConfig)
    {
      EcommClientCertCheckResponeData responseData;

      Service clientCertCheckService = null;

      try
      {
        clientCertCheckService = new Service();

        if (!wsConfig.WSURL.ToLower().StartsWith("https:"))
        {
          throw new Exception("You must call EcommClientCertCheck over https");
        }

        clientCertCheckService.Url = wsConfig.WSURL;
        clientCertCheckService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;

        X509Certificate2 clientCertificate = GetClientCertificate(wsConfig.GetConfigValue("CertificateName"));
        if (clientCertificate == null)
        {
          throw new Exception("Certificate not found.");
        }

        clientCertCheckService.ClientCertificates.Add(clientCertificate);

        string formattedCertSubject = GetCertSubjectInCorrectOrder(requestData.CertificateSubject);

        bool isAuthorized = clientCertCheckService.Check(formattedCertSubject,
                                                         string.Format(APPLICATION_NAME_FORMAT, requestData.ApplicationTeam, requestData.ApplicationName),
                                                         requestData.MethodName,
                                                         IPAddress.Loopback.ToString());

        responseData = new EcommClientCertCheckResponeData(isAuthorized);
      }
      catch (Exception ex)
      {
        responseData = new EcommClientCertCheckResponeData(requestData, ex);
      }
      finally
      {
        if (clientCertCheckService != null)
        {
          clientCertCheckService.Dispose();
        }
      }

      return responseData;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommClientCertCheckResponeData responseData;

      EcommClientCertCheckRequestData ecommClientCertCheckRequestData = (EcommClientCertCheckRequestData) requestData;
      WsConfigElement wsConfig = (WsConfigElement) config;

      string cacheKey = string.Format(CACHE_KEY, ecommClientCertCheckRequestData.ApplicationTeam, ecommClientCertCheckRequestData.ApplicationName, ecommClientCertCheckRequestData.MethodName, ecommClientCertCheckRequestData.CertificateSubject);
      bool isAuthorized;

      if (CacheInstance.TryGetValue(cacheKey, out isAuthorized))
      {
        responseData = new EcommClientCertCheckResponeData(isAuthorized);
      }
      else
      {
        responseData = GetRequestFromService(ecommClientCertCheckRequestData, wsConfig);
        if (responseData.IsSuccess)
        {
          CacheInstance.Insert(cacheKey, responseData.IsAuthorized);
        }
      }

      return responseData;
    }
  }
}

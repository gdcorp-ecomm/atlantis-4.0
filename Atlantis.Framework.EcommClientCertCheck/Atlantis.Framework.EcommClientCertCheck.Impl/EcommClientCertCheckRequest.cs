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

    private EcommClientCertCheckCache CacheInstance
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

    private EcommClientCertCheckRequestData RequestData { get; set; }

    private WsConfigElement WsConfig { get; set; }

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

    private bool GetRequestFromService(out bool isAuthorized, out string errorMessage)
    {
      bool success;
      isAuthorized = false;
      errorMessage = string.Empty;

      Service clientCertCheckService = null;

      try
      {
        clientCertCheckService = new Service();

        if (!WsConfig.WSURL.ToLower().StartsWith("https:"))
        {
          errorMessage = "You must call EcommClientCertCheck over https";
        }

        clientCertCheckService.Url = WsConfig.WSURL;
        clientCertCheckService.Timeout = (int)RequestData.RequestTimeout.TotalMilliseconds;

        X509Certificate2 clientCertificate = GetClientCertificate(WsConfig.GetConfigValue("CertificateName"));
        if (clientCertificate == null)
        {
          errorMessage = "Certificate not found.";
        }

        clientCertCheckService.ClientCertificates.Add(clientCertificate);

        string formattedCertSubject = GetCertSubjectInCorrectOrder(RequestData.CertificateSubject);

        isAuthorized = clientCertCheckService.Check(formattedCertSubject,
                                                    string.Format(APPLICATION_NAME_FORMAT, RequestData.ApplicationTeam, RequestData.ApplicationName),
                                                    RequestData.MethodName,
                                                    IPAddress.Loopback.ToString());

        success = true;
      }
      catch (Exception ex)
      {
        errorMessage = ex.Message;
        success = false;
      }
      finally
      {
        if (clientCertCheckService != null)
        {
          clientCertCheckService.Dispose();
        }
      }

      return success;
    }

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EcommClientCertCheckResponeData responseData;

      RequestData = (EcommClientCertCheckRequestData) requestData;
      WsConfig = (WsConfigElement) config;

      string cacheKey = string.Format(CACHE_KEY, RequestData.ApplicationTeam, RequestData.ApplicationName, RequestData.MethodName, RequestData.CertificateSubject);
      
      bool isAuthorized;
      string errorMessage;
      if (CacheInstance.TryGetValue(cacheKey, GetRequestFromService, out isAuthorized, out errorMessage))
      {
        responseData = new EcommClientCertCheckResponeData(isAuthorized);
      }
      else
      {
        responseData = new EcommClientCertCheckResponeData(RequestData, new Exception(errorMessage));
      }

      return responseData;
    }
  }
}

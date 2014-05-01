using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfilesList.Interface;

namespace Atlantis.Framework.PayeeProfilesList.Impl
{
  public class PayeeProfilesListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PayeeProfilesListResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));
        cert.Verify();

        PayeeProfilesListRequestData request = (PayeeProfilesListRequestData)requestData;

        using (var svc = new PayeeWS.Service())
        {
          svc.Url = ((WsConfigElement)config).WSURL;
          svc.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          svc.ClientCertificates.Add(cert);
          responseXml = svc.GetAccountsForShopper(request.ShopperID);
        }

        if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          string data = string.Format("Response XML: {0}", responseXml);
          AtlantisException exAtlantis = new AtlantisException("PayeeProfilesList::RequestHandler", 0, "Payee GetAccountsForShopper WebService Failed", data);

          responseData = new PayeeProfilesListResponseData(exAtlantis);
        }
        else
        {
          responseData = new PayeeProfilesListResponseData(responseXml);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new PayeeProfilesListResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new PayeeProfilesListResponseData(requestData, ex);
      }

      return responseData;
    }

    #region Find Certificate
    private static X509Certificate2 FindCertificate(StoreLocation location, StoreName name, X509FindType findType, string findValue)
    {
      X509Store store = new X509Store(name, location);

      try
      {
        // create and open store for read-only access
        store.Open(OpenFlags.ReadOnly);

        // search store
        X509Certificate2Collection col = store.Certificates.Find(findType, findValue, true);

        // return first certificate found
        return col[0];
      }
      // always close the store
      finally
      {
        store.Close();
      }
    }
    #endregion
  }
}

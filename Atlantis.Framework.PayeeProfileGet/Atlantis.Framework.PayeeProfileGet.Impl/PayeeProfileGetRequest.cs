using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfileGet.Interface;

namespace Atlantis.Framework.PayeeProfileGet.Impl
{
  public class PayeeProfileGetRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PayeeProfileGetResponseData responseData = null;
      string responseXML = string.Empty;

      try
      {
        X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));
        cert.Verify();

        PayeeProfileGetRequestData payeeProfileRequestData = (PayeeProfileGetRequestData)requestData;
        using (PayeeProfileWS.WSCgdCAPService svc = new PayeeProfileWS.WSCgdCAPService())
        {
          svc.Url = ((WsConfigElement)config).WSURL;
          svc.Timeout = (int)payeeProfileRequestData.RequestTimeout.TotalMilliseconds;
          svc.ClientCertificates.Add(cert);
          responseXML = svc.GetAccountDetail(requestData.ShopperID, payeeProfileRequestData.CapId);
        }

        if (responseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(requestData
            , "PayeeProfileRequestGet.RequestHandler"
            , responseXML
            , requestData.ToXML());

          responseData = new PayeeProfileGetResponseData(responseXML, exAtlantis);
        }
        else
        {
          responseData = new PayeeProfileGetResponseData(responseXML);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new PayeeProfileGetResponseData(responseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new PayeeProfileGetResponseData(responseXML, requestData, ex);
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

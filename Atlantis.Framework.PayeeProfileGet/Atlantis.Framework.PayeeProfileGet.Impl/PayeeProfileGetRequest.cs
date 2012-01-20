using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeProfileGet.Interface;

namespace Atlantis.Framework.PayeeProfileGet.Impl
{
  public class PayeeProfileGetRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      PayeeProfileGetResponseData oResponseData = null;
      string sResponseXML = string.Empty;

      try
      {
        X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, oConfig.GetConfigValue("CertificateName"));
        cert.Verify();

        PayeeProfileGetRequestData oGetPayeeProfileRequestData = (PayeeProfileGetRequestData)oRequestData;
        using (PayeeProfileWS.WSCgdCAPService oSvc = new PayeeProfileWS.WSCgdCAPService())
        {
          oSvc.Url = ((WsConfigElement)oConfig).WSURL;
          oSvc.Timeout = (int)oGetPayeeProfileRequestData.RequestTimeout.TotalMilliseconds;
          oSvc.ClientCertificates.Add(cert);
          sResponseXML = oSvc.GetAccountDetail(oRequestData.ShopperID, oGetPayeeProfileRequestData.ICAPID);
        }

        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "PayeeProfileRequestGet.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new PayeeProfileGetResponseData(sResponseXML, exAtlantis);
        }
        else
        {
          oResponseData = new PayeeProfileGetResponseData(sResponseXML);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new PayeeProfileGetResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new PayeeProfileGetResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

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
  }
}

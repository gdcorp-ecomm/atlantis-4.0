using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeAdd.Impl.PayeeWS;
using Atlantis.Framework.PayeeAdd.Interface;

namespace Atlantis.Framework.PayeeAdd.Impl
{
  public class PayeeAddRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      PayeeAddResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));
        cert.Verify();

        PayeeAddRequestData payeeRequest = (PayeeAddRequestData)requestData;
        using (WSCgdCAPService svc = new WSCgdCAPService())
        {
          svc.Url = ((WsConfigElement)config).WSURL;
          svc.Timeout = (int)payeeRequest.RequestTimeout.TotalMilliseconds;
          svc.ClientCertificates.Add(cert);
          responseXml = svc.AddAccount(payeeRequest.ToXML());
        }

        if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          string data = string.Format("Response XML: {0} | Request XML: {1}", responseXml, requestData.ToXML());
          AtlantisException exAtlantis = new AtlantisException(requestData
            , "PayeeAdd::RequestHandler"
            , "Payee Add WebService Failed"
            , data);

          responseData = new PayeeAddResponseData(exAtlantis);
        }
        else
        {
          responseData = new PayeeAddResponseData(responseXml);
        }
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new PayeeAddResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new PayeeAddResponseData(requestData, ex);
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

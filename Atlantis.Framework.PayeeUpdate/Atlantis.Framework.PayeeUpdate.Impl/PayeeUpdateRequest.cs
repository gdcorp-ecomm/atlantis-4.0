using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.Interface;
using Atlantis.Framework.PayeeUpdate.Impl.PayeeWS;
using Atlantis.Framework.PayeeUpdate.Interface;

namespace Atlantis.Framework.PayeeUpdate.Impl
{
  public class PayeeUpdateRequest : IRequest
  {
   public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
   {
      PayeeUpdateResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
         X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My, X509FindType.FindBySubjectName, config.GetConfigValue("CertificateName"));
        cert.Verify();

        PayeeUpdateRequestData payeeRequest = (PayeeUpdateRequestData)requestData;
        using (WSCgdCAPService svc = new WSCgdCAPService())
        {
          svc.Url = ((WsConfigElement)config).WSURL;
          svc.Timeout = (int)payeeRequest.RequestTimeout.TotalMilliseconds;
          svc.ClientCertificates.Add(cert);
          responseXml = svc.UpdateAccount(payeeRequest.ToXML());
        }

        if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          string data = string.Format("Response XML: {0} | Request XML: {1}", responseXml, requestData.ToXML());
          AtlantisException exAtlantis = new AtlantisException("PayeeUpdate::RequestHandler"
            , requestData.SourceURL
            , "0"
            , "Payee Update WebService Failed"
            , data
            , requestData.ShopperID
            , requestData.OrderID
            , string.Empty
            , requestData.Pathway
            , requestData.PageCount);

          responseData = new PayeeUpdateResponseData(exAtlantis);
        }
        else
        {
          responseData = new PayeeUpdateResponseData(responseXml);
        }              
      } 
    
      catch (AtlantisException exAtlantis)
      {
        responseData = new PayeeUpdateResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new PayeeUpdateResponseData(requestData, ex);
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

using System;
using System.Security.Cryptography.X509Certificates;

using Atlantis.Framework.ExpressCheckoutPurchase.Impl.InstantPurchase;
using Atlantis.Framework.ExpressCheckoutPurchase.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.ExpressCheckoutPurchase.Impl
{
  public class ExpressCheckoutPurchaseRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      ExpressCheckoutPurchaseResponseData responseData;

      try
      {
        string wsUrl = ((WsConfigElement)config).WSURL;
        if (!wsUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(requestData, "ExpressCheckoutPurchaseRequest::RequestHandler", "Express Checkout WS URL in atlantis.config must use https.", string.Empty);
        }

        var xcRequestData = (ExpressCheckoutPurchaseRequestData)requestData;        
        xcRequestData.WebServiceRequestXml.Element("instantPurchase").Attribute("requestingApp").SetValue(config.GetConfigValue("ApplicationName"));

        string xmlResponse;
        string orderXml;
        using (var expressCheckoutWs = new Service())
        {
          expressCheckoutWs.Url = wsUrl;
          expressCheckoutWs.Timeout = (int) xcRequestData.RequestTimeout.TotalMilliseconds;

          X509Certificate2 cert = FindCertificate(StoreLocation.LocalMachine, StoreName.My,
                                                  X509FindType.FindBySubjectName,
                                                  config.GetConfigValue("CertificateName"));

          cert.Verify();
          expressCheckoutWs.ClientCertificates.Add(cert);
          xmlResponse = expressCheckoutWs.InstantPurchase(xcRequestData.ShopperID, xcRequestData.ToXML(), out orderXml);
        }

        responseData = new ExpressCheckoutPurchaseResponseData(xmlResponse, orderXml);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new ExpressCheckoutPurchaseResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new ExpressCheckoutPurchaseResponseData(requestData, ex);
      }

      return responseData;
    }

    private static X509Certificate2 FindCertificate(StoreLocation location, StoreName name, X509FindType findType, string findValue)
    {
      var store = new X509Store(name, location);

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

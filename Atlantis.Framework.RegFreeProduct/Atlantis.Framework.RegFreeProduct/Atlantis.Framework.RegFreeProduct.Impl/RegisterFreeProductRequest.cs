
namespace Atlantis.Framework.EcommFreeProduct.Impl
{
  using System;
  using System.Linq;
  using System.Xml.Linq;
  using Atlantis.Framework.Interface;
  using Atlantis.Framework.EcommFreeProduct.Impl.ProcessFreeItems;
  using Atlantis.Framework.EcommFreeProduct.Interface;

  public class RegisterFreeProductRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      RegisterFreeProductResponseData returnValue = new RegisterFreeProductResponseData(requestData.ToXML());

      try
      {
        WsConfigElement wsConfig = config as WsConfigElement;
        if (!ReferenceEquals(null, wsConfig))
        {
          using (Service svc = new ProcessFreeItems.Service())
          {
            svc.Url = wsConfig.WSURL;
            svc.Timeout = Convert.ToInt32(requestData.RequestTimeout.TotalMilliseconds);
            svc.ClientCertificates.Add(wsConfig.GetClientCertificate("CertificateName"));
            returnValue = new RegisterFreeProductResponseData(svc.ProcessFreeItems(requestData.ShopperID, requestData.ToXML()));
          }
          if (XDocument.Parse(requestData.ToXML()).Elements("error").Any())
          {
            throw new AtlantisException(requestData, "RegisterFreeProductRequest.RequestHandler", returnValue.ToXML(), requestData.ToXML());
          }
        }
      } 
      catch (AtlantisException exAtlantis)
      {
        returnValue = new RegisterFreeProductResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        returnValue = new RegisterFreeProductResponseData(requestData, ex);
      }
       
      return returnValue;
    }
  }
}

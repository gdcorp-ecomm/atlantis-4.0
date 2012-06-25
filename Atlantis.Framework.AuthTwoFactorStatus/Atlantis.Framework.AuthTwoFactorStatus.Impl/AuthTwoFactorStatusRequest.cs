using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.AuthTwoFactorStatus.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.AuthTwoFactorStatus.Impl
{
  public class AuthTwoFactorStatusRequest : IRequest
  {
    #region IRequest Members

    // **************************************************************** //

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AuthTwoFactorStatusResponseData oResponseData;
      string sResponseXml = string.Empty;

      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement) oConfig;
        AuthTwoFactorStatusRequestData requestData = (AuthTwoFactorStatusRequestData)oRequestData;

        X509Certificate2 clientCertificate = wsConfigElement.GetClientCertificate();
        if (clientCertificate == null)
        {
          throw new AtlantisException(requestData, "AuthTwoFactorStatus.RequestHandler", "Unable to find client certificate for web service call.", string.Empty);
        }

        using (WSCgdShopper.WSCgdShopperService shopperWs = new WSCgdShopper.WSCgdShopperService())
        {
          shopperWs.Url = wsConfigElement.WSURL;
          shopperWs.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;
          shopperWs.ClientCertificates.Add(clientCertificate);
          sResponseXml = shopperWs.TwoFactorStatus(requestData.ShopperID);
          if (sResponseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 sResponseXml, string.Empty);

            oResponseData = new AuthTwoFactorStatusResponseData(sResponseXml, exAtlantis);
          }
          else
          {
            oResponseData = new AuthTwoFactorStatusResponseData(sResponseXml);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new AuthTwoFactorStatusResponseData(sResponseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new AuthTwoFactorStatusResponseData(sResponseXml,
                                                       oRequestData,
                                                       ex);
      }

      return oResponseData;
    }

    // **************************************************************** //

    #endregion

  }
}

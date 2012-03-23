using System;
using System.Security.Cryptography.X509Certificates;
using Atlantis.Framework.AuthTwoFactorStatus.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.ServiceHelper;

namespace Atlantis.Framework.AuthTwoFactorStatus.Impl
{
  public class AuthTwoFactorStatusRequest : IRequest
  {
    #region IRequest Members

    // **************************************************************** //

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      AuthTwoFactorStatusResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        AuthTwoFactorStatusRequestData requestData = (AuthTwoFactorStatusRequestData)oRequestData;

        X509Certificate2 cert = ClientCertHelper.GetClientCertificate(oConfig);
        cert.Verify();

        using (WSCgdShopper.WSCgdShopperService shopperWS = new WSCgdShopper.WSCgdShopperService())
        {
          shopperWS.Url = ((WsConfigElement)oConfig).WSURL;
          shopperWS.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;
          shopperWS.ClientCertificates.Add(cert);
          sResponseXML = shopperWS.TwoFactorStatus(requestData.ShopperID);
          if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 sResponseXML, string.Empty);

            oResponseData = new AuthTwoFactorStatusResponseData(sResponseXML, exAtlantis);
          }
          else
          {
            oResponseData = new AuthTwoFactorStatusResponseData(sResponseXML);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new AuthTwoFactorStatusResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new AuthTwoFactorStatusResponseData(sResponseXML,
                                                       oRequestData,
                                                       ex);
      }

      return oResponseData;
    }

    // **************************************************************** //

    #endregion

  }
}

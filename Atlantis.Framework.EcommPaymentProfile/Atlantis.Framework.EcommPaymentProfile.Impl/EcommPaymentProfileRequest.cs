using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.EcommPaymentProfile.Interface;

namespace Atlantis.Framework.EcommPaymentProfile.Impl
{
  public class EcommPaymentProfileRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      EcommPaymentProfileResponseData oResponseData;
      string responseXml = string.Empty;

      try
      {
        var oEcommPaymentProfileRequestData = (EcommPaymentProfileRequestData)oRequestData;

        using (var oSvc = new WsgdCPPSvc.PPWebSvcService())
        {
          oSvc.Url = ((WsConfigElement) oConfig).WSURL;
          responseXml = oSvc.GetInfoByProfileID(string.Empty, oEcommPaymentProfileRequestData.ProfileId, oRequestData.ShopperID);
          if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            var exAtlantis = new AtlantisException(oRequestData,
                                                   "EcommPaymentProfileRequest.RequestHandler",
                                                   responseXml,
                                                   oRequestData.ToXML());

            oResponseData = new EcommPaymentProfileResponseData(responseXml, exAtlantis);
          }
          else
          {
            oResponseData = new EcommPaymentProfileResponseData(oRequestData, responseXml);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new EcommPaymentProfileResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new EcommPaymentProfileResponseData(responseXml, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

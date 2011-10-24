using System;
using Atlantis.Framework.GetPaymentProfiles.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.GetPaymentProfiles.Impl
{
  public class GetPaymentProfilesRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      GetPaymentProfilesResponseData oResponseData = null;
      string sResponseXML = "";

      try
      {
        GetPaymentProfilesRequestData oGetPaymentProfilesRequestData = (GetPaymentProfilesRequestData)oRequestData;
        WsgdCPPSvc.PPWebSvcService oSvc = new Atlantis.Framework.GetPaymentProfiles.Impl.WsgdCPPSvc.PPWebSvcService();
        oSvc.Url = ((WsConfigElement)oConfig).WSURL;
        sResponseXML = string.Empty;
        sResponseXML = oSvc.GetInfoByShopperID(string.Empty, oRequestData.ShopperID);
        if (sResponseXML.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
        {
          AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                               "GetPaymentProfilesRequest.RequestHandler",
                                                               sResponseXML,
                                                               oRequestData.ToXML());

          oResponseData = new GetPaymentProfilesResponseData(sResponseXML, exAtlantis);
        }
        else
          oResponseData = new GetPaymentProfilesResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new GetPaymentProfilesResponseData(sResponseXML, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new GetPaymentProfilesResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

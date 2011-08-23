using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.SearchShoppers.Interface;

namespace Atlantis.Framework.SearchShoppers.Impl
{
  public class SearchShoppersRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      SearchShoppersResponseData oResponseData = null;
      string responseXml = String.Empty;

      try
      {
        SearchShoppersRequestData oSearchRequestData = (SearchShoppersRequestData)oRequestData;
        using (WSCgdShopper.WSCgdShopperService shopperWS = new WSCgdShopper.WSCgdShopperService())
        {
          shopperWS.Url = ((WsConfigElement)oConfig).WSURL;
          shopperWS.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;
          responseXml = shopperWS.SearchShoppers(oSearchRequestData.ToXML());

          if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
          {
            AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                 "ShopperRequest.RequestHandler",
                                                                 responseXml,
                                                                 oRequestData.ToXML());

            oResponseData = new SearchShoppersResponseData(responseXml, exAtlantis);
          }
          else
            oResponseData = new SearchShoppersResponseData(responseXml);
        }
      }
      catch (Exception ex)
      {
        oResponseData = new SearchShoppersResponseData(responseXml, 
                                                       oRequestData, 
                                                       ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

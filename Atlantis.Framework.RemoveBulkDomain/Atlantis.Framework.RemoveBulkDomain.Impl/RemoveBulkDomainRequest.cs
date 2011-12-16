using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RemoveBulkdomain.Interface;

namespace Atlantis.Framework.RemoveBulkdomain.Impl
{
  public class RemoveBulkDomainRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      RemoveBulkDomainResponseData responseData;
      string responseXml = string.Empty;
      WSCgdBasket.WscgdBasketService basketService = null;

      try
      {
        RemoveBulkDomainRequestData oRemoveDomainRequestData = (RemoveBulkDomainRequestData)oRequestData;
        using (basketService = new WSCgdBasket.WscgdBasketService())
        {
          basketService.Url = ((WsConfigElement)oConfig).WSURL;

          basketService.Timeout = Convert.ToInt32(oRemoveDomainRequestData.RequestTimeout.TotalMilliseconds);
          //Validate Row ID
          if (oRemoveDomainRequestData.RowID != -1 && oRemoveDomainRequestData.FullDomainList() != string.Empty)
          {
            responseXml = basketService.RemoveDomainsByItem(oRemoveDomainRequestData.ShopperID,
                                                oRemoveDomainRequestData.RowID,
                                                oRemoveDomainRequestData.FullDomainList());
            if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
            {
              AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                   "RemoveBulkDomainRequest.RequestHandler",
                                                                   responseXml,
                                                                   oRequestData.ToXML());

              responseData = new RemoveBulkDomainResponseData(responseXml, exAtlantis);
            }
            else
            {
              responseData = new RemoveBulkDomainResponseData(responseXml);
            }
          }
          else if (oRemoveDomainRequestData.RowID != -1 && oRemoveDomainRequestData.DomainList() != string.Empty)
          {
            responseXml = basketService.RemoveBulkDomains(oRemoveDomainRequestData.ShopperID,
                                                oRemoveDomainRequestData.RowID.ToString(),
                                                oRemoveDomainRequestData.DomainList());

            if (responseXml.IndexOf("<error>", StringComparison.OrdinalIgnoreCase) > -1)
            {
              AtlantisException exAtlantis = new AtlantisException(oRequestData,
                                                                   "RemoveBulkDomainRequest.RequestHandler",
                                                                   responseXml,
                                                                   oRequestData.ToXML());

              responseData = new RemoveBulkDomainResponseData(responseXml, exAtlantis);
            }
            else
            {
              responseData = new RemoveBulkDomainResponseData(responseXml);
            }
          }
          else
          {
            ArgumentException exArguments = new ArgumentException("You need a valid rowID and a domainlist");
            responseData = new RemoveBulkDomainResponseData(string.Empty, oRequestData, exArguments);
          }
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RemoveBulkDomainResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RemoveBulkDomainResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}

using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.RegCheckCaContacts.Interface;

namespace Atlantis.Framework.RegCheckCaContacts.Impl
{
  public class RegCheckCaContactsRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData = null;
      string responseXML = string.Empty;

      RegCaDataWebSvc.RegCaDataWebSvc regCaDataWebService = null;
      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement)oConfig;
        regCaDataWebService = new RegCaDataWebSvc.RegCaDataWebSvc();
        regCaDataWebService.Url = wsConfigElement.WSURL;
        regCaDataWebService.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);
        string inputXml = oRequestData.ToXML();
        responseXML = regCaDataWebService.ValidateDomainAndContactsCA(inputXml);
        responseData = new RegCheckCaContactsResponseData(responseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegCheckCaContactsResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegCheckCaContactsResponseData(responseXML, oRequestData, ex);
      }
      finally
      {
        if (regCaDataWebService != null)
        {
          regCaDataWebService.Dispose();
        }
      }

      return responseData;
    }

    #endregion
  }
}

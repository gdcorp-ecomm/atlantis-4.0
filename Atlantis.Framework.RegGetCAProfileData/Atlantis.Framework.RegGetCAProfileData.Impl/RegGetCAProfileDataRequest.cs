using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.RegGetCAProfileData.Interface;

namespace Atlantis.Framework.RegGetCAProfileData.Impl
{
  public class RegGetCAProfileDataRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData = null;
      string responseXML = string.Empty;

      RegCaDataWebService.RegCaDataWebSvc regCaDataWebService = null;
      try
      {
        WsConfigElement wsConfigElement = (WsConfigElement)oConfig;
        regCaDataWebService = new RegCaDataWebService.RegCaDataWebSvc();
        regCaDataWebService.Url = wsConfigElement.WSURL;
        regCaDataWebService.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

        string inputXml = oRequestData.ToXML();
        responseXML = regCaDataWebService.GetProfileDataFromShopperId(inputXml);
        responseData = new RegGetCAProfileDataResponseData(responseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegGetCAProfileDataResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegGetCAProfileDataResponseData(responseXML, oRequestData, ex);
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

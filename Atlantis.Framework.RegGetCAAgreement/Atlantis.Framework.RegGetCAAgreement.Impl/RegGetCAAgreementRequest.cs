using System;

using Atlantis.Framework.Interface;
using Atlantis.Framework.RegGetCAAgreement.Interface;

namespace Atlantis.Framework.RegGetCAAgreement.Impl
{
  public class RegGetCAAgreementRequest : IRequest
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

        responseXML = regCaDataWebService.GetAgreement();
        if (!string.IsNullOrEmpty(responseXML))
        {
          responseData = new RegGetCAAgreementResponseData(responseXML);
        }
        else
        {
          throw new Exception("RegGetCAAgreementRequest returned empty string.");
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new RegGetCAAgreementResponseData(exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new RegGetCAAgreementResponseData(responseXML, oRequestData, ex);
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

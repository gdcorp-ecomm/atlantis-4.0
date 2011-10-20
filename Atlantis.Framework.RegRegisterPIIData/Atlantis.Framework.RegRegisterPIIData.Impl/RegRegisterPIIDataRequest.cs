using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegRegisterPIIData.Interface;
using Atlantis.Framework.RegRegisterPIIData.Impl.AppTokenWS;

namespace Atlantis.Framework.RegRegisterPIIData.Impl
{
  public class RegRegisterPIIDataRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result;
      string responseXml = string.Empty;

      RegAppTokenWebSvc appTokenService = null;

      try
      {
        string serviceUrl = ((WsConfigElement)oConfig).WSURL;
        if (!serviceUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
        {
          throw new AtlantisException(oRequestData, "RegRegisterPIIData.RequestHandler", "RegRegisterPIIData WS URL in atlantis.config must use https.", string.Empty);
        }

        RegRegisterPIIDataRequestData request = (RegRegisterPIIDataRequestData)oRequestData;
        appTokenService = new RegAppTokenWebSvc();
        appTokenService.Url = serviceUrl;
        appTokenService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

        responseXml = appTokenService.RegisterPIIData(request.ToXML());
        result = new RegRegisterPIIDataResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        result = new RegRegisterPIIDataResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        result = new RegRegisterPIIDataResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(appTokenService != null)
        {
          appTokenService.Dispose();
        }
      }

      return result;
    }

    #endregion
  }
}

using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.RegIsValidXN.Impl.RegIDNCheckerWS;
using Atlantis.Framework.RegIsValidXN.Interface;

namespace Atlantis.Framework.RegIsValidXN.Impl
{
  public class RegIsValidXNRequest : IRequest
  {

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData result = null;
      string responseXml = string.Empty;

      RegIDNCheckerWebSvc xnRequest = null;
      try
      {
        string serviceUrl = ((WsConfigElement)oConfig).WSURL;
        xnRequest = new RegIDNCheckerWebSvc();
        xnRequest.Url = serviceUrl;
        xnRequest.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);
        responseXml = xnRequest.IsValidXN(oRequestData.ToXML());
        result = new RegIsValidXNResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        result = new RegIsValidXNResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        result = new RegIsValidXNResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if (xnRequest != null)
        {
          xnRequest.Dispose();
        }
      }

      return result;
    }

    #endregion

  }
}

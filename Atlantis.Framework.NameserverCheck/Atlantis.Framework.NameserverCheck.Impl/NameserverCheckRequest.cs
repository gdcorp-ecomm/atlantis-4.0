using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.NameserverCheck.Impl.AvailCheckWS;
using Atlantis.Framework.NameserverCheck.Interface;

namespace Atlantis.Framework.NameserverCheck.Impl
{
  public class NameserverCheckRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData response = null;
      string responseXml = string.Empty;

      AvailCheckWebSvc availCheckService = null;
      try
      {
        availCheckService = new AvailCheckWebSvc();
        availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        availCheckService.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

        responseXml = availCheckService.Check(oRequestData.ToXML());
        if (responseXml == null)
        {
          throw new Exception("AvailCheck returned null response.");
        }

        response = new NameserverCheckResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        response = new NameserverCheckResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        response = new NameserverCheckResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if (availCheckService != null)
        {
          availCheckService.Dispose();
        }
      }

      return response;
    }

    #endregion
  }
}

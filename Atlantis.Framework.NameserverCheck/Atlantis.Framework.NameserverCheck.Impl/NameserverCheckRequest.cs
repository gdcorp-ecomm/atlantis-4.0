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

      try
      {
        NameserverCheckRequestData nameServerRequestData = (NameserverCheckRequestData)oRequestData;
        AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
        availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        availCheckService.Timeout = (int)nameServerRequestData.RequestTimeout.TotalMilliseconds;

        responseXml = availCheckService.Check(nameServerRequestData.ToXML());
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

      return response;
    }

    #endregion
  }
}

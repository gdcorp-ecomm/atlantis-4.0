using System;
using System.Net;
using Atlantis.Framework.DomainCheck.Impl.AvailCheckWS;
using Atlantis.Framework.DomainCheck.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainCheck.Impl
{
  public class DomainCheckRequest : IRequest
  {
    private const int _MAX_SERVICETIMEOUT_MILLISECONDS = 300000;// in miliseconds

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string sResponseXML = string.Empty;

      try
      {
        DomainCheckRequestData oDomainCheckRequestData = (DomainCheckRequestData)oRequestData;

        using (AvailCheckWebSvc availCheckService = new AvailCheckWebSvc())
        {
          availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
          if (oDomainCheckRequestData.RequestTimeout.TotalMilliseconds > _MAX_SERVICETIMEOUT_MILLISECONDS)
          {
            availCheckService.Timeout = _MAX_SERVICETIMEOUT_MILLISECONDS;
          }
          else
          {
            availCheckService.Timeout = (int)oDomainCheckRequestData.RequestTimeout.TotalMilliseconds;
          }

          string XML = oDomainCheckRequestData.ToXML();
          sResponseXML = availCheckService.Check(XML);
          if (sResponseXML == null)
          {
            throw new Exception("AvailCheck returned null response.");
          }
          oResponseData = new DomainCheckResponseData(sResponseXML);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainCheckResponseData(sResponseXML, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainCheckResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainCheckResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}

using System;
using System.Net;
using Atlantis.Framework.DomainCheckGA.Impl.AvailCheckWS;
using Atlantis.Framework.DomainCheckGA.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainCheckGA.Impl
{
  public class DomainCheckGARequest : IRequest
  {
    private const int _MAX_SERVICETIMEOUT_MILLISECONDS = 300000;// in miliseconds

    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData oResponseData = null;
      string sResponseXML = string.Empty;

      try
      {
        DomainCheckGARequestData oDomainCheckGARequestData = (DomainCheckGARequestData)oRequestData;

        AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
        availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
        if (oDomainCheckGARequestData.RequestTimeout.TotalMilliseconds > _MAX_SERVICETIMEOUT_MILLISECONDS)
        {
          availCheckService.Timeout = _MAX_SERVICETIMEOUT_MILLISECONDS;
        }
        else
        {
          availCheckService.Timeout = (int)oDomainCheckGARequestData.RequestTimeout.TotalMilliseconds;
        }

        sResponseXML = availCheckService.Check(oDomainCheckGARequestData.ToXML());
        if (sResponseXML == null)
        {
          throw new Exception("AvailCheck returned null response.");
        }

        oResponseData = new DomainCheckGAResponseData(sResponseXML);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainCheckGAResponseData(sResponseXML, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainCheckGAResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainCheckGAResponseData(sResponseXML, oRequestData, ex);
      }

      return oResponseData;
    }

    #endregion

  }
}

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

    public IResponseData RequestHandler(RequestData requestData, ConfigElement configElement)
    {
      IResponseData oResponseData = null;
      var responseXML = string.Empty;

      try
      {
        using (var availCheckService = new AvailCheckWebSvcClass())
        {
          availCheckService.Url = ((WsConfigElement)configElement).WSURL;

          if (requestData.RequestTimeout.TotalMilliseconds > _MAX_SERVICETIMEOUT_MILLISECONDS)
          {
            availCheckService.Timeout = _MAX_SERVICETIMEOUT_MILLISECONDS;
          }
          else
          {
            availCheckService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
          }

          var xml = requestData.ToXML();
          //responseXML = availCheckService.Check(xml);
          responseXML = availCheckService.FindCheck(xml);

          if (responseXML == null)
          {
            throw new Exception("AvailCheck returned null response.");
          }

          oResponseData = new DomainCheckResponseData(responseXML);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainCheckResponseData(responseXML, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainCheckResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainCheckResponseData(responseXML, requestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

using System;
using System.Net;
using Atlantis.Framework.DomainCheck.Impl.AvailCheckWS;
using Atlantis.Framework.DomainCheck.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainCheck.Impl
{
  public class DomainCheckAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
      availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
      availCheckService.Timeout = (int)oRequestData.RequestTimeout.TotalMilliseconds;

      AsyncState asyncState = new AsyncState(oRequestData, oConfig, availCheckService, oState);
      IAsyncResult asyncResult = availCheckService.BeginCheck(oRequestData.ToXML(), oCallback, asyncState);
      return asyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData oResponseData = null;
      string responseXml = string.Empty;
      AsyncState asyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        AvailCheckWebSvc availCheckService = (AvailCheckWebSvc)asyncState.Request;
        responseXml = availCheckService.EndCheck(oAsyncResult);
        if (responseXml == null)
        {
          throw new Exception("AvailCheck returned null response.");
        }

        oResponseData = new DomainCheckResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainCheckResponseData(responseXml, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainCheckResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainCheckResponseData(responseXml, asyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

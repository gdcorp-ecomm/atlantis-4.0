using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.NameserverCheck.Impl.AvailCheckWS;
using Atlantis.Framework.NameserverCheck.Interface;

namespace Atlantis.Framework.NameserverCheck.Impl
{
  class NameserverCheckAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
      availCheckService.Url = ((WsConfigElement)oConfig).WSURL;
      availCheckService.Timeout = (int)Math.Truncate(oRequestData.RequestTimeout.TotalMilliseconds);

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

        oResponseData = new NameserverCheckResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new NameserverCheckResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        oResponseData = new NameserverCheckResponseData(responseXml, asyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

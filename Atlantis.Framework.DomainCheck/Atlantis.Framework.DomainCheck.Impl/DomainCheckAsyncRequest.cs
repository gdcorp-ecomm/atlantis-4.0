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

    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement configElement, AsyncCallback callback, object state)
    {
      var availCheckService = new AvailCheckWebSvcClass();
      availCheckService.Url = ((WsConfigElement)configElement).WSURL;
      availCheckService.Timeout = (int)requestData.RequestTimeout.TotalMilliseconds;
      var asyncState = new AsyncState(requestData, configElement, availCheckService, state);
      var asyncResult = availCheckService.BeginFindCheck(requestData.ToXML(), callback, asyncState);
      return asyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult oAsyncResult)
    {
      IResponseData responseData = null;
      var responseXml = string.Empty;
      var asyncState = (AsyncState)oAsyncResult.AsyncState;

      try
      {
        var availCheckService = (AvailCheckWebSvcClass)asyncState.Request;
        responseXml = availCheckService.EndFindCheck(oAsyncResult);

        if (responseXml == null)
        {
          throw new Exception("AvailCheck returned null response.");
        }

        responseData = new DomainCheckResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DomainCheckResponseData(responseXml, exAtlantis);
      }
      catch (WebException exWeb)
      {
        responseData = new DomainCheckResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        responseData = new DomainCheckResponseData(responseXml, asyncState.RequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}

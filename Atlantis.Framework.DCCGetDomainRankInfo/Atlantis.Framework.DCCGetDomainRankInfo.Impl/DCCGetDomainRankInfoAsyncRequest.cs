using System;
using Atlantis.Framework.DCCGetDomainRankInfo.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetDomainRankInfo.Impl
{
  public class DCCGetDomainRankInfoAsyncRequest : IAsyncRequest
  {
    public IAsyncResult BeginHandleRequest(RequestData requestData, ConfigElement config, AsyncCallback callback, object state)
    {
      DCCGetDomainRankInfoRequestData request = (DCCGetDomainRankInfoRequestData)requestData;

      WebScoreBossWebSvc.WebScoreBossWebSvc domainRankInfoWS = new WebScoreBossWebSvc.WebScoreBossWebSvc();
      domainRankInfoWS.Url = ((WsConfigElement)config).WSURL;
      domainRankInfoWS.Timeout = (int)request.RequestTimeout.TotalMilliseconds;

      AsyncState asyncState = new AsyncState(requestData, config, domainRankInfoWS, state);
      IAsyncResult asyncResult = domainRankInfoWS.BeginGetRankInfoForDomainIds(request.ToXML(), callback, asyncState);

      return asyncResult;
    }

    public IResponseData EndHandleRequest(IAsyncResult asyncResult)
    {
      IResponseData responseData;
      string responseXml;
      AsyncState asyncState = (AsyncState)asyncResult.AsyncState;
      WebScoreBossWebSvc.WebScoreBossWebSvc domainRankService = null;

      try
      {
        domainRankService = (WebScoreBossWebSvc.WebScoreBossWebSvc)asyncState.Request;
        responseXml = domainRankService.EndGetRankInfoForDomainIds(asyncResult);

        if (!string.IsNullOrEmpty(responseXml) && responseXml.Contains("<success>1</success>"))
        {
          responseData = new DCCGetDomainRankInfoResponseData(responseXml);
        }
        else
        {
          DCCGetDomainRankInfoRequestData reqData = (DCCGetDomainRankInfoRequestData)asyncState.RequestData;
          responseData = new DCCGetDomainRankInfoResponseData(reqData, responseXml);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetDomainRankInfoResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new DCCGetDomainRankInfoResponseData(asyncState.RequestData, ex);
      }
      finally
      {
        if(domainRankService != null)
        {
          domainRankService.Dispose();
        }
      }

      return responseData;
    }
  }
}

using System;
using System.Net;
using Atlantis.Framework.DomainCheckGA.Impl.AvailCheckWS;
using Atlantis.Framework.DomainCheckGA.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainCheckGA.Impl
{
  public class DomainCheckGAAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      DomainCheckGARequestData oDomainCheckGARequestData = (DomainCheckGARequestData)oRequestData;
      AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
      availCheckService.Url = ((WsConfigElement)oConfig).WSURL;

      AsyncState asyncState = new AsyncState(oRequestData, oConfig, availCheckService, oState);
      IAsyncResult asyncResult = availCheckService.BeginCheck(oDomainCheckGARequestData.ToXML(), oCallback, asyncState);
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

        oResponseData = new DomainCheckGAResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainCheckGAResponseData(responseXml, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainCheckGAResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainCheckGAResponseData(responseXml, asyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

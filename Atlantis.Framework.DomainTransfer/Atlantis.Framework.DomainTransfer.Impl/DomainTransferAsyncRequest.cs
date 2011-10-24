using System;
using System.Net;
using Atlantis.Framework.DomainTransfer.Impl.AvailCheckWS;
using Atlantis.Framework.DomainTransfer.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DomainTransfer.Impl
{
  public class DomainTransferAsyncRequest : IAsyncRequest
  {
    #region IAsyncRequest Members

    public IAsyncResult BeginHandleRequest(RequestData oRequestData, ConfigElement oConfig, AsyncCallback oCallback, object oState)
    {
      DomainTransferRequestData oDomainCheckRequestData = (DomainTransferRequestData)oRequestData;
      AvailCheckWebSvc availCheckService = new AvailCheckWebSvc();
      availCheckService.Url = ((WsConfigElement)oConfig).WSURL;

      AsyncState asyncState = new AsyncState(oRequestData, oConfig, availCheckService, oState);
      IAsyncResult asyncResult = availCheckService.BeginCheck(oDomainCheckRequestData.ToXML(), oCallback, asyncState);
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

        oResponseData = new DomainTransferResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        oResponseData = new DomainTransferResponseData(responseXml, exAtlantis);
      }
      catch (WebException exWeb)
      {
        oResponseData = new DomainTransferResponseData(exWeb.Status);
      }
      catch (Exception ex)
      {
        oResponseData = new DomainTransferResponseData(responseXml, asyncState.RequestData, ex);
      }

      return oResponseData;
    }

    #endregion
  }
}

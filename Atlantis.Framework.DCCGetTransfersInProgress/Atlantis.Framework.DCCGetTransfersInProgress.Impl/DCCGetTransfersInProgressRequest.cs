using System;
using Atlantis.Framework.DCCGetTransfersInProgress.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Impl
{
  public class DCCGetTransfersInProgressRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCGetTransfersInProgressResponseData responseData;
      string responseXml = string.Empty;
      try
      {
        using(DomainStatusWebServiceSupport.RegCheckDomainStatusWebSvcService transferService = new DomainStatusWebServiceSupport.RegCheckDomainStatusWebSvcService())
        {
          DCCGetTransfersInProgressRequestData oRequest = (DCCGetTransfersInProgressRequestData)oRequestData;
          transferService.Url = ((WsConfigElement)oConfig).WSURL;
          transferService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          responseXml = transferService.GetDCCTransfersInProgress(oRequest.ToXML());
        }
        responseData = new DCCGetTransfersInProgressResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetTransfersInProgressResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetTransfersInProgressResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}

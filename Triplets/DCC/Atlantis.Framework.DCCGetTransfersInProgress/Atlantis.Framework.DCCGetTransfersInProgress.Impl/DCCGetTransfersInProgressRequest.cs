using System;
using Atlantis.Framework.DCCGetTransfersInProgress.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCGetTransfersInProgress.Impl
{
  public class DCCGetTransfersInProgressRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      DCCGetTransfersInProgressResponseData responseData;
      string responseXml = string.Empty;
      try
      {
        var request = (DCCGetTransfersInProgressRequestData)requestData;
        using(var transferService = new DomainStatusWebServiceSupport.RegCheckDomainStatusWebSvcService())
        {
          transferService.Url = ((WsConfigElement)config).WSURL;
          transferService.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          responseXml = transferService.GetDCCTransfersInProgress(request.ToXML());
        }
        responseData = new DCCGetTransfersInProgressResponseData(responseXml);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCGetTransfersInProgressResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCGetTransfersInProgressResponseData(responseXml, requestData, ex);
      }

      return responseData;
    }

    #endregion
  }
}

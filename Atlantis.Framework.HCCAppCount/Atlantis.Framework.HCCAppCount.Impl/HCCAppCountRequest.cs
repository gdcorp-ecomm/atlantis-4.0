using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using Atlantis.Framework.Interface;
using Atlantis.Framework.HCCAppCount.Interface;

namespace Atlantis.Framework.HCCAppCount.Impl
{
  public class HCCAppCountRequest : IRequest 
  {
    #region Implementation of IRequest
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      IResponseData responseData = null;
      HCCAppCountRequestData appCountRequest = (HCCAppCountRequestData)requestData;
     
      try
      {
        MetroWebservices.HCAppCountService webService = new MetroWebservices.HCAppCountService();
        webService.SoapVersion = SoapProtocolVersion.Soap12;
        webService.Url = ((WsConfigElement)config).WSURL;
        webService.Timeout = (int)appCountRequest.RequestTimeout.TotalMilliseconds;
        int appCount = webService.GetInstalledAppCount();
        responseData = new HCCAppCountResponseData(requestData, appCount);
      }
      catch (Exception ex)
      {
        responseData = new HCCAppCountResponseData(requestData, ex);
      }

      return responseData;
    }


    #endregion
  }
}

using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.MyaCheckDomainRenewalStatus.Interface;

namespace Atlantis.Framework.MyaCheckDomainRenewalStatus.Impl
{
  public class MyaCheckDomainRenewalStatusRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
    {
      MyaCheckDomainRenewalStatusResponseData responseData = null;
      string responseXml = string.Empty;
      MyaCheckDomainRenewalStatusRequestData verifyRequest = (MyaCheckDomainRenewalStatusRequestData)requestData;
      RegDCCVerifyService.RegDCCVerifyWS oDsWebVerify = new RegDCCVerifyService.RegDCCVerifyWS();
      oDsWebVerify.Url = ((WsConfigElement)oConfig).WSURL;
      oDsWebVerify.Timeout = (int)verifyRequest.RequestTimeout.TotalMilliseconds;

      try
      {
        responseXml = oDsWebVerify.VerifyDomainRenewal(verifyRequest.ActionXml, verifyRequest.DomainXml);
        responseData = new MyaCheckDomainRenewalStatusResponseData(responseXml, verifyRequest);
      }
      catch (AtlantisException aex)
      {
        responseData = new MyaCheckDomainRenewalStatusResponseData(responseXml, requestData, aex);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData,
                             "MyaCheckDomainRenewalStatusRequest RequestHandler()",
                             ex.Message,
                             requestData.ToXML());

        responseData = new MyaCheckDomainRenewalStatusResponseData(responseXml, requestData, aex);
      }

      return responseData;
    }

    #endregion
  }
}

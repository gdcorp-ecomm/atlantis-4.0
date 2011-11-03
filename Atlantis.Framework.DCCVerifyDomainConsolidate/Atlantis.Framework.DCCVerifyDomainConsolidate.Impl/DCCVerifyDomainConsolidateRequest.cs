using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCVerifyDomainConsolidate.Interface;


namespace Atlantis.Framework.DCCVerifyDomainConsolidate.Impl
{
  public class DCCVerifyDomainConsolidateRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData requestData, ConfigElement oConfig)
    {
      DCCVerifyDomainConsolidateResponseData responseData = null;      
      string responseXml = string.Empty;
      DCCVerifyDomainConsolidateRequestData verifyRequest = (DCCVerifyDomainConsolidateRequestData)requestData;
      RegDCCVerifyWS.RegDCCVerifyWS oDsWebVerify = new RegDCCVerifyWS.RegDCCVerifyWS();
      oDsWebVerify.Url = ((WsConfigElement)oConfig).WSURL;
      oDsWebVerify.Timeout = (int)verifyRequest.RequestTimeout.TotalMilliseconds;

      try
      {
        responseXml = oDsWebVerify.VerifyDomainConsolidate(verifyRequest.ActionXml, verifyRequest.DomainXml);
        responseData = new DCCVerifyDomainConsolidateResponseData(responseXml, verifyRequest);
      }
      catch (AtlantisException aex)
      {
        responseData = new DCCVerifyDomainConsolidateResponseData(responseXml, requestData, aex);
      }
      catch (Exception ex)
      {
        AtlantisException aex = new AtlantisException(requestData,
                             "DCCVerifyDomainConsolidateRequest RequestHandler()",
                             ex.Message,
                             requestData.ToXML());

        responseData = new DCCVerifyDomainConsolidateResponseData(responseXml, requestData, aex);
      }

      return responseData;
    }

    #endregion
  }
}

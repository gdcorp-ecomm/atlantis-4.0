using System;
using Atlantis.Framework.DCCForwardingUpdate.Impl.DsWebValidate;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCForwardingUpdate.Interface;

namespace Atlantis.Framework.DCCForwardingUpdate.Impl
{
  public class DCCForwardingUpdateRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCForwardingUpdateResponseData responseData;
      string responseXml = "";
      string validateResponseXml;

      RegDCCValidateWebSvc oDsWebValidate = new RegDCCValidateWebSvc();

      try
      {
        DCCForwardingUpdateRequestData oRequest = (DCCForwardingUpdateRequestData)oRequestData;

        string verifyAction;
        string verifyDomains;

        oRequest.XmlToVerify(out verifyAction, out verifyDomains);
        
        string sValidateUrl = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCValidateWebSvc/RegDCCValidateWebSvc.asmx");
        oDsWebValidate.Url = sValidateUrl;
        oDsWebValidate.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        validateResponseXml = oDsWebValidate.ValidateDomainForwardingUpdate(verifyAction, verifyDomains);

        //Success will be zero the 4 is for testing.
        if (validateResponseXml.Contains("<ACTIONRESULTS></ACTIONRESULTS>"))
        {
          string verifyResponseXml;

          DsWebVerify.RegDCCVerifyWebSvcService oDsWebVerify = new DsWebVerify.RegDCCVerifyWebSvcService();
          string sVerifyUrl = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll",
                                                                        "RegDCCVerifyWebSvc/RegDCCVerifyWebSvc.dll");

          oDsWebVerify.Url = sVerifyUrl;
          oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          verifyResponseXml = oDsWebVerify.VerifyDomainForwardingUpdate(verifyAction, verifyDomains);

          if (verifyResponseXml.Contains("ActionResultID=\"0\""))
          {
            using (DsWebSubmit.RegDCCRequestWebSvcService oDsWeb = new DsWebSubmit.RegDCCRequestWebSvcService())
            {
              oDsWeb.Url = ((WsConfigElement) oConfig).WSURL;
              oDsWeb.Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds;

              responseXml = oDsWeb.SubmitRequestStandard(oRequest.XmlToSubmit());
              responseData = new DCCForwardingUpdateResponseData(responseXml);
            }
          }
          else
          {
            responseData = new DCCForwardingUpdateResponseData(verifyResponseXml, oRequestData);
          }
        }
        else
        {
          responseData = new DCCForwardingUpdateResponseData(validateResponseXml, false);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCForwardingUpdateResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCForwardingUpdateResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        oDsWebValidate.Dispose();
      }

      return responseData;
    }
    #endregion
  }
}

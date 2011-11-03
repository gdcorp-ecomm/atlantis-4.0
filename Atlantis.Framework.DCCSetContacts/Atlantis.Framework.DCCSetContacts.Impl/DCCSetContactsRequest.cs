using System;
using Atlantis.Framework.DCCSetContacts.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetContacts.Impl
{
  public class DCCSetContactsRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCSetContactsResponseData responseData;
      string responseXml = string.Empty;
      string verifyResponseXml;
      DsWebValidate.RegDCCValidateWebSvc validationService = null;

      try
      {
        DCCSetContactsRequestData oRequest = (DCCSetContactsRequestData)oRequestData;

        string verifyAction;
        string verifyDomains;
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);

        validationService = new DsWebValidate.RegDCCValidateWebSvc();
        validationService.Url = oConfig.GetConfigValue("ValidateUrl");
        validationService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        string validateResponseXml = validationService.ValidateContactUpdate(verifyAction, verifyDomains);

        if (validateResponseXml.Contains("<ACTIONRESULTS></ACTIONRESULTS>"))
        {
          DsWebVerify.RegDCCVerifyWS oDsWebVerify = new DsWebVerify.RegDCCVerifyWS();
          oDsWebVerify.Url = oConfig.GetConfigValue("VerifyUrl");
          oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          verifyResponseXml = oDsWebVerify.VerifyContactUpdate(verifyAction, verifyDomains);

          if (verifyResponseXml.Contains("ActionResultID=\"0\""))
          {
            DsWebSubmit.RegDCCRequestWS oDsWeb = new DsWebSubmit.RegDCCRequestWS();
            oDsWeb.Url = ((WsConfigElement)oConfig).WSURL;
            oDsWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;

            responseXml = oDsWeb.SubmitRequestStandard(oRequest.XmlToSubmit());
            responseData = new DCCSetContactsResponseData(responseXml);
          }
          else
          {            
            responseData = new DCCSetContactsResponseData(verifyResponseXml,oRequestData);
          }
        }
        else
        {
          responseData = new DCCSetContactsResponseData(validateResponseXml, false);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCSetContactsResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCSetContactsResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(validationService != null)
        {
          validationService.Dispose();
        }
      }

      return responseData;
    }
  }
}

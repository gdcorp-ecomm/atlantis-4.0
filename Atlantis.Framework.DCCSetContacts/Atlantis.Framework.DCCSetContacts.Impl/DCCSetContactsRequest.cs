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
        string sValidateUrl = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCValidateWebSvc/RegDCCValidateWebSvc.asmx");
        validationService.Url = sValidateUrl;
        validationService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        string validateResponseXml = validationService.ValidateContactUpdate(verifyAction, verifyDomains);

        if (validateResponseXml.Contains("<ACTIONRESULTS></ACTIONRESULTS>"))
        {
          DsWebVerify.RegDCCVerifyWebSvcService oDsWebVerify = new DsWebVerify.RegDCCVerifyWebSvcService();
          oDsWebVerify.Url = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCVerifyWebSvc/RegDCCVerifyWebSvc.dll");
          oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          verifyResponseXml = oDsWebVerify.VerifyContactUpdate(verifyAction, verifyDomains);

          if (verifyResponseXml.Contains("ActionResultID=\"0\""))
          {
            DsWebSubmit.RegDCCRequestWebSvcService oDsWeb = new DsWebSubmit.RegDCCRequestWebSvcService();
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

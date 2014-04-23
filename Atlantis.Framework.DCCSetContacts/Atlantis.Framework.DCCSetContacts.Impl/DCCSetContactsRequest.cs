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
      DsWebValidate.RegDCCValidateWebSvc validationService = null;

      try
      {
        var oRequest = (DCCSetContactsRequestData)oRequestData;

        string verifyAction;
        string verifyDomains;
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);

        validationService = new DsWebValidate.RegDCCValidateWebSvc();
        validationService.Url = oConfig.GetConfigValue("ValidateUrl");
        validationService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        string validateResponseXml = validationService.ValidateContactUpdate(verifyAction, verifyDomains);

        if (validateResponseXml.Contains("<ACTIONRESULTS></ACTIONRESULTS>"))
        {
          var oDsWebVerify = new DsWebVerify.RegDCCVerifyWS
          {
            Url = oConfig.GetConfigValue("VerifyUrl"),
            Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds
          };
          string verifyResponseXml = oDsWebVerify.VerifyContactUpdate(verifyAction, verifyDomains);

          if (verifyResponseXml.Contains("ActionResultID=\"0\""))
          {
            var oDsWeb = new DsWebSubmit.RegDCCRequestWS
            {
              Url = ((WsConfigElement) oConfig).WSURL,
              Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds
            };

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
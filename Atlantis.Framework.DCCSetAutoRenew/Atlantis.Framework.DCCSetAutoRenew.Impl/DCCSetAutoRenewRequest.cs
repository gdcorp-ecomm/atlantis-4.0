using System;
using Atlantis.Framework.DCCSetAutoRenew.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Impl
{
  public class DCCSetAutoRenewRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCSetAutoRenewResponseData responseData;
      string responseXml = string.Empty;
      DsWebVerify.RegDCCVerifyWS regDccVerifyWebService = null;

      try
      {
        var oRequest = (DCCSetAutoRenewRequestData)oRequestData;
        
        string verifyAction;
        string verifyDomains;
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);
        regDccVerifyWebService = new DsWebVerify.RegDCCVerifyWS
        {
          Url = oConfig.GetConfigValue("VerifyUrl"),
          Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds
        };
        responseXml = regDccVerifyWebService.VerifyDomainSetAutoRenew(verifyAction, verifyDomains);

        if (responseXml.Contains("ActionResultID=\"0\""))
        {
          var oDsWeb = new DsWebSubmit.RegDCCRequestWS
          {
            Url = ((WsConfigElement) oConfig).WSURL,
            Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds
          };

          responseXml = oDsWeb.SubmitRequestStandard(oRequest.ToXML());
          responseData = new DCCSetAutoRenewResponseData(responseXml);
        }
        else
        {
          responseData = new DCCSetAutoRenewResponseData(responseXml, false);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCSetAutoRenewResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCSetAutoRenewResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(regDccVerifyWebService != null)
        {
          regDccVerifyWebService.Dispose();
        }
      }

      return responseData;
    }
  }
}

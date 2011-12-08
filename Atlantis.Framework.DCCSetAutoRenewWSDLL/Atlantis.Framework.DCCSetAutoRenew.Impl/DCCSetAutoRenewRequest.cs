using System;
using Atlantis.Framework.Interface;
using Atlantis.Framework.DCCSetAutoRenew.Interface;

namespace Atlantis.Framework.DCCSetAutoRenew.Impl
{
  public class DCCSetAutoRenewRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCSetAutoRenewResponseData responseData = null;
      string responseXml = string.Empty;

      try
      {
        DCCSetAutoRenewRequestData oRequest = (DCCSetAutoRenewRequestData)oRequestData;
        
        string verifyAction = "";
        string verifyDomains = "";
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);

        DsWebVerify.RegDCCVerifyWebSvcService oDsWebVerify = new DsWebVerify.RegDCCVerifyWebSvcService();
        oDsWebVerify.Url = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCVerifyWebSvc/RegDCCVerifyWebSvc.dll");
        oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = oDsWebVerify.VerifyDomainSetAutoRenew(verifyAction, verifyDomains);

        if (responseXml.Contains("ActionResultID=\"0\""))
        {
          DsWebSubmit.RegDCCRequestWebSvcService oDsWeb = new DsWebSubmit.RegDCCRequestWebSvcService();
          oDsWeb.Url = ((WsConfigElement)oConfig).WSURL;
          oDsWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;

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

      return responseData;
    }

    #endregion
  }
}

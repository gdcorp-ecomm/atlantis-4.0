using System;
using Atlantis.Framework.DCCSetLocking.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetLocking.Impl
{
  public class DCCSetLockingRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCSetLockingResponseData responseData;
      string responseXml = string.Empty;
      DsWebVerify.RegDCCVerifyWebSvcService verifyWebService = null;

      try
      {
        DCCSetLockingRequestData oRequest = (DCCSetLockingRequestData)oRequestData;
        
        string verifyAction;
        string verifyDomains;
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);

        verifyWebService = new DsWebVerify.RegDCCVerifyWebSvcService();
        verifyWebService.Url = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCVerifyWebSvc/RegDCCVerifyWebSvc.dll");
        verifyWebService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = verifyWebService.VerifyDomainSetLock(verifyAction, verifyDomains);

        if (responseXml.Contains("ActionResultID=\"0\""))
        {
          DsWebSubmit.RegDCCRequestWebSvcService oDsWeb = new DsWebSubmit.RegDCCRequestWebSvcService();
          oDsWeb.Url = ((WsConfigElement)oConfig).WSURL;
          oDsWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;

          responseXml = oDsWeb.SubmitRequestStandard(oRequest.ToXML());
          responseData = new DCCSetLockingResponseData(responseXml);
        }
        else if (responseXml.Contains("ActionResultID=\"52\""))
        {
          //Already in the state requested
          responseData = new DCCSetLockingResponseData("<success");
        }
        else
        {
          responseData = new DCCSetLockingResponseData(responseXml, false);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCSetLockingResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCSetLockingResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        if(verifyWebService != null)
        {
          verifyWebService.Dispose();
        }
      }

      return responseData;
    }
  }
}

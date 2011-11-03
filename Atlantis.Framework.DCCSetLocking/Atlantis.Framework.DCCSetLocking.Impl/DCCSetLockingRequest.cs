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
      DsWebVerify.RegDCCVerifyWS verifyWebService = null;

      try
      {
        DCCSetLockingRequestData oRequest = (DCCSetLockingRequestData)oRequestData;
        
        string verifyAction;
        string verifyDomains;
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);

        verifyWebService = new DsWebVerify.RegDCCVerifyWS();
        verifyWebService.Url = oConfig.GetConfigValue("VerifyUrl");
        verifyWebService.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        responseXml = verifyWebService.VerifyDomainSetLock(verifyAction, verifyDomains);

        if (responseXml.Contains("ActionResultID=\"0\""))
        {
          DsWebSubmit.RegDCCRequestWS oDsWeb = new DsWebSubmit.RegDCCRequestWS();
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

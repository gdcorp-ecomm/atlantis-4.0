using System;
using Atlantis.Framework.DCCForwardingDelete.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCForwardingDelete.Impl
{
  public class DCCForwardingDeleteRequest : IRequest
  {
    #region IRequest Members

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCForwardingDeleteResponseData responseData;
      string responseXml = string.Empty;
      string verifyResponseXml = string.Empty;

      DsWebVerify.RegDCCVerifyWS oDsWebVerify = new DsWebVerify.RegDCCVerifyWS();

      try
      {
        DCCForwardingDeleteRequestData oRequest = (DCCForwardingDeleteRequestData)oRequestData;

        string verifyAction = string.Empty;
        string verifyDomains = string.Empty;
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);

        oDsWebVerify.Url = oConfig.GetConfigValue("VerifyUrl"); ;
        oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        verifyResponseXml = oDsWebVerify.VerifyDomainForwardingDelete(verifyAction, verifyDomains);

        if (verifyResponseXml.Contains("ActionResultID=\"0\""))
        {
          using (DsWebSubmit.RegDCCRequestWS oDsWeb = new DsWebSubmit.RegDCCRequestWS())
          {
            oDsWeb.Url = ((WsConfigElement) oConfig).WSURL;
            oDsWeb.Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds;

            responseXml = oDsWeb.SubmitRequestStandard(oRequest.XmlToSubmit());
            responseData = new DCCForwardingDeleteResponseData(responseXml);
          }
        }
        else
        {
          responseData = new DCCForwardingDeleteResponseData(verifyResponseXml, oRequestData);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCForwardingDeleteResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCForwardingDeleteResponseData(responseXml, oRequestData, ex);
      }
      finally
      {
        oDsWebVerify.Dispose();
      }

      return responseData;
    }
    #endregion
  }
}

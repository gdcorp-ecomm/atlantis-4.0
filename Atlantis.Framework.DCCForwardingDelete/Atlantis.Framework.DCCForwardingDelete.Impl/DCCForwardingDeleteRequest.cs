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
      DCCForwardingDeleteResponseData responseData = null;
      string responseXml = string.Empty;
      string verifyResponseXml = string.Empty;

      DsWebVerify.RegDCCVerifyWebSvcService oDsWebVerify = new DsWebVerify.RegDCCVerifyWebSvcService();

      try
      {
        DCCForwardingDeleteRequestData oRequest = (DCCForwardingDeleteRequestData)oRequestData;

        string verifyAction = "";
        string verifyDomains = "";
        oRequest.XmlToVerify(out verifyAction, out verifyDomains);


        string sVerifyUrl = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCVerifyWebSvc/RegDCCVerifyWebSvc.dll");

        oDsWebVerify.Url = sVerifyUrl;
        oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        verifyResponseXml = oDsWebVerify.VerifyDomainForwardingDelete(verifyAction, verifyDomains);

        if (verifyResponseXml.Contains("ActionResultID=\"0\""))
        {
          using (DsWebSubmit.RegDCCRequestWebSvcService oDsWeb = new DsWebSubmit.RegDCCRequestWebSvcService())
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

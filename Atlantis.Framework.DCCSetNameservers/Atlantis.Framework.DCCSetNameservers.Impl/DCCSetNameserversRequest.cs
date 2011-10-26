using System;
using System.Linq;
using Atlantis.Framework.DCCSetNameservers.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetNameservers.Impl
{
  public class DCCSetNameserversRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCSetNameserversResponseData responseData;
      string responseXml = string.Empty;
      string verifyResponseXml = string.Empty;
      try
      {
        DCCSetNameserversRequestData oRequest = (DCCSetNameserversRequestData)oRequestData;

        if (oRequest.IsPremium)
        {
          AddCustomNameservers(oRequest.PremiumNameservers.ToArray(), oRequest);
        }
        else
        {
          switch (oRequest.RequestType)
          {
            case DCCSetNameserversRequestData.NameserverType.Forward:
              string[] forwardingNameservers = GetForwardingNameServers();
              AddCustomNameservers(forwardingNameservers, oRequest);
              break;
            case DCCSetNameserversRequestData.NameserverType.Host:
              string[] hostNameservers = GetHostNameservers(oRequest.PrivateLabelID);
              AddCustomNameservers(hostNameservers, oRequest);
              break;
            case DCCSetNameserversRequestData.NameserverType.Park:
              string[] parkNameservers = GetParkNameservers(oRequest.PrivateLabelID);
              AddCustomNameservers(parkNameservers, oRequest);
              break;
          }
        }

        string verifyAction;
        string verifyDomains;

        oRequest.XmlToVerify(out verifyAction, out verifyDomains);
        DsWebValidate.RegDCCValidateWebSvc oDsWebValidate = new DsWebValidate.RegDCCValidateWebSvc();
        string sValidateUrl = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCValidateWebSvc/RegDCCValidateWebSvc.asmx");
        oDsWebValidate.Url = sValidateUrl;
        oDsWebValidate.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
        string validateResponseXml = oDsWebValidate.ValidateNameserverUpdate(verifyAction, verifyDomains);

        if (validateResponseXml.Contains("ActionResultID=\"51\" Description=\"Redundant change - nameservers"))
        {
          responseData = new DCCSetNameserversResponseData("<success");
        }
        else if (validateResponseXml.Contains("<ACTIONRESULTS></ACTIONRESULTS>"))
        {

          DsWebVerify.RegDCCVerifyWebSvcService oDsWebVerify = new DsWebVerify.RegDCCVerifyWebSvcService();
          oDsWebVerify.Url = ((WsConfigElement)oConfig).WSURL.Replace("RegDCCRequestWebSvc/RegDCCRequestWebSvc.dll", "RegDCCVerifyWebSvc/RegDCCVerifyWebSvc.dll");
          oDsWebVerify.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;
          verifyResponseXml = oDsWebVerify.VerifyNameServerUpdate(verifyAction, verifyDomains);

          if (verifyResponseXml.Contains("ActionResultID=\"0\""))
          {
            DsWebSubmit.RegDCCRequestWebSvcService oDsWeb = new DsWebSubmit.RegDCCRequestWebSvcService();
            oDsWeb.Url = ((WsConfigElement)oConfig).WSURL;
            oDsWeb.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;

            responseXml = oDsWeb.SubmitRequestStandard(oRequest.ToXML());
            responseData = new DCCSetNameserversResponseData(responseXml);
          }
          else if (verifyResponseXml.Contains("ActionResultID=\"51\" Description=\"Redundant change - nameservers"))
          {
            responseData = new DCCSetNameserversResponseData("<success");
          }
          else
          {
            responseData = new DCCSetNameserversResponseData(verifyResponseXml, oRequestData, new Exception(verifyResponseXml));
          }
        }
        else
        {
          responseData = new DCCSetNameserversResponseData(validateResponseXml, false);
        }
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new DCCSetNameserversResponseData(responseXml, exAtlantis);
      }
      catch (Exception ex)
      {
        responseData = new DCCSetNameserversResponseData(responseXml, oRequestData, ex);
      }

      return responseData;
    }

    private static void AddCustomNameservers(string[] nameservers, DCCSetNameserversRequestData requestData)
    {
      foreach (string nameserver in nameservers)
      {
        if (nameserver.Length > 0)
        {
          requestData.AddCustomNameserver(nameserver);
        }
      }
    }

    private static string[] GetForwardingNameServers()
    {
      string[] forwadingNameservers = null;

      /*
          Call CacheServiceLib.GetPlData(iPrivateLabelId, eCategory)
          - iPrivateLabelId = 1592.
          - eCategory = PrivateLabelCategory.ParkNameServers = 6 
          The iPrivateLabelId = 1592 is VERY IMPORTANT.  It is needed especially for resellers that define their own parked nameservers.  Those nameservers will not allow forwarding to work.
          This returns a pipe (|) delimited string of nameservers for the specified PLID.
      */
      try
      {
        string nameserverValues = DataCache.DataCache.GetPLData(1592, 6);
        if(!string.IsNullOrEmpty(nameserverValues))
        {
          forwadingNameservers = nameserverValues.Split('|');
        }
      }
      catch
      {
        forwadingNameservers = new string[0];
      }

      return forwadingNameservers;
    }

    private static string[] GetHostNameservers(int privateLabelId)
    {
      string[] hostNameservers = null;

      /*
          Call CacheServiceLib.GetPlData(iPrivateLabelId, eCategory)
          - iPrivateLabelId is the PLID for the shopper.
          - eCategory = PrivateLabelCategory.NameServers = 7
      */

      try
      {
        string nameserverValues = DataCache.DataCache.GetPLData(privateLabelId, 7);
        if (!string.IsNullOrEmpty(nameserverValues))
        {
          hostNameservers = nameserverValues.Split('|');
        }
      }
      catch
      {
        hostNameservers = new string[0];
      }

      return hostNameservers;
    }

    private static string[] GetParkNameservers(int privateLabelId)
    {
      string[] parkNameservers = null;

      /*
        Call CacheServiceLib.GetPlData(iPrivateLabelId, eCategory)
        - iPrivateLabelId is the PLID for the shopper.
        - eCategory = PrivateLabelCategory.ParkNameServers = 6
      */

      try
      {
        string nameserverValues = DataCache.DataCache.GetPLData(privateLabelId, 6);
        if (!string.IsNullOrEmpty(nameserverValues))
        {
          parkNameservers = nameserverValues.Split('|');
        }
      }
      catch
      {
        parkNameservers = new string[0];
      }

      return parkNameservers;
    }
  }
}

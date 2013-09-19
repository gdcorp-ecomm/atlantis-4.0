using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Atlantis.Framework.DCCSetNameservers.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.DCCSetNameservers.Impl
{

  public class DCCSetNameserversRequest : IRequest
  {
    enum VerificationUpdateResultCodes
    {
      Unknown = -1,
      Success = 0,
      ProhibitsUpdate = 4,
      RedundantChange = 51

    }

    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      DCCSetNameserversResponseData responseData;
      string responseXml = string.Empty;
      string verifyResponseXml;
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
              string[] hostNameservers = GetHostNameservers(oRequest.PrivateLabelId);
              AddCustomNameservers(hostNameservers, oRequest);
              break;
            case DCCSetNameserversRequestData.NameserverType.Park:
              string[] parkNameservers = GetParkNameservers(oRequest.PrivateLabelId);
              AddCustomNameservers(parkNameservers, oRequest);
              break;
          }
        }

        
        var oDsWebValidate = new DsWebValidate.RegDCCValidateWS();
        oDsWebValidate.Url = oConfig.GetConfigValue("ValidateUrl"); 
        oDsWebValidate.Timeout = (int)oRequest.RequestTimeout.TotalMilliseconds;

        var validateRequestXml = oRequest.GetDomainNameserverValidateRequestXml();
        var validateResponseXml = oDsWebValidate.Validate(validateRequestXml); //ValidateNameserverUpdate(verifyAction, verifyDomains);

        if (NameserverIsValid(validateResponseXml))
        {
          string verifyAction;
          string verifyDomains;
          oRequest.XmlToVerify(out verifyAction, out verifyDomains);

          var oDsWebVerify = new DsWebVerify.RegDCCVerifyWS
            {
              Url = oConfig.GetConfigValue("VerifyUrl"),
              Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds
            };

          verifyResponseXml = oDsWebVerify.VerifyNameServerUpdate(verifyAction, verifyDomains);

          var updateResultCode = GetNameserverUpdateResultCode(verifyResponseXml);
          if (updateResultCode == VerificationUpdateResultCodes.Success)
          {
            var oDsWeb = new DsWebSubmit.RegDCCRequestWS
              {
                Url = ((WsConfigElement) oConfig).WSURL,
                Timeout = (int) oRequest.RequestTimeout.TotalMilliseconds
              };

            responseXml = oDsWeb.SubmitRequestStandard(oRequest.ToXML());
            responseData = new DCCSetNameserversResponseData(true, responseXml);
          }
          else if (updateResultCode == VerificationUpdateResultCodes.RedundantChange)
          {
            responseData = new DCCSetNameserversResponseData(true, verifyResponseXml);
          }
          else
          {
            responseData = new DCCSetNameserversResponseData(verifyResponseXml, oRequestData, new Exception(verifyResponseXml));
          }
        }
        else
        {
          responseData = new DCCSetNameserversResponseData(false, validateResponseXml);
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

    private static void AddCustomNameservers(IEnumerable<string> nameservers, DCCSetNameserversRequestData requestData)
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

    private bool NameserverIsValid(string dccValidationResponseXml)
    {
      var responseDoc = XDocument.Parse(dccValidationResponseXml).Root;
      var isSuccess = responseDoc != null && responseDoc.Attribute("result").Value == "success";

      return isSuccess;
    }

    private VerificationUpdateResultCodes GetNameserverUpdateResultCode(string dccVerifyUpdateReponseXml)
    {
      var updateResult = VerificationUpdateResultCodes.Unknown;

      var responseDoc = XDocument.Parse(dccVerifyUpdateReponseXml).Root;

      if (responseDoc != null && responseDoc.Element("ACTIONRESULTS") != null)
      {
        var actionResults = responseDoc.Element("ACTIONRESULTS");
        if (actionResults != null)
        {
          if (!actionResults.HasElements)
          {
            updateResult = VerificationUpdateResultCodes.Success;
          }
          else
          {
            var actionResult = actionResults.Element("ACTIONRESULT");
            if (actionResult != null)
            {
              int resultCode;
              if (int.TryParse(actionResult.Attribute("ActionResultID").Value, out resultCode))
              {
                switch (resultCode)
                {
                  case 0:
                    updateResult = VerificationUpdateResultCodes.Success;
                    break;
                  case 51:
                    updateResult = VerificationUpdateResultCodes.RedundantChange;
                    break;
                }
              }
            }
          }
        }
      }

      return updateResult;
    }
  }
}

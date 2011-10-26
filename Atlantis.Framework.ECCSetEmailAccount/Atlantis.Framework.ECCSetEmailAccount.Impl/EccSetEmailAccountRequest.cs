using System;
using System.Collections.Generic;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCSetEmailAccount.Impl.Json;
using Atlantis.Framework.ECCSetEmailAccount.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

namespace Atlantis.Framework.ECCSetEmailAccount.Impl
{
  public class EccSetEmailAccountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      EccSetEmailAccountRequestData plansRequest = (EccSetEmailAccountRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "setEmailAccount";

      string requestKey;
      string authName;
      string authToken;

      string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
      NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

      try
      {
        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        EccJsonSetEmailAccountRequest oJsonRequestBody = new EccJsonSetEmailAccountRequest();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId.ToString();
        oJsonRequestBody.AccountUid = new Guid(plansRequest.AccountUid);
        oJsonRequestBody.Subaccount = plansRequest.Subaccount;
        oJsonRequestBody.DiskSpace = plansRequest.DiskSpace;
        
        oJsonRequestBody.EmailAddress = plansRequest.EmailAddress;
        oJsonRequestBody.Password = plansRequest.Password;

        oJsonRequestBody.IsCatchAll = plansRequest.IsCatchAll;
        oJsonRequestBody.IsSpamFilterActive = plansRequest.HasSpamFilter;
        oJsonRequestBody.SmtpRelays = plansRequest.SmtpRelays;

        oJsonRequestBody.SendSingleResponse = plansRequest.SendSingleResponse;
        oJsonRequestBody.AutoResponderStartDate = plansRequest.AutoResponsderStart;
        oJsonRequestBody.AutoResponderEndDate = plansRequest.AutoResponsderEnd;
        oJsonRequestBody.AutoResponderFromAddress = plansRequest.AutoResponderFrom;
        oJsonRequestBody.AutoResponderSubject = plansRequest.AutoResponderSubject;
        oJsonRequestBody.AutoResponderMessage = plansRequest.AutoResponderMessage;
        
        int arStatus;
        int.TryParse(plansRequest.AutoResponderStatus, out arStatus);
        oJsonRequestBody.AutoResponderStatus = arStatus;
        
        List<string> cc = new List<string>();
        if (plansRequest.CCList != null)
        {
          cc.AddRange(plansRequest.CCList.Split(",".ToCharArray()));
        }
        oJsonRequestBody.CCList = cc;

        EccJsonRequest<EccJsonSetEmailAccountRequest> oRequest = new EccJsonRequest<EccJsonSetEmailAccountRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey, plansRequest.RequestTimeout);
        responseData = new EccSetEmailAccountResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new EccSetEmailAccountResponseData(oRequestData, ex);
      }

      return responseData;
    }
  }
}

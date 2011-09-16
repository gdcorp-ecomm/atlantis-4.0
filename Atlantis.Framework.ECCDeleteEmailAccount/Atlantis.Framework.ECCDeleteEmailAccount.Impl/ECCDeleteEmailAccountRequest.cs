using System;
using System.Collections.Generic;
using System.Xml;
using Atlantis.Framework.Ecc.Interface.Authentication;
using Atlantis.Framework.Ecc.Interface.jsonHelpers;
using Atlantis.Framework.ECCSetEmailAccount.Impl.Json;
using Atlantis.Framework.ECCSetEmailAccount.Interface;
using Atlantis.Framework.Interface;
using Atlantis.Framework.Nimitz;

/** * Delete an email address. This web service performs the first of 2 stages for "email removal". 
* The 2nd stage of removal is an automatic process that is not exposed through an API. * 
* Result Code series: 29000 * 
* @param RequestObj $aRequest Standard inbound request object * REQUIRED OBJECT PROPERTIES: 
* RequestObj->Id The Authentication ID used to identify the caller 
* RequestObj->Token The Token/Password associated with the supplied Authentication ID 
* RequestObj->Parameters->shopper The Shopper ID of the customer account being queried 
* RequestObj->Parameters->reseller The Reseller ID associated to the Shopper ID 
* RequestObj->Parameters->emailaddress The email address being removed * 
* CONTITIONAL OBJECT PROPERTIES: * None of the RequestObj properties are conditional for this web service * 
* OPTIONAL OBJECT PROPERTIES: 
* RequestObj->Parameters->subaccount The Shopper ID of the customer sub account being queried (overrides the shopper) * 
* IGNORED OBJECT PROPERTIES: 
* RequestObj->Return->PageNumber 
* RequestObj->Return->ResultsPerPage 
* RequestObj->Return->OrderBy 
* RequestObj->Return->SortOrder * 
* @return ResponseObj Standard outbound response object 
* ON SUCCESS: 
* ResponseObj->ResultCode = 0 
* ResponseObj->Message = blank 
* ResponseObj->Timer = # of seconds it took to derive the response 
* ResponseObj->Results = empty * 
* ON FAIL: 
* ResponseObj->ResultCode = Unique number indicating why the web service failed 
* ResponseObj->Message = Reason why web service failed 
* ResponseObj->Timer = # of seconds it took to derive the response * ResponseObj->Results = empty */


namespace Atlantis.Framework.ECCDeleteEmailAccount.Impl
{
  public class ECCDeleteEmailAccountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData oRequestData, ConfigElement oConfig)
    {
      IResponseData responseData;
      ECCDeleteEmailAccountRequestData plansRequest = (ECCDeleteEmailAccountRequestData)oRequestData;

      const int pageNumber = 1;
      const int resultsPerPage = 100000;
      const string requestMethod = "removeEmailAddress";      
      /*
       *  <Connect>
          <requestKey>tH15!zt433Cc@P1*</requestKey> 
          <UserID>GDMobile</UserID> 
          <Password>godaddymobile</Password> 
          </Connect>*/
  
      try
      {
        string requestKey;
        string authName;
        string authToken;

        string nimitzAuthXml = NetConnect.LookupConnectInfo(oConfig, ConnectLookupType.Xml);
        NimitzAuthHelper.GetConnectionCredentials(nimitzAuthXml, out requestKey, out authName, out authToken);

        WsConfigElement wsConfig = ((WsConfigElement)oConfig);

        EccJsonDeleteEmailAccountRequest oJsonRequestBody = new EccJsonDeleteEmailAccountRequest();
        oJsonRequestBody.ShopperId = plansRequest.ShopperID;
        oJsonRequestBody.ResellerId = plansRequest.ResellerId.ToString();        
        oJsonRequestBody.Subaccount = plansRequest.Subaccount;        
        oJsonRequestBody.EmailAddress = plansRequest.EmailAddress;
               
        EccJsonRequest<EccJsonDeleteEmailAccountRequest> oRequest = new EccJsonRequest<EccJsonDeleteEmailAccountRequest>();
        oRequest.Id = authName;
        oRequest.Token = authToken;
        oRequest.Return = new EccJsonPaging(pageNumber, resultsPerPage, String.Empty, string.Empty);
        oRequest.Parameters = oJsonRequestBody;

        string sRequest = oRequest.ToJson();

        string response = EccJsonRequestHandler.PostRequest(sRequest, wsConfig.WSURL, requestMethod, requestKey);
        responseData = new ECCDeleteEmailAccountResponseData(response);
      }
      catch (Exception ex)
      {
        responseData = new ECCDeleteEmailAccountResponseData(oRequestData, ex);
      }

      return responseData;
    }    
  }  
}

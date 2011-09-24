using System;
using System.Collections.Generic;
using Atlantis.Framework.HCC.Interface;
using Atlantis.Framework.HCCGetAccountList.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.HCCGetAccountList.Impl
{
  public class HCCGetAccountListRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {      
      IResponseData responseData = null;
      HCCGetAccountListRequestData apiRequestData = requestData as HCCGetAccountListRequestData;

      try
      {
        HCCAPIWebService.HCCAPIService HCCWebService = new HCCAPIWebService.HCCAPIService();
        HCCWebService.Url = ((WsConfigElement)config).WSURL;
        HCCWebService.Timeout = (int)apiRequestData.RequestTimeout.TotalMilliseconds;
        HCCAPIWebService.AccountListResponse apiResponse = HCCWebService.GetAccountList(apiRequestData.ShopperID);
        
        if (apiResponse != null && apiResponse.AccountList != null)
        {
          responseData = new HCCGetAccountListResponseData(GetHCCAccountResponse(apiResponse));
        }
        else
        {
          AtlantisException ex = new AtlantisException(requestData,
            "HCCGetAccountListRequest.RequestHandler",
            "API Response is null or AccountList is null",
            string.Empty);

          responseData = new HCCGetAccountListResponseData(requestData, ex);
        }
      }
      catch (Exception ex)
      {
        responseData = new HCCGetAccountListResponseData(requestData, ex);
      }

      return responseData;
    }

    HCCAccounts GetHCCAccountResponse(HCCAPIWebService.AccountListResponse accountListResponse)
    {
      HCCAPIWebService.AccountListItem[] items = accountListResponse.AccountList;
      HCCAccounts hccAccountsResponse = new HCCAccounts(accountListResponse.Message, accountListResponse.Status, accountListResponse.StatusCode);      
      List<HCCAccount> accounts = new List<HCCAccount>(accountListResponse.AccountList.Length);

      foreach(HCCAPIWebService.AccountListItem item in items)
      {          
        HCCAccount account = new HCCAccount();

        account.HasAccountExecutive = item.AccountExec;
        account.AccountUid = item.AccountUID;
        account.BandwidthInMb = item.BandwidthInMB;
        account.DiskspaceInMb = item.DiskSpaceInMB;
        account.Domain = item.Domain;
        account.OperatingSystem = item.OperatingSystem;
        account.Plan = item.Plan;
        account.Status = item.Status;

        accounts.Add(account);
      }

      hccAccountsResponse.SetAccountList(accounts.AsReadOnly());

      return hccAccountsResponse;
    }
  }
}

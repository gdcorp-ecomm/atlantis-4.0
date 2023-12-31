﻿using System;
using System.Collections.Generic;
using System.Linq;
using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionGetShopperIdByIP.Impl.AccountQueriesService;
using Atlantis.Framework.OrionGetShopperIdByIP.Interface;
using Atlantis.Framework.OrionSecurityAuth.Interface;

namespace Atlantis.Framework.OrionGetShopperIdByIP.Impl
{
  public class OrionGetShopperIdByIPRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      OrionGetShopperIdByIPResponseData response = null;

      try
      {
        OrionSecurityAuthResponseData responseSecurityData = GetOrionAuthToken(requestData);
        var orionRequestData = (OrionGetShopperIdByIPRequestData)requestData;

        if (responseSecurityData.IsSuccess && !string.IsNullOrEmpty(responseSecurityData.AuthToken))
        {
          using (var accountServices = new AccountQueries())
          {
            accountServices.Url = ((WsConfigElement)config).WSURL;
            accountServices.Timeout = (int)orionRequestData.RequestTimeout.TotalMilliseconds;
            accountServices.SecureHeaderValue = new SecureHeader { Token = responseSecurityData.AuthToken };

            List<Account> accounts = SearchForAccounts(accountServices, "IPADDRESS", orionRequestData.IpToSearch);

            if (accounts.Count != 1)
            {
              accounts = SearchForAccounts(accountServices, "DEDICATED_IPADDRESS", orionRequestData.IpToSearch);

              if (accounts.Count == 0)
              {
                response = new OrionGetShopperIdByIPResponseData(shopperId: null);
              }
              else if (accounts.Count == 1)
              {
                var shopperId = accounts[0].CustomerNum ?? string.Empty;
                response = new OrionGetShopperIdByIPResponseData(shopperId);
              }
              else
              {
                var aex = new AtlantisException(requestData, "OrionGetShopperIdByIPResponse", "More than one account with this dedicated IP address is setup.  Possibly bad data in DB", "Dedicated IP: " + orionRequestData.IpToSearch);
                response = new OrionGetShopperIdByIPResponseData(aex);
              }
            }
            else
            {              
              var shopperId = accounts[0].CustomerNum ?? string.Empty;
              response = new OrionGetShopperIdByIPResponseData(shopperId);
            }
          }
        }
      }
      catch (AtlantisException aex)
      {
        response = new OrionGetShopperIdByIPResponseData(aex);
      }
      catch (Exception ex)
      {
        response = new OrionGetShopperIdByIPResponseData(requestData, ex);
      }

      return response;
    }

    #region Search For Accounts
    private List<Account> SearchForAccounts(AccountQueries accountServices, string elementType, string ipAddress)
    {
      string[] errors = { };
      string[] returnAttributeList = { };
      AccountQueryResponse accountList;

      var query = new AccountQuery();

      query.ProductName = string.Empty;
      query.SelectTop = 0;
      query.StartingId = 0;
      query.SystemNamespace = string.Empty;
      query.CustomerNum = string.Empty;
      query.ResellerId = -1;
      query.ShowRemoved = 0;
      query.cdtn = new AccountQueryCondition[] {
                              new AccountQueryCondition { 
                                    AttributeName= "server_data", 
                                    ElementName= elementType,
                                    ElementValue = ipAddress }
                         };

      accountServices.GetAccountListByQuery("1", query, returnAttributeList, out accountList, out errors);
      List<Account> accounts = accountList.AccountList.Where(a => a.Status.ToLower().Equals("setup")).ToList();

      return accounts;
    }

    #endregion

    #region OrionSecurity
    private static OrionSecurityAuthResponseData GetOrionAuthToken(RequestData accountRequestData)
    {
      var securityRequestData = new OrionSecurityAuthRequestData(accountRequestData.ShopperID,
                                                                 accountRequestData.SourceURL,
                                                                 accountRequestData.OrderID,
                                                                 accountRequestData.Pathway,
                                                                 accountRequestData.PageCount,
                                                                 "OrionGetShopperIdByIP");

      var responseSecurityData = (OrionSecurityAuthResponseData)DataCache.DataCache.GetProcessRequest(securityRequestData, securityRequestData.OrionSecurityAuthRequestType);

      return responseSecurityData;
    }
    #endregion
  }
}

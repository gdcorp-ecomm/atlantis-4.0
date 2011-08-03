using System;
using System.Xml.Linq;

using Atlantis.Framework.Interface;
using Atlantis.Framework.OrionGetAccountsByUID.Impl.AccountQueriesService;
using Atlantis.Framework.OrionGetAccountsByUID.Interface;
using Atlantis.Framework.OrionSecurityAuth.Interface;

namespace Atlantis.Framework.OrionGetAccountsByUID.Impl
{
  public class OrionGetAccountsByUIDRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData accountRequestData, ConfigElement config)
    {
      string[] _errors = { };
      int _resultCode = -1;
      Account[] _accountList = { };
      OrionGetAccountsByUIDResponseData responseData;

      try
      {
        OrionSecurityAuthResponseData responseSecurityData = GetOrionAuthToken(accountRequestData);

        var orionRequestData = (OrionGetAccountsByUIDRequestData)accountRequestData;

        if (responseSecurityData.IsSuccess && !string.IsNullOrEmpty(responseSecurityData.AuthToken))
        {
          using (var accountServices = new AccountQueries())
          {
            accountServices.Url = ((WsConfigElement) config).WSURL;
            accountServices.Timeout = (int) orionRequestData.RequestTimeout.TotalMilliseconds;
            accountServices.SecureHeaderValue = new SecureHeader {Token = responseSecurityData.AuthToken};

            _resultCode = accountServices.GetAccountListByAccountUid(orionRequestData.MessageId
                                                                     , orionRequestData.AccountUid
                                                                     , orionRequestData.ReturnAttributeList
                                                                     , out _accountList
                                                                     , out _errors);
          }
        }

        XElement getAccountListByAccountUidResponse = CreateOrionResponseXml(_resultCode, _accountList, _errors);

        responseData = new OrionGetAccountsByUIDResponseData(getAccountListByAccountUidResponse.ToString(), accountRequestData);
      }
      catch (AtlantisException exAtlantis)
      {
        responseData = new OrionGetAccountsByUIDResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new OrionGetAccountsByUIDResponseData(accountRequestData, ex);
      }

      return responseData;
    }

    #region XML Creation
    private static XElement CreateOrionResponseXml(int resultCode, Account[] accountList, string[] errors)
    {
      var getAccountListByAccountUidResponse = new XElement("GetAccountListByAccountUidResponse");
      getAccountListByAccountUidResponse.Add(new XElement("GetAccountListByAccountUidResult", resultCode));
      var xAccountList = new XElement("AccountList");

      if (resultCode == 0)
      {
        foreach (Account acct in accountList)
        {
          var xAcctAttributes = new XElement("AccountAttributes");
          foreach (AccountAttribute attr in acct.AccountAttributes)
          {
            var xAcctAttr = new XElement("AccountAttribute");
            xAcctAttr.Add(new XElement("AttributeId", attr.AttributeId));
            xAcctAttr.Add(new XElement("AttributeUid", attr.AttributeUid));
            xAcctAttr.Add(new XElement("DisplayStatus", attr.DisplayStatus));
            xAcctAttr.Add(new XElement("IsTemplateAttribute", attr.IsTemplateAttribute));
            xAcctAttr.Add(new XElement("Name", attr.Name));
            xAcctAttr.Add(new XElement("ProductAttributeUid", attr.ProductAttributeUid));
            xAcctAttr.Add(new XElement("Status", attr.Status));
            xAcctAttr.Add(new XElement("TemplateInternalName", attr.TemplateInternalName ?? string.Empty));


            var xAcctElements = new XElement("AccountElements");
            foreach (AccountElement acctElem in attr.AccountElements)
            {
              var xAcctElement = new XElement("AccountElement");
              xAcctElement.Add(new XElement("DisplayStatus", acctElem.DisplayStatus));
              xAcctElement.Add(new XElement("ElementId", acctElem.ElementId));
              xAcctElement.Add(new XElement("ElementUid", acctElem.ElementUid));
              xAcctElement.Add(new XElement("Name", acctElem.Name));
              xAcctElement.Add(new XElement("ProductAttributeElementUid", acctElem.ProductAttributeElementUid));
              xAcctElement.Add(new XElement("Status", acctElem.Status));
              xAcctElement.Add(new XElement("Value", acctElem.Value));
              xAcctElements.Add(xAcctElement);
            }
            xAcctAttr.Add(xAcctElements);
            xAcctAttributes.Add(xAcctAttr);
          }

          var xQuotas = new XElement("Quotas");
          if (acct.Quotas != null)
          {
            foreach (AccountQuota quota in acct.Quotas)
            {
              var xQuota = new XElement("AccountQuota");
              xQuota.Add(new XAttribute("ObjectUid", quota.ObjectUid));
              xQuota.Add(new XAttribute("objType", quota.objType.ToString()));
              xQuota.Add(new XAttribute("QuotaBeginPeriod", quota.QuotaBeginPeriod));
              xQuota.Add(new XAttribute("QuotaEndPeriod", quota.QuotaEndPeriod));
              xQuota.Add(new XAttribute("QuotaType", quota.QuotaType));

              var xDetails = new XElement("Details");
              foreach (AccountQuotaDetail elem in quota.Details)
              {
                xDetails.Add("AccountAttributeName", elem.AccountAttributeName);
                xDetails.Add("QuotaBeginPeriod", elem.QuotaBeginPeriod);
                xDetails.Add("QuotaEndPeriod", elem.QuotaEndPeriod);
                xDetails.Add("QuotaType", elem.QuotaType);
                xDetails.Add("ProductAttributeElementUid", elem.QuotaUsage);
              }
              xQuota.Add(xDetails);
            }
          }

          var xData = new XElement("data");
          if (acct.data != null)
          {
            foreach (AccountInternalData data in acct.data)
            {
              var xDatum = new XElement("AccountInternalData");
              xDatum.Add(new XAttribute("ItemName", data.ItemName));
              xDatum.Add(new XAttribute("ItemValue", data.ItemValue));
              xData.Add(xDatum);
            }
          }

          xAccountList.Add(
            new XElement("Account",
              new XElement("SystemNamespace", acct.SystemNamespace),
              new XElement("ResellerId", acct.ResellerId),
              new XElement("CustomerNum", acct.CustomerNum),
              new XElement("ProductName", acct.ProductName),
              new XElement("ProductTemplateId", acct.ProductTemplateId),
              new XElement("ProductTemplateName", acct.ProductTemplateName),
              new XElement("ProductUid", acct.ProductUid),
              new XElement("OrionCustomerId", acct.OrionCustomerId),
              new XElement("AccountId", acct.AccountId),
              new XElement("AccountUid", acct.AccountUid),
              new XElement("DisplayStatus", acct.DisplayStatus),
              new XElement("Status", acct.Status),
              new XElement("ExpireDate", acct.ExpireDate),
              new XElement("CanBeModified", acct.CanBeModified),
              new XElement("IsActive", acct.IsActive),
              new XElement("IsRemoved", acct.IsRemoved),
              new XElement(xAcctAttributes),
              new XElement(xQuotas),
              new XElement(xData)
            )
          );
        }
      }

      var xError = new XElement("errors");
      if (errors != null && errors.Length > 0)
      {
        foreach (string err in errors)
        {
          xError.Add(new XElement("error", err));
        }
      }
      getAccountListByAccountUidResponse.Add(xAccountList);
      getAccountListByAccountUidResponse.Add(xError);
      return getAccountListByAccountUidResponse;
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
                                                                 "OrionGetAccountsByUID");

      var responseSecurityData = (OrionSecurityAuthResponseData)DataCache.DataCache.GetProcessRequest(securityRequestData, securityRequestData.OrionSecurityAuthRequestType);

      return responseSecurityData;
    }
    #endregion
  }
}

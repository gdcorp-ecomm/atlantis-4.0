using System;
using System.Xml.Linq;
using Atlantis.Framework.EEMCreateNewAccount.Impl.CampaignBlazerWS;
using Atlantis.Framework.EEMCreateNewAccount.Interface;
using Atlantis.Framework.EEMGetQuotaAndPermissions.Interface;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMCreateNewAccount.Impl
{
  public class EEMCreateNewAccountRequest : IRequest
  {
    public IResponseData RequestHandler(RequestData requestData, ConfigElement config)
    {
      EEMCreateNewAccountResponseData responseData = null;
      int quota;
      int permissions;

      try
      {
        var request = (EEMCreateNewAccountRequestData)requestData;
        EEMGetQuotaAndPermissions(request, out quota, out permissions);
        int customerId = 0;

        using (CampaignBlazer eemWs = new CampaignBlazer())
        {
          eemWs.Url = ((WsConfigElement)config).WSURL;
          eemWs.Timeout = (int)request.RequestTimeout.TotalMilliseconds;
          customerId = eemWs.CreateNewAccount(CreateNewAccountInputXml(request, quota, permissions));
        }
        responseData = new EEMCreateNewAccountResponseData(customerId);
      }

      catch (AtlantisException exAtlantis)
      {
        responseData = new EEMCreateNewAccountResponseData(exAtlantis);
      }

      catch (Exception ex)
      {
        responseData = new EEMCreateNewAccountResponseData(requestData, ex);
      }

      return responseData;
    }

    #region New Account XML
    private string CreateNewAccountInputXml(EEMCreateNewAccountRequestData request, int quota, int permissions)
    {
      XElement customer = new XElement("Customer",
        new XElement("shopper_id", request.ShopperID),
        new XElement("private_label_id", request.PrivateLabelId.ToString()),
        new XElement("billing_date", request.StartDate.ToString()),
        new XElement("customer_billing_type_id", request.BillingType.ToString()),
        new XElement("email_limit", quota.ToString()),
        new XElement("permissions", permissions.ToString()));

      if (request.ResellerPrivateLabelId > 0)
      {
        customer.Add(new XElement("reseller_private_label_id", request.ResellerPrivateLabelId.ToString()));
      }

      return customer.ToString();

    }
    #endregion

    #region Quotas & Permissions
    private void EEMGetQuotaAndPermissions(EEMCreateNewAccountRequestData request, out int quota, out int permissions)
    {
      EEMGetQuotaAndPermissionsResponseData response; 
      var quotaRequest = new EEMGetQuotaAndPermissionsRequestData(request.ShopperID
        , request.SourceURL
        , request.OrderID
        , request.Pathway
        , request.PageCount
        , request.Pfid);

      int requestType = request.UpdatedEEMQuotaAndPermissionsRequestType.HasValue ? request.UpdatedEEMQuotaAndPermissionsRequestType.Value : quotaRequest.EEMQuotaAndPermissionsRequestType;

      try
      {
        response = DataCache.DataCache.GetProcessRequest(quotaRequest, requestType) as EEMGetQuotaAndPermissionsResponseData;     
      }
      catch (Exception ex)
      {
        throw new AtlantisException(quotaRequest
          , "EEMCreateNewAccountRequest::EEMGetQuotaAndPermissions"
          , ex.Message
          , string.Empty
          , ex);
      }

      quota = 0;
      permissions = 0;
      if (response.IsSuccess)
      {
        quota = response.Quota;
        permissions = response.Permissions;
      }
    }
    #endregion
  }
}

using System;
using System.Security.Cryptography;
using Atlantis.Framework.Interface;

namespace Atlantis.Framework.EEMCreateNewAccount.Interface
{
  public class EEMCreateNewAccountRequestData : RequestData
  {
    #region Properties
    private const int RESELLER_PRODUCT_TYPE_ID = 56;

    public int BillingResourceId { get; private set; }
    public int ParentBundleId { get; private set; }
    public int ParentBundleTypeId { get; private set; }
    public int Pfid { get; private set; }
    public int PrivateLabelId { get; private set; }
    public string RecurringPaymentType { get; private set; }
    public DateTime StartDate { get; private set; }

    public int ResellerPrivateLabelId
    {
      get
      {
        int rplId = -1;
        if (ParentBundleTypeId.Equals(RESELLER_PRODUCT_TYPE_ID))
        {
          rplId = ParentBundleId;
        }
        return rplId;
      }
    }

    public int BillingType
    {
      get
      {
        int billingType;
        switch (RecurringPaymentType.ToLowerInvariant())
        {
          case "annual":
            billingType = 3;
            break;
          case "monthly":
            billingType = 2;
            break;
          case "bulk":
            billingType = 1;
            break;
          case "none":
          default:
            billingType = 0;
            break;
        }
        return billingType;
      }
    }

    public int? UpdatedEEMQuotaAndPermissionsRequestType { get; set; }
    #endregion

    public EEMCreateNewAccountRequestData(string shopperId
      , string sourceUrl
      , string orderId
      , string pathway
      , int pageCount
      , int billingResourceId
      , int parentBundleId
      , int parentBundleTypeId
      , int pfid
      , int privateLabelId
      , string recurringPaymentType
      , DateTime startDate)
      : base(shopperId, sourceUrl, orderId, pathway, pageCount)
    {
      BillingResourceId = billingResourceId;
      ParentBundleId = parentBundleId;
      ParentBundleTypeId = parentBundleTypeId;
      Pfid = pfid;
      PrivateLabelId = privateLabelId;
      RecurringPaymentType = recurringPaymentType;
      StartDate = startDate;
      RequestTimeout = TimeSpan.FromSeconds(5);
    }

    public override string GetCacheMD5()
    {
      throw new NotImplementedException("GetCacheMD5 not implemented in EEMCreateNewAccountRequestData");
    }
  }
}

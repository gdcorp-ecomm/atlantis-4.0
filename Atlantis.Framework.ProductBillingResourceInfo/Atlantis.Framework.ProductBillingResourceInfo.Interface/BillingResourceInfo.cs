using System;

namespace Atlantis.Framework.ProductBillingResourceInfo.Interface
{
  public class BillingResourceInfo
  {
    public int ResourceId { get; set; }
    public string Namespace { get; set; }
    public int ProfileId { get; set; }
    public string ProductDescription { get; set; }
    public string Info { get; set; }
    public DateTime BillingDate { get; set; }
    public string OrderId { get; set; }
    public string RenewalSKU { get; set; }
    public int IsLimited { get; set; }
    public int AutoRenewFlag { get; set; }
    public int PFID { get; set; }
    public int AllowRenewals { get; set; }
    public string RecurringPayment { get; set; }
    public int NumberOfPeriods { get; set; }
    public int RenewalPFID { get; set; }
    public int IsPastDue { get; set; }
    public DateTime UsageStartDate { get; set; }
    public DateTime UsageEndDate { get; set; }
    public string ExternalResourceId { get; set; }
    public int PurchasedDuration { get; set; }
    public int ProductTypeId { get; set; }
    public int IsPrivacyPlusDomain { get; set; }
    public string MostRecentRenewalOrderId { get; set; }
    public DateTime MostRecentRenewalOrderDate { get; set; }

    public BillingResourceInfo(int resourceId, string nameSpace, int profileId, string productDescription, string info, DateTime billingDate, string orderId,
      string renewalSku, int isLimited, int autoRenewFlag, int pfid, int allowRenewals, string recurringPayment, int numberOfPeriod, int renewalPfid, int isPastDue,
      DateTime usageStartDate, DateTime usageEndDate, string externalResourceId, int purchaseDuration, int productTypeId, int isPrivacyPlusDomain, string mostRecentRenewalOrderId, DateTime mostRecentRenewalOrderDate)
    {
      ResourceId = resourceId;
      Namespace = nameSpace;
      ProfileId = profileId;
      ProductDescription = productDescription;
      Info = info;
      BillingDate = billingDate;
      OrderId = orderId;
      RenewalSKU = renewalSku;
      IsLimited = isLimited;
      AutoRenewFlag = autoRenewFlag;
      PFID = pfid;
      AllowRenewals = allowRenewals;
      RecurringPayment = recurringPayment;
      NumberOfPeriods = numberOfPeriod;
      RenewalPFID = renewalPfid;
      IsPastDue = isPastDue;
      UsageStartDate = usageStartDate;
      UsageEndDate = usageEndDate;
      ExternalResourceId = externalResourceId;
      PurchasedDuration = purchaseDuration;
      ProductTypeId = productTypeId;
      IsPrivacyPlusDomain = isPrivacyPlusDomain;
      MostRecentRenewalOrderId = mostRecentRenewalOrderId;
      MostRecentRenewalOrderDate = mostRecentRenewalOrderDate;
    }

  }
}

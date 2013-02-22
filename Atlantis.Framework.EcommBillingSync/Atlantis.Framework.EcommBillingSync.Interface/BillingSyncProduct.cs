using System;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class BillingSyncProduct
  {
    public bool AllowRenewals { get; private set; }
    public int BillingResourceId { get; private set; }
    public string Namespace { get; private set; }
    public DateTime OriginalBillingDate { get; private set; }
    public string OrionId { get; private set; }
    public string ProductNameInfo { get; private set; }
    public string RecurringPaymentType { get; private set; }
    public int RenewalProductId { get; private set; }
    public int RenewalPeriods { get; private set; }

    public BillingSyncProduct(int allowRenewals, int billingResourceId, string nameSpace, string orionId, DateTime originalBillingDate, string recurringPaymentType, int renewalProductId, int renewalPeriods, string productNameInfo)
    {
      AllowRenewals = allowRenewals.Equals(1);
      BillingResourceId = billingResourceId;
      Namespace = nameSpace;
      OriginalBillingDate = originalBillingDate;
      OrionId = orionId;
      ProductNameInfo = productNameInfo;
      RecurringPaymentType = recurringPaymentType;
      RenewalProductId = renewalProductId;
      RenewalPeriods = renewalPeriods;
    }
  }
}

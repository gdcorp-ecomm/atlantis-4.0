using System;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class BillingSyncProduct
  {
    public bool AllowRenewals { get; private set; }
    public int BillingResourceId { get; private set; }
    public DateTime OriginalBillingDate { get; private set; }
    public string OrionId { get; private set; }
    public string RecurringPaymentType { get; private set; }
    public int RenewalPfId { get; private set; }
    public int RenewalPeriods { get; private set; }

    public BillingSyncProduct(int allowRenewals, int billingResourceId, string orionId, DateTime originalBillingDate, string recurringPaymentType, int renewalPfId, int renewalPeriods)
    {
      AllowRenewals = allowRenewals.Equals(1);
      BillingResourceId = billingResourceId;
      OriginalBillingDate = originalBillingDate;
      OrionId = orionId;
      RecurringPaymentType = recurringPaymentType;
      RenewalPfId = renewalPfId;
      RenewalPeriods = renewalPeriods;
    }
  }
}

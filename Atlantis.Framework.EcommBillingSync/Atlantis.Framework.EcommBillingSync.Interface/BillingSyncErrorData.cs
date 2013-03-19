using System;

namespace Atlantis.Framework.EcommBillingSync.Interface
{
  public class BillingSyncErrorData
  {
    #region Enum & Properties
    public enum BillingSyncErrorType
    {
      InvalidSyncDate,
      RenewalNotAllowedByFlag,
      RenewalNotAllowedByRecurringType,
      BillingStateSvcError,
      BasketSvcError,
      GdOverrideLibSvcError,
      UnifiedProductIdLookupError
    }

    public BillingSyncErrorType ErrorType { get; private set; }
    public Exception Error { get; private set; }
    public BillingSyncProduct OffendingProduct { get; private set; }
    #endregion

    public BillingSyncErrorData(BillingSyncErrorType errorType, Exception error, BillingSyncProduct offendingProduct = null)
    {
      ErrorType = errorType;
      Error = error;
      OffendingProduct = offendingProduct;
    }
  }
}

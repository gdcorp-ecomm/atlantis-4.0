
namespace Atlantis.Framework.MyaProduct.Interface
{
  public class MyaProductRenewalOption
  {
    public string RenewalProductId { get; private set; }
    public int NumberOfPeriods { get; private set; }
    public RecurringPaymentType RecurringPayment { get; private set; }

    internal MyaProductRenewalOption(string renewalProductId, int numberOfPeriods, RecurringPaymentType recurringPaymentType)
    {
      RenewalProductId = renewalProductId;
      NumberOfPeriods = numberOfPeriods;
      RecurringPayment = recurringPaymentType;
    }
  }
}


namespace Atlantis.Framework.Providers.Interface.Products
{
  public interface IProductInfo
  {
    string Name { get; }
    int NumberOfPeriods { get; }
    int ProductTypeId { get; }
    RecurringPaymentUnitType RecurringPayment { get; }
  }
}

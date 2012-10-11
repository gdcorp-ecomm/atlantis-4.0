
namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductPackageChildProduct
  {
    int ProductId { get; }

    int Quantity { get; }

    double Duration { get; }

    bool ChildFlag { get; }

    bool IsFree { get; }

    string DiscountCode { get; }
  }
}

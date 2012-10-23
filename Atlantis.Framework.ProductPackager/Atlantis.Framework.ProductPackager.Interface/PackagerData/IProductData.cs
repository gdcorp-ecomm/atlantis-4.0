
namespace Atlantis.Framework.ProductPackager.Interface
{
  public interface IProductData
  {
    int ProductId { get; }

    int Quantity { get; }

    double Duration { get; }

    bool IsDefault { get; }
  }
}

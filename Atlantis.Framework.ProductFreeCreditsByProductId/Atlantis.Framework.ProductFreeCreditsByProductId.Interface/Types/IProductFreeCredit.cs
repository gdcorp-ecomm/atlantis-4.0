
namespace Atlantis.Framework.ProductFreeCreditsByProductId.Interface.Types
{
  public interface IProductFreeCredit
  {
    int UnifiedProductId { get; set; }
    int Quantity { get; set; }
    string ProductNamespace { get; set; }
  }
}


namespace Atlantis.Framework.ProductFreeCreditsByProductId.Interface.Types
{
  public class ProductFreeCredit : IProductFreeCredit
  {
    public int UnifiedProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductNamespace { get; set; }
  }
}

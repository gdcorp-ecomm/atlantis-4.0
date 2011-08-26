using Atlantis.Framework.ProductFreeCreditsByProductId.Interface.Types;

namespace Atlantis.Framework.ProductFreeCreditsByResource.Interface.Types
{
  public class ResourceFreeCredit : IProductFreeCredit
  {
    public int UnifiedProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductNamespace { get; set; }
  }
}

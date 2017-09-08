
namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  public class ProductPlan
  {
    public string TreeId { get; private set; }
    public string UnifiedProductId { get; private set; }
    public bool IsCurrent { get; private set; }
    public bool IsFree { get; private set; }
    public string TreeTypeId { get; private set; }

    public ProductPlan(string treeId, string treeTypeId, string unifiedProductId, bool isFree, bool isCurrent)
    {
      TreeId = treeId;
      TreeTypeId = treeTypeId;
      UnifiedProductId = unifiedProductId;
      IsCurrent = isCurrent;
      IsFree = isFree;
    }
  }
}

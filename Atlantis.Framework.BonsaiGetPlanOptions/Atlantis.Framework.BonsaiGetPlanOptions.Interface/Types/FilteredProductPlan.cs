
namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  public class FilteredProductPlan : ProductPlan
  {
    public int ReasonCode { get; private set; }
    public string ReasonMessage { get; set; }

    public FilteredProductPlan(string treeId, string unifiedProductId, int reasonCode, string reasonMessage, bool isFree)
      : base(treeId, unifiedProductId, isFree, isCurrent:false)
    {
      ReasonCode = reasonCode;
      ReasonMessage = reasonMessage;
    }
  }
}

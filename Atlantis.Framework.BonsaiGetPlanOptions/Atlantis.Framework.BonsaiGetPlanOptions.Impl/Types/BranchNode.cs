using System.Collections.Generic;
using Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Impl.Types
{
  internal class BranchNode : ProductAddon
  {
    public List<string> TransitionNodeIds { get; private set; }
    public bool IsVisited { get; set; }

    public BranchNode(string branchId, string nodeId, int categoryId, bool isCurrent, bool isFinal,
                      bool isDefault, int currentQty, string unifiedProductId, bool isQuantityBased,
                      int minQty, int maxQty, int minDuration, int maxDuration, int increment,
                      List<string> transitionIDs)
      : base(branchId, nodeId, categoryId, isCurrent, isFinal, isDefault, currentQty, unifiedProductId,
             isQuantityBased, minQty, maxQty, minDuration, maxDuration, increment)
    {
      TransitionNodeIds = transitionIDs;
      IsVisited = false;
    }

    public ProductAddon ToProductAddon()
    {
      var newAddon = new ProductAddon(this.BranchId, this.NodeId, this.CategoryId, this.IsCurrent,
                                      this.IsFinal, this.IsDefault, this.CurrentQty, this.UnifiedProductId,
                                      this.IsQuantityBased, this.MinQuantity, this.MaxQuantity,
                                      this.MinDuration, this.MaxDuration, this.Increment);
      foreach (var childAddon in this.ChildAddons)
      {
        newAddon.ChildAddons.AddProductAddons(childAddon.Key, childAddon.Value);
      }

      return newAddon;
    }
  }
}

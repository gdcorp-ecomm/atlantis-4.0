using System.Runtime.Serialization;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  [DataContract]
  public class ProductAddon : AddonBase
  {
    public int CategoryId { get; private set; }
    [DataMember(Name = "BId")]
    public string BranchId { get; private set; }
    [DataMember(Name = "NId")]
    public string NodeId { get; private set; }
    [DataMember(Name = "Owned")]
    public bool IsCurrent { get; private set; }
    [DataMember(Name = "Final")]
    public bool IsFinal { get; private set; }
    [DataMember(Name = "Default")]
    public bool IsDefault { get; private set; }
    [DataMember(Name = "Qty")]
    public int CurrentQty { get; private set; }
    [DataMember(Name = "ChildAddons")]
    public CategoryAddonCollection ChildAddons { get; private set; }

    public ProductAddon (string branchId, string nodeId, int categoryId, bool isCurrent, bool isFinal,
                         bool isDefault, int currentQty, string unifiedProductId, bool isQuantityBased,
                         int minQty, int maxQty, int minDuration, int maxDuration, int increment)
      : base(unifiedProductId, isQuantityBased, minQty, maxQty, minDuration, maxDuration, increment)
    {
      BranchId = branchId;
      NodeId = nodeId;
      CategoryId = categoryId;
      IsCurrent = isCurrent;
      IsFinal = isFinal;
      IsDefault = isDefault;
      CurrentQty = currentQty;
      ChildAddons = new CategoryAddonCollection();
    }
  }
}

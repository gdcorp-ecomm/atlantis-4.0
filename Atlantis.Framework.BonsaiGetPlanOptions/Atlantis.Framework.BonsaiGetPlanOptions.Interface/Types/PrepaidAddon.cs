
namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  public class PrepaidAddon : AddonBase
  {
    public string Name { get; private set; }

    public PrepaidAddon(string name, string unifiedProductId, bool isQuantityBased, int minQty, int maxQty, int minDuration, int maxDuration, int increment)
      : base(unifiedProductId, isQuantityBased, minQty, maxQty, minDuration, maxDuration, increment)
    {
      Name = name;
    }
  }
}

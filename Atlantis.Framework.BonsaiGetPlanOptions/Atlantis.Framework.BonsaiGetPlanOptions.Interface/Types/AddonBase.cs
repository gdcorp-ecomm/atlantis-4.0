using System.Runtime.Serialization;

namespace Atlantis.Framework.BonsaiGetPlanOptions.Interface.Types
{
  [DataContract]
  public class AddonBase
  {
    [DataMember(Name = "UpId")]
    public string UnifiedProductId { get; private set; }
    [DataMember(Name = "QtyBased")]
    public bool IsQuantityBased { get; private set; }
    [DataMember(Name = "MinQty")]
    public int MinQuantity { get; private set; }
    [DataMember(Name = "MaxQty")]
    public int MaxQuantity { get; private set; }
    [DataMember(Name = "MinDur")]
    public int MinDuration { get; private set; }
    [DataMember(Name = "MaxDur")]
    public int MaxDuration { get; private set; }
    [DataMember(Name = "Incr")]
    public int Increment { get; private set; }

    public AddonBase(string unifiedProductId, bool isQuantityBased, int minQty, int maxQty, int minDuration, int maxDuration, int increment)
    {
      UnifiedProductId = unifiedProductId;
      IsQuantityBased = isQuantityBased;
      MinQuantity = minQty;
      MaxQuantity = maxQty;
      MinDuration = minDuration;
      MaxDuration = maxDuration;
      Increment = increment;
    }
  }
}

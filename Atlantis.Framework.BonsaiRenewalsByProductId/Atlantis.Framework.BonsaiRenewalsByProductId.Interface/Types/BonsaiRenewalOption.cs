
namespace Atlantis.Framework.BonsaiRenewalsByProductId.Interface.Types
{
  public class BonsaiRenewalOption
  {
    public int UnifiedProductId { get; private set; }
    public int RenewalLength { get; private set; }
    public string RenewalType { get; private set; }
    public int MinPeriods { get; private set; }
    public int MaxPeriods { get; private set; }

    public BonsaiRenewalOption(int unifiedProductId, int renewalLength, string renewalType, int minPeriods, int maxPeriods)
    {
      UnifiedProductId = unifiedProductId;
      RenewalLength = renewalLength;
      RenewalType = renewalType;
      MinPeriods = minPeriods;
      MaxPeriods = maxPeriods;
    }
  }
}

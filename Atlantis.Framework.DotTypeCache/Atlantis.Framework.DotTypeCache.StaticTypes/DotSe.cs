using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotSe : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {

      //57701 AND 57799
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 57701, 57702, 57703, 57704, 57705, 57706, 57707, 57708, 57709, 57710 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 57720, 57736, 57742, 57760, 57748, 57766, 57772, 57778, 57784, 57754 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 57721, 57737, 57743, 57761, 57749, 57767, 57773, 57779, 57785, 57755 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 57724, 57738, 57744, 57762, 57750, 57768, 57774, 57780, 57786, 57756 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 57726, 57739, 57745, 57763, 57751, 57769, 57775, 57781, 57787, 57757 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 57728, 57740, 57746, 57764, 57752, 57770, 57776, 57782, 57788, 57758 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 57711, 57712, 57713, 57714, 57715, 57716, 57717, 57718, 57719 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 57729 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 57730 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 57732 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 57733 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 57734 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 67701, 67702, 67703, 67704, 67705, 67706, 67707, 67708, 67709, 67710 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 67720, 67736, 67742, 67760, 67748, 67766, 67772, 67778, 67784, 67754 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 67721, 67737, 67743, 67761, 67749, 67767, 67773, 67779, 67785, 67755 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 67724, 67738, 67744, 67762, 67750, 67768, 67774, 67780, 67786, 67756 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 67726, 67739, 67745, 67763, 67751, 67769, 67775, 67781, 67787, 67757 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 67728, 67740, 67746, 67764, 67752, 67770, 67776, 67782, 67788, 67758 });
      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "SE"; }
    }

    public override int MaxRenewalLength
    {
      get
      {
        return 1;
      }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }

    public override int MaxTransferLength
    {
      get
      {
        return 1;
      }
    }
  }
}

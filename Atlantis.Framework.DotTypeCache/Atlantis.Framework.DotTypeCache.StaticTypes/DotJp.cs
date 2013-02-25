using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotJp : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12701, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12730, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12730, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12732, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12734, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12736, 12702, 12703, 12704, 12705, 12706, 12707, 12708, 12709, 12710 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12711, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 12740, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 12740, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 12741, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 12742, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 12743, 12722, 12723, 12724, 12725, 12726, 12727, 12728, 12729 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 12712, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 22730, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 22730, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 22732, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 22734, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 22736, 12713, 12714, 12715, 12716, 12717, 12718, 12719, 12720, 12721 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "JP"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 1; }
    }

    public override int MaxRenewalLength
    {
      get { return 1; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 24; }
    }
  }
}

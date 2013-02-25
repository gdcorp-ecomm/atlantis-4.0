using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotTk : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 40701, 40702, 40703, 40704, 40705, 40706, 40707, 40708, 40709, 40710 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      return null;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 50701, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 50720, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 50720, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 50722, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 50724, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 50726, 50702, 50703, 50704, 50705, 50706, 50707, 50708, 50709, 50710 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "TK"; }
    }

    public override int MinRegistrationLength
    {
      get { return 2; }
    }

    public override int MaxRegistrationLength
    {
      get { return 2; }
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

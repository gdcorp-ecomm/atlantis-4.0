using Atlantis.Framework.DotTypeCache.Static;

namespace Atlantis.Framework.DotTypeCache.StaticTypes
{
  public class DotNetDotCn : StaticDotType
  {
    protected override StaticDotTypeTiers InitializeRegistrationProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14301, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14330, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14330, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14332, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14334, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14336, 14302, 14303, 14304, 14305, 14306, 14307, 14308, 14309, 14310 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Register, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeTransferProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14311, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 14340, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 14340, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 14341, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 14342, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 14343, 14322, 14323, 14324, 14325, 14326, 14327, 14328, 14329 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Transfer, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    protected override StaticDotTypeTiers InitializeRenewalProductIds()
    {
      StaticDotTypeTier DotTypeTier0 = new StaticDotTypeTier(0, new int[] { 14312, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      StaticDotTypeTier DotTypeTier6to20 = new StaticDotTypeTier(6, new int[] { 24330, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      StaticDotTypeTier DotTypeTier21to49 = new StaticDotTypeTier(21, new int[] { 24330, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      StaticDotTypeTier DotTypeTier50to100 = new StaticDotTypeTier(50, new int[] { 24332, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      StaticDotTypeTier DotTypeTier101to200 = new StaticDotTypeTier(101, new int[] { 24334, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });
      StaticDotTypeTier DotTypeTier201andup = new StaticDotTypeTier(201, new int[] { 24336, 14313, 14314, 14315, 14316, 14317, 14318, 14319, 14320, 14321 });

      StaticDotTypeTiers result = new StaticDotTypeTiers(StaticDotTypeProductIdTypes.Renewal, new StaticDotTypeTier[] { DotTypeTier0, DotTypeTier6to20, DotTypeTier21to49, DotTypeTier50to100, DotTypeTier101to200, DotTypeTier201andup });
      return result;
    }

    public override string DotType
    {
      get { return "NET.CN"; }
    }

    public override int MaxRegistrationLength
    {
      get { return 5; }
    }

    public override int MaxRenewalLength
    {
      get { return 5; }
    }

    protected override int MaxRenewalMonthsOut
    {
      get { return 72; }
    }
  }
}
